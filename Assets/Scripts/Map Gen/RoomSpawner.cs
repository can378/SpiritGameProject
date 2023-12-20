using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door


    private RoomTemplates templates;
    private RoomManager roomManager;
    private int rand;
    public bool spawned = false;

    void Start()
    {
        Destroy(gameObject, 10f);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        Invoke("Spawn",0.1f);
    }


    void Spawn()
    {
        if (spawned == false)
        {
            // ���� ���¿� ���� �� ����

            if (roomManager.maxRoom - 1 <= roomManager.room.Count)
            {
                rand = 0;
            }
            else
            {
                // 0�� turning
                // Ȯ���� ���� �ʿ�
                int turning = Random.Range(0, roomManager.maxRoom / roomManager.turning);

                // ���� ��迡 ������ ȸ��
                if ((openingDirection == 1 || openingDirection == 2) &&
                    (transform.position.y == -(50 * roomManager.roomSize) || transform.position.y == (50 * roomManager.roomSize)))
                {
                    Debug.Log("trun");
                    turning = 0;
                }
                else if((openingDirection == 3 || openingDirection == 4) &&
                    (transform.position.x == -(50 * roomManager.roomSize) || transform.position.x == (50 * roomManager.roomSize)))
                {
                    Debug.Log("trun");
                    turning = 0;
                }


                //����
                if (turning != 0)
                {
                    rand = 3;
                }
                else
                {
                    // turning
                    // ���� �����ϰ� ��� ���� Ȯ���ϴ� ��
                    // �� ���� ����ִ��� Ȯ��

                    // ���� �ȿ��� ȸ�� �� ��
                    if (openingDirection == 1 || openingDirection == 2)
                    {
                        // y���� x�� ȸ��
                        // + �Ǵ� - �������� �� �� �ش� ���⿡ ���� ���� �� ����
                        bool isY = false;
                        for (int i = 0; i < roomManager.room.Count; i++)
                        {
                            if (transform.position.y == roomManager.room[i].transform.position.y)
                            {
                                isY = true;
                                if (transform.position.x < roomManager.room[i].transform.position.x)
                                {
                                    rand = 1;
                                }
                                else if (transform.position.x > roomManager.room[i].transform.position.x)
                                {
                                    rand = 2;
                                }
                                break;
                            }
                        }
                        if(isY == false)
                        {
                            rand = Random.Range(1, 3);
                        }
                    }
                    else if (openingDirection == 3 || openingDirection == 4)
                    {
                        // x���� y�� ȸ��
                        // + �Ǵ� - �������� �� �� �ش� ���⿡ ���� ���� �� ����
                        bool isX = false;
                        for (int i = 0; i < roomManager.room.Count; i++)
                        {
                            if (transform.position.x == roomManager.room[i].transform.position.x)
                            {
                                isX = true;
                                if (transform.position.y < roomManager.room[i].transform.position.y)
                                {
                                    rand = 2;
                                }
                                else if (transform.position.y > roomManager.room[i].transform.position.y)
                                {
                                    rand = 1;
                                }
                                break;
                            }
                        }
                        if (isX == false)
                        {
                            rand = Random.Range(1, 3);
                        }
                    }

                    // ���� ��迡�� ȸ�� �� ��
                    // ���� �������� ȸ��
                    if ((openingDirection == 1 || (openingDirection == 2)) &&
                    (transform.position.x == (50 * roomManager.roomSize)))
                    {
                        rand = 1;
                    }
                    else if ((openingDirection == 1 || (openingDirection == 2)) &&
                    (transform.position.x == -(50 * roomManager.roomSize)))
                    {
                        rand = 2;
                    }
                    if ((openingDirection == 3 || (openingDirection == 4)) &&
                    (transform.position.y == (50 * roomManager.roomSize)))
                    {
                        rand = 2;
                    }
                    else if ((openingDirection == 3 || (openingDirection == 4)) &&
                    (transform.position.y == -(50 * roomManager.roomSize)))
                    {
                        rand = 1;
                    }

                }
            

            
            }

            // �ش� ������ �ݴ�� ��ΰ� �ִ� ��
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a RIGHT door.
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a LEFT door.
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}