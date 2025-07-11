using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [HideInInspector]
    public RoomTemplates roomTemplates;

    [HideInInspector]
    public MapTemplates mapTemplates;

    [HideInInspector]
    public TileBaseTemplate tileBaseTemplate { get; set; }

    [HideInInspector]
    public EnemyTemplates enemyTemplates;


    [SerializeField] bool spawn;

    [field: SerializeField] public int addedRoom { get; private set; }                   // 각 경로당 추가 생성할 방
    [field: SerializeField] public int turning { get; private set; }                     // 꺾을 횟수
    [field: SerializeField] public int crossedRoom { get; private set; }                 // 갈림길 방의 수
    [field: SerializeField] public int defaultMaxRoom { get; private set; }              // 기본 최대방의 수
    [field: SerializeField] public int maxRoom { get; private set; }                    // 최대방의 수 [기본 상태 0]
    [field: SerializeField] public int roomSize { get; private set; }                    // 방 크기 배율 기본 : 3
    [field: SerializeField] public int area {get; private set;}                        // 방 상화 좌우 영역 기본 : 5
    public List<GameObject> rooms;
    [field: SerializeField] public bool finish{get; private set;}                     // 맵 생성 완료 보기용

    public Camera miniMapCamera;
    public GameObject hideMap;
    GameObject roomParent;

    bool spawning;
    int preCount = 0;
    float waitTime = 0;
    int crossedRoomCount = 0;

    void Start()
    {
        //templates 설정
        roomTemplates = GetComponent<RoomTemplates>();
        mapTemplates = GetComponent<MapTemplates>();
        //enemyTemplates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<EnemyTemplates>();
        enemyTemplates = GameManager.instance.enemyTemplates;

        // room parent!!!!
        roomParent = new GameObject("Rooms");
        roomParent.tag = "roomParent";
        FindObj.instance.roomParent= roomParent;

        //Minimap camera
        miniMapCamera.cullingMask = 1 << LayerMask.NameToLayer("MiniMapOnly");
        maxRoom = defaultMaxRoom;

        finish = false;
        spawning = false;
    }

    void Update()
    {
        Spawn();
        FinishCheck();
    }

    // 방의 업데이트가 끝나고 1초 후 방 생성 완료
    void FinishCheck()
    {
        if(spawning)
        {
            if (preCount != rooms.Count)
            {
                waitTime = 0;
                preCount = rooms.Count;
                return;
            }

            if (waitTime >= 1)
            {
                spawning = false;
                if (rooms.Count < maxRoom)
                {
                    spawn = true;
                    return;
                }
                SetMapType();
                finish = true;
                setMinimapCamera();
            }
            else
            {
                waitTime += Time.deltaTime;
            }
        }
    }

    
    // 초기 위치 설정
    void Spawn()
    {
        if (spawn)
        {
            // 생성중이면 무시
            if(spawning)
            {
                spawn = false;
                return;
            }

            //초기화
            finish = false;
            spawning = true;
            waitTime = 0;
            crossedRoomCount = 0;
            maxRoom = defaultMaxRoom;
            crossedRoomCount = 0;

            // 방 삭제
            for (int i = 0; i < rooms.Count; i++)
            {
                Destroy(rooms[i]);
            }
            rooms.Clear();
            
            // 시작 방향 설정
            int dir = Random.Range(1, 5);
            Vector2 startPoisition = new Vector2(Random.Range(-2, 3) * 10 * roomSize, Random.Range(-2, 3) * 10 * roomSize);

            GameObject instObj=null;

            if (dir == 1)
            {
                instObj = Instantiate(roomTemplates.bottomRooms[0], startPoisition, Quaternion.identity);
            }
            else if (dir == 2)
            {
                instObj = Instantiate(roomTemplates.topRooms[0], startPoisition, Quaternion.identity);
                
            }
            else if (dir == 3)
            {
                instObj = Instantiate(roomTemplates.rightRooms[0], startPoisition, Quaternion.identity);
            }
            else if (dir == 4)
            {
                instObj = Instantiate(roomTemplates.leftRooms[0], startPoisition, Quaternion.identity);
            }

            instObj.transform.localScale = new Vector3(roomSize, roomSize, 1);
            instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;


            spawn = false;
            Invoke("Crossed", 0.1f * defaultMaxRoom + 0.1f);
        }
        
    }

    // 갈림길 설정
    void Crossed()
    {
        maxRoom = defaultMaxRoom + (crossedRoom * (addedRoom - 1));
        for (int i = 0; i < defaultMaxRoom * 2; i++)
        {
            if (crossedRoomCount >= crossedRoom)
                break;

            int index = Random.Range(1, rooms.Count - 1);
            GameObject crossedRoomGameObject = rooms[index];
            Room crossedRoomGameObjectRoom = crossedRoomGameObject.GetComponent<Room>();

            crossedRoomGameObject.SetActive(false);
            GameObject instObj = null;

            if (crossedRoomGameObjectRoom.top && crossedRoomGameObjectRoom.bottom)
            {
                instObj = Instantiate(roomTemplates.verticalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation);
                instObj.transform.localScale = new Vector3(roomSize, roomSize, 1);
                instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;
            }
            else if (crossedRoomGameObjectRoom.left && crossedRoomGameObjectRoom.right)
            {
                instObj = Instantiate(roomTemplates.horizontalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation);
                instObj.transform.localScale = new Vector3(roomSize, roomSize, 1);
                instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;
            }
            else
            {
                crossedRoomGameObject.SetActive(true);
                continue;
            }

            crossedRoomCount++;
            Destroy(crossedRoomGameObject);
            rooms.RemoveAt(index);
        }
    }

    // 생성된 방들의 타입을 설정한다.
    void SetMapType()
    {
        bool isBoss = false;
        Room room = rooms[0].GetComponent<Room>();
        room.SetMapManager(MapType.Default);

        // 모든 방 생성을 완료하면 각 방들의 용도를 설정한다.
        // 첫번째 방은 몬스터 없음
        for (int i = 1; i < rooms.Count; i++)
        {
            room = rooms[i].GetComponent<Room>();

            int roomType = room.GetRoomWayType();

            // 1. 통로가 없는 방이라면
            // 1-1 무조건 보상방
            if (roomType == 0)
            {
                room.SetMapManager(MapType.Default);
            }
            // 2. 통로가 하나인 방이라면
            // 2-1 미션방
            // 2-2 보스방 : 세팅되어있지않다면 보스방 세팅
            else if (roomType == 1)
            {
                if (!isBoss)
                {
                    room.SetMapManager(MapType.Boss);
                    isBoss = true;
                    continue;
                }
                room.SetMapManager(MapType.Mission);
            }
            // 3. 통로가 2개인 방이라면
            // 3-1 일반방 
            // 3-2 보상방 : 낮은 확률로 적이 없는 잠시 휴식을 위한 방
            else if (roomType == 2)
            {
                int ran = Random.Range(0, 50);
                //if(ran == 0)
                //{
                //    room.SetMapManager(MapType.Reward);
                //    continue;
                //}
                room.SetMapManager(MapType.Default);
            }
            // 4. 통로가 3개인 방이라면
            // 4-1 보상방 : 적이 없는 잠시 휴식을 위한 방
            else if (roomType == 3)
            {
                room.SetMapManager(MapType.Reward);
            }
        }
    }

    public MapType ResetMapType(int _WayType)
    {

        // 2. 통로가 하나인 방이라면
        // 2-1 미션방
        // 2-2 보스방 : 세팅되어있지않다면 보스방 세팅
        if (_WayType == 1)
        {
            return MapType.Mission;
        }
        // 3. 통로가 2개인 방이라면
        // 3-1 일반방 
        // 3-2 보상방 : 낮은 확률로 적이 없는 잠시 휴식을 위한 방
        else if (_WayType == 2)
        {
            return MapType.Default;
        }
        // 4. 통로가 3개인 방이라면
        // 4-1 보상방 : 적이 없는 잠시 휴식을 위한 방
        else if (_WayType == 3)
        {
            return MapType.Reward;
        }

        return MapType.Default;
    }

    public Transform GetBossRoomPos()
    {
        // 첫번째 1개 통로방은 무조건 보스방임
        for( int i = 1 ; i< rooms.Count ; i++)
        {
            Room room = rooms[i].GetComponent<Room>();
            if(room.GetRoomWayType() == 1)
            {
                return room.gameObject.transform;
            }
        }
        return null;
    }


    //맵의 중앙을 찾아서 미니맵 카메라 이동
    void setMinimapCamera()
    {
        List<GameObject> maps=new List<GameObject>();//random 생성된 맵들

        //Vector2 middlePos=Vector2.zero;

        for(int i=0;i<roomParent.transform.childCount;i++) 
        {
            maps.Add(roomParent.transform.GetChild(i).gameObject);
            //middlePos += (Vector2)roomParent.transform.GetChild(i).transform.position;
        }

        /*
        if (roomParent.transform.childCount > 0)
        {
            middlePos /= roomParent.transform.childCount;
        }

        miniMapCamera.transform.position = middlePos;
        miniMapCamera.transform.position += new Vector3(0,0,-5);
        */

        //add


        // Calculate bounds (all maps)
        Bounds bounds = new Bounds(maps[0].transform.position, Vector3.zero);
        foreach (GameObject map in maps)
        {
            //bounds.Encapsulate(map.GetComponent<Renderer>().bounds);
            bounds.Encapsulate(map.GetComponent<BoxCollider2D>().bounds);
        }

        Vector3 center = bounds.center;//maps center
        Vector3 size = bounds.size;//camera size

        // set camera position
        miniMapCamera.transform.position = new Vector3(center.x, center.y, -10f);

        // set camera size
        float largestSize = Mathf.Max(size.x, size.y);
        miniMapCamera.orthographicSize = largestSize / 2;

    }

    /*
    void HidingMap()
    {
        GameObject hide;
        for (int i = 0; i < roomParent.transform.childCount; i++)
        {
            hide=Instantiate(hideMap);
            hide.transform.parent=roomParent.transform.GetChild(i).transform;
            hide.transform.localScale = new Vector3(roomSize*roomSize, roomSize * roomSize, 1);
            hide.transform.position = roomParent.transform.GetChild(i).position;
        }

    }
    */
}
