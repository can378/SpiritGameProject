using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomTemplates roomTemplates;
    public MapTemplates mapTemplates;
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
        roomTemplates = FindObj.instance.roomManager.GetComponent<RoomTemplates>();
        mapTemplates = FindObj.instance.roomManager.GetComponent<MapTemplates>();
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
        Finish();
    }

    // 방의 업데이트가 끝나고 1초 후 방 생성 완료
    void Finish()
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
                SetMap();
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

    /*
    void Lock()
    {
        if(Lock)
        {
            for (int i = 0; i < room.Count; i++)
            {
                room[i].GetComponent<Room>().type = MapType.Default;
            }

            while (lockRoomCount < lockRoom)
            {
                GameObject lockRoomGameObject = room[Random.Range(2, room.Count - 1)];
                Room lockRoom = lockRoomGameObject.GetComponent<Room>();
                if (lockRoom.type == MapType.Lock)
                    continue;
                lockRoom.type = MapType.Lock;
                lockRoomCount++;
            }
            Lock = false;
        }
        
    }
    */

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

    void SetMap()
    {
        bool isBoss = false;
        bool isEvent = false;


        // 모든 방 생성을 완료하면 각 방들의 용도를 설정한다.
        for( int i = 1 ; i< rooms.Count ; i++)
        {
            Room room = rooms[i].GetComponent<Room>();
            
            if(room.roomType == RoomType.None)
            {
                room.SetMapManager(MapType.Treasure);
            }
            else if(room.roomType == RoomType.OneWay)
            {
                if(!isBoss)
                {
                    room.SetMapManager(MapType.Boss);
                    isBoss = true;
                    continue;
                }
                room.SetMapManager(MapType.Mission);
            }
            else if (room.roomType == RoomType.TwoWay)
            {
                
                int ran = Random.Range(0, 10);
                if(ran == 0)
                {
                    room.SetMapManager(MapType.Treasure);
                    continue;
                }
                room.SetMapManager(MapType.Default);
            }
            else if (room.roomType == RoomType.ThreeWay)
            {
                if(!isEvent)
                {
                    room.SetMapManager(MapType.Event);
                    isEvent = true;
                    continue;
                }
                room.SetMapManager(MapType.Shop);
            }
        }


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
