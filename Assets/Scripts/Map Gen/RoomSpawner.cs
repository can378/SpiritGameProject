using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // ���� ����
    // �� �ڸ��� ���ÿ� 3���� ���� �����Ƿ��� �ϸ� ���ڸ��� �������� ���� ������
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
        // ������ ������ ���� ������
        // �Ϲ������� �Ѱ��� ��η� ������
        // ���� ������ �ִ�� ���� �Ͽ��ų� ���� ���� ���� �����ϸ� ���θ��� ���� ����
        limitArea = roomManager.area * roomManager.roomSize * 10;
        roomSize = new Vector3(roomManager.roomSize, roomManager.roomSize, 1);

        // ���� ���� ����ϰų�
        // ������ �Ѿ�� �Ǹ� �ݴ´�.
        if ((roomManager.maxRoom - 1 <= roomManager.rooms.Count) ||
        (transform.position.y < -limitArea || transform.position.y > limitArea || transform.position.x < -limitArea || transform.position.x > limitArea))
        {
            index = 0;
        }
        // ȸ��
        // Ȯ�������� ȸ���ϰų�
        // ������ ����� ������ ȸ��
        else
        {
            // 0�� turning
            // Ȯ���� ���� �ʿ�
            int turning = roomManager.turning == 0 ? 1 : Random.Range(0, roomManager.maxRoom / roomManager.turning);

            // ���� ��迡 ������ ȸ��
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


            //����
            if (turning != 0)
            {
                index = 3;
            }
            else
            {
                // ���� �ȿ��� ȸ�� �� ��
                index = Random.Range(1, 3);
                if (openingDirection == 1 || openingDirection == 2)
                {
                    // y���� x�� ȸ��
                    // + �Ǵ� - �������� �� �� �ش� ���⿡ ���� ���� �� ����
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
                    // x���� y�� ȸ��
                    // + �Ǵ� - �������� �� �� �ش� ���⿡ ���� ���� �� ����
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

                // ���� ��迡�� ȸ�� �� ��
                // ���� �������� ȸ��
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