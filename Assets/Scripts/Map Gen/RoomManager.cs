using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool spawn;
    public int addedRoom;                   // �� ��δ� �߰� ������ ��
    public int turning;                     // ���� Ƚ��
    public int crossedRoom;                 // ������ ���� ��
    public int defaultMaxRoom;              // �⺻ �ִ���� ��
    public int maxRoom;                     // �ִ���� �� [�⺻ ���� 0]
    public int roomSize;                    // �� ũ�� ���� �⺻ : 3
    public int area;                        // �� ��ȭ �¿� ���� �⺻ : 5

    public List<GameObject> room;

    private bool cross;
    private int crossedRoomCount = 0;

    private RoomTemplates templates;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (spawn)
        {
            cross = false;

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
            cross = true;
            Invoke("CrossedRoom",2f);
        }
        
    }

    /*
    void LockRoom()
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

    void CrossedRoom()
    {
        if(cross)
        {
            maxRoom = defaultMaxRoom + (crossedRoom * (addedRoom-1));
            for(int i = 0 ; i < defaultMaxRoom ; i++ )
            {
                
                int index = Random.Range(1, room.Count - 1);
                GameObject crossedRoomGameObject = room[index];
                Room crossedRoomGameObjectRoom = crossedRoomGameObject.GetComponent<Room>();
                
                crossedRoomGameObject.SetActive(false);

                if (crossedRoomGameObjectRoom.top && crossedRoomGameObjectRoom.bottom)
                {
                    Instantiate(templates.verticalCrossedRooms[Random.Range(0,2)], crossedRoomGameObject.transform.position,crossedRoomGameObject.transform.rotation).transform.localScale = 
                    new Vector3(roomSize,roomSize,1);
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

                if(crossedRoomCount >= crossedRoom)
                    break;
            }
        }
    }
}
