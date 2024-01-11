using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomTemplates roomTemplates;
    public bool spawn;
    public int addedRoom;                   // �� ��δ� �߰� ������ ��
    public int turning;                     // ���� Ƚ��
    public int crossedRoom;                 // ������ ���� ��
    public int defaultMaxRoom;              // �⺻ �ִ���� ��
    public int maxRoom;                     // �ִ���� �� [�⺻ ���� 0]
    public int roomSize;                    // �� ũ�� ���� �⺻ : 3
    public int area;                        // �� ��ȭ �¿� ���� �⺻ : 5
    public List<GameObject> rooms;
    public bool finish;                     // �� ���� �Ϸ� �����
    
    bool spawning;
    int preCount = 0;
    float waitTime = 0;
    int crossedRoomCount = 0;

    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomTemplates>();
        maxRoom = defaultMaxRoom;
        finish = true;
        spawning = false;
    }

    void Update()
    {
        Spawn();
        Finish();
    }

    // ���� ������Ʈ�� ������ 1�� �� �� ���� �Ϸ�
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
            }
            else
            {
                waitTime += Time.deltaTime;
            }
        }
    }

    // �ʱ� ��ġ ����
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

            if (dir == 1)
            {
                Instantiate(roomTemplates.bottomRooms[0], startPoisition, Quaternion.identity).transform.localScale = 
                new Vector3(roomSize,roomSize,1);
            }
            else if (dir == 2)
            {
                Instantiate(roomTemplates.topRooms[0], startPoisition, Quaternion.identity).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (dir == 3)
            {
                Instantiate(roomTemplates.rightRooms[0], startPoisition, Quaternion.identity).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (dir == 4)
            {
                Instantiate(roomTemplates.leftRooms[0], startPoisition, Quaternion.identity).transform.localScale =
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

    // ������ ����
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

            if (crossedRoomGameObjectRoom.top && crossedRoomGameObjectRoom.bottom)
            {
                Instantiate(roomTemplates.verticalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
            }
            else if (crossedRoomGameObjectRoom.left && crossedRoomGameObjectRoom.right)
            {
                Instantiate(roomTemplates.horizontalCrossedRooms[Random.Range(0, 2)], crossedRoomGameObject.transform.position, crossedRoomGameObject.transform.rotation).transform.localScale =
                new Vector3(roomSize, roomSize, 1);
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


        // ��� �� ������ �Ϸ��ϸ� �� ����� �뵵�� �����Ѵ�.
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

                int ran = Random.Range(0,2);
                if(ran == 0)    // �̼�
                    room.mapType = MapType.Mission;
                else if(ran == 1)   //���� ����
                    room.mapType = MapType.Treasure;
            }
            else if (room.roomType == RoomType.TwoWay)
            {
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
}
