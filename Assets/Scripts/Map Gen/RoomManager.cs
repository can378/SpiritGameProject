using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomTemplates roomTemplates;
    public bool spawn;
    public int addedRoom;                   // 각 경로당 추가 생성할 방
    public int turning;                     // 꺾을 횟수
    public int crossedRoom;                 // 갈림길 방의 수
    public int defaultMaxRoom;              // 기본 최대방의 수
    public int maxRoom;                     // 최대방의 수 [기본 상태 0]
    public int roomSize;                    // 방 크기 배율 기본 : 3
    public int area;                        // 방 상화 좌우 영역 기본 : 5
    public List<GameObject> rooms;
    public bool finish;                     // 맵 생성 완료 보기용

    public GameObject miniMapCamera;
    public GameObject hideMap;
    Transform roomParent;
    

    bool spawning;
    int preCount = 0;
    float waitTime = 0;
    int crossedRoomCount = 0;

    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomTemplates>();
        roomParent = GameObject.FindWithTag("roomParent").transform;
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
                HidingMap();
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
            if(spawning)
            {
                spawn = false;
                return;
            }

            finish = false;
            spawning = true;
            waitTime = 0;
            crossedRoomCount = 0;
            maxRoom = defaultMaxRoom;
            crossedRoomCount = 0;

            for (int i = 0; i < rooms.Count; i++)
            {
                Destroy(rooms[i]);
            }
            rooms.Clear();
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
        bool isMiniBoss = false;
        bool isEvent = false;


        // 모든 방 생성을 완료하면 각 방들의 용도를 설정한다.
        for( int i = 1 ; i< rooms.Count ; i++)
        {
            Room room = rooms[i].GetComponent<Room>();
            if(room.roomType == RoomType.None)
            {
                room.mapType = MapType.Treasure;
            }
            else if(room.roomType == RoomType.OneWay)
            {
                if(!isBoss)
                {
                    room.mapType = MapType.Boss;
                    isBoss = true;
                    continue;
                }
                else if (!isMiniBoss)
                {
                    room.mapType = MapType.MiniBoss;
                    isMiniBoss = true;
                    continue;
                }
                room.mapType = MapType.Mission;
            }
            else if (room.roomType == RoomType.TwoWay)
            {
                
                int ran = Random.Range(0, 10);
                if(ran == 0)
                {
                    room.mapType = MapType.Treasure;
                    continue;
                }
                room.mapType = MapType.Default;
            }
            else if (room.roomType == RoomType.ThreeWay)
            {
                if(!isEvent)
                {
                    room.mapType = MapType.Event;
                    isEvent = true;
                    continue;
                }
                room.mapType = MapType.Shop;
            }
        }


    }


    //맵의 중앙을 찾아서 미니맵 카메라 이동
    void setMinimapCamera()
    {

        Vector2 middlePos=Vector2.zero;

        for(int i=0;i<roomParent.childCount;i++) 
        {
            middlePos += (Vector2)roomParent.GetChild(i).transform.position;
        }


        if (roomParent.childCount > 0)
        {
            middlePos /= roomParent.childCount;
        }

        miniMapCamera.transform.position = middlePos;
        miniMapCamera.transform.position += new Vector3(0,0,-5);
        
    }

    void HidingMap()
    {
        GameObject hide;
        for (int i = 0; i < roomParent.childCount; i++)
        {
            hide=Instantiate(hideMap);
            hide.transform.parent=roomParent.GetChild(i).transform;
            hide.transform.localScale = new Vector3(roomSize*roomSize, roomSize * roomSize, 1);
            hide.transform.position = roomParent.GetChild(i).position;
        }

    }
}
