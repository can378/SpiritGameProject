using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // 버그 있음
    // 한 자리에 동시에 3개의 방이 생성되려고 하면 한자리에 여러개의 방이 생성됨
    public bool spawned = false;
    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door


    RoomTemplates templates;
    RoomManager roomManager;
    int index = 0;
    int limitArea = 0;
    Vector3 roomSize;

    void Start()
    {
        Destroy(gameObject,5f);
        templates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomTemplates>();
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        Invoke("Spawn",0.1f);
       
    }

    void SetRoom()
    {
        // 적절한 형태의 방을 설정함
        // 일반적으로 한개의 통로로 생성됨
        // 맵의 개수가 최대로 도달 하였거나 맵의 영역 끝에 도달하면 가로막힌 방이 생성
        limitArea = roomManager.area * roomManager.roomSize * 10;
        roomSize = new Vector3(roomManager.roomSize, roomManager.roomSize, 1);

        // 방의 수가 충분하거나
        // 영역을 넘어가게 되면 닫는다.
        if ((roomManager.maxRoom - 1 <= roomManager.rooms.Count) ||
        (transform.position.y < -limitArea || transform.position.y > limitArea || transform.position.x < -limitArea || transform.position.x > limitArea))
        {
            index = 0;
        }
        // 회전
        // 확률적으로 회전하거나
        // 영역의 경계의 닿을시 회전
        else
        {
            // 0이 turning
            // 확률임 수정 필요
            int turning = roomManager.turning == 0 ? 1 : Random.Range(0, roomManager.maxRoom / roomManager.turning);

            // 범위 경계에 닿을시 회전
            if ((openingDirection == 1 || openingDirection == 2) &&
                (transform.position.y == -limitArea || transform.position.y == limitArea))
            {
                turning = 0;
            }
            else if ((openingDirection == 3 || openingDirection == 4) &&
                (transform.position.x == -limitArea || transform.position.x == limitArea))
            {
                turning = 0;
            }


            //직진
            if (turning != 0)
            {
                index = 3;
            }
            else
            {
                // 범위 안에서 회전 할 시
                index = Random.Range(1, 3);
                if (openingDirection == 1 || openingDirection == 2)
                {
                    // y에서 x로 회전
                    // + 또는 - 방향으로 갈 때 해당 방향에 방이 없는 방 선택
                    for (int i = 0; i < roomManager.rooms.Count; i++)
                    {
                        if (transform.position.y == roomManager.rooms[i].transform.position.y)
                        {
                            if (transform.position.x < roomManager.rooms[i].transform.position.x)
                            {
                                index = 1;
                            }
                            else if (transform.position.x > roomManager.rooms[i].transform.position.x)
                            {
                                index = 2;
                            }
                            break;
                        }
                    }
                }
                else if (openingDirection == 3 || openingDirection == 4)
                {
                    // x에서 y로 회전
                    // + 또는 - 방향으로 갈 때 해당 방향에 방이 없는 방 선택
                    for (int i = 0; i < roomManager.rooms.Count; i++)
                    {
                        if (transform.position.x == roomManager.rooms[i].transform.position.x)
                        {
                            if (transform.position.y < roomManager.rooms[i].transform.position.y)
                            {
                                index = 2;
                            }
                            else if (transform.position.y > roomManager.rooms[i].transform.position.y)
                            {
                                index = 1;
                            }
                            break;
                        }
                    }
                }

                // 범위 경계에서 회전 할 시
                // 범위 안쪽으로 회전
                if ((openingDirection == 1 || (openingDirection == 2)) &&
                (transform.position.x == limitArea))
                {
                    index = 1;
                }
                else if ((openingDirection == 1 || (openingDirection == 2)) &&
                (transform.position.x == -limitArea))
                {
                    index = 2;
                }
                if ((openingDirection == 3 || (openingDirection == 4)) &&
                (transform.position.y == limitArea))
                {
                    index = 2;
                }
                else if ((openingDirection == 3 || (openingDirection == 4)) &&
                (transform.position.y == -limitArea))
                {
                    index = 1;
                }

            }
        }
    }

    void Spawn()
    {
        if (spawned == false)
        {
            SetRoom();

            GameObject instObj = null;
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                instObj = Instantiate(templates.bottomRooms[index], transform.position, templates.bottomRooms[index].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                instObj = Instantiate(templates.topRooms[index], transform.position, templates.topRooms[index].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a RIGHT door.
                instObj = Instantiate(templates.rightRooms[index], transform.position, templates.rightRooms[index].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a LEFT door.
                instObj = Instantiate(templates.leftRooms[index], transform.position, templates.leftRooms[index].transform.rotation);
                
            }

            instObj.transform.localScale = roomSize;
            instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;
            
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("SpawnPoint"))
        {
            GameObject instObj = null;
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                limitArea = roomManager.area * roomManager.roomSize * 10;
                roomSize = new Vector3(roomManager.roomSize, roomManager.roomSize, 1);
                instObj = Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                instObj.transform.localScale = roomSize;
                instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}