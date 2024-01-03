using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    
    public bool spawn;
    public int addedRoom;                   // 각 경로당 추가 생성할 방
    public int turning;                     // 꺾을 횟수
    public int crossedRoom;                 // 갈림길 방의 수
    public int defaultMaxRoom;              // 기본 최대방의 수
    public int maxRoom;                     // 최대방의 수 [기본 상태 0]
    public int roomSize;                    // 방 크기 배율 기본 : 3
    public int area;                        // 방 상화 좌우 영역 기본 : 5
    public RoomTemplates templates;
    public List<GameObject> room;
    public bool finish;                     // 맵 생성 완료 보기용

    int preCount = 0;
    float waitTime = 0;
    int crossedRoomCount = 0;
    

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void Update()
    {
        Spawn();
        Finish();
    }

    // 방의 업데이트가 끝나고 1초 후 방 생성 완료
    void Finish()
    {
        if(preCount != room.Count)
        {
            finish = false;
            waitTime = 0;
            preCount = room.Count;
            return;
        }

        if(waitTime >=1)
        {
            if(room.Count < maxRoom )
            {
                spawn = true;
                return;
            }
            finish = true;
        }
        else 
        {
            waitTime += Time.deltaTime;
        }
    }

    // 초기 위치 설정
    void Spawn()
    {
        if (spawn)
        {
            crossedRoomCount = 0;
            maxRoom = defaultMaxRoom;
            crossedRoomCount = 0;

            for (int i = 0; i < room.Count; i++)
            {
                Destroy(room[i]);
            }
            room.Clear();
            int dir = Random.Range(1, 5);
            Vector2 startPoisition = new Vector2(Random.Range(-2, 3) * 10 * roomSize, Random.Range(-2, 3) * 10 * roomSize);

            if (dir == 1)
            {
                Instantiate(templates.bottomRooms[0], startPoisition, Quaternion.identity).transform.localScale = 
                new Vector3(roomSize,roomSize,1);
            }
            else if (dir == 2)
            {
                Instantiate(templates.topRooms[0], startPoisition, Quaternion.identity).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (dir == 3)
            {
                Instantiate(templates.rightRooms[0], startPoisition, Quaternion.identity).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (dir == 4)
            {
                Instantiate(templates.leftRooms[0], startPoisition, Quaternion.identity).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
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

            int index = Random.Range(1, room.Count - 1);
            GameObject crossedRoomGameObject = room[index];
            Room crossedRoomGameObjectRoom = crossedRoomGameObject.GetComponent<Room>();

            crossedRoomGameObject.SetActive(false);

            if (crossedRoomGameObjectRoom.top && crossedRoomGameObjectRoom.bottom)
            {
                Instantiate(templates.verticalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (crossedRoomGameObjectRoom.left && crossedRoomGameObjectRoom.right)
            {
                Instantiate(templates.horizontalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else
            {
                crossedRoomGameObject.SetActive(true);
                continue;
            }

            crossedRoomCount++;
            Destroy(crossedRoomGameObject);
            room.RemoveAt(index);
        }
    }

    // 상점 설정
    void Shop()
    {
        
    }

    // 보물 설정
    void Treasure(){

    }
}
