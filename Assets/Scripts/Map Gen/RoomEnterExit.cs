using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterExit : MonoBehaviour
{
    SpriteRenderer spr;
    public GameObject playerPos;
    public Room room;
    GameObject enemyGroup;
    public bool isEnemyHere=false;

    void Start()
    {
        spr= GetComponent<SpriteRenderer>();
        switch (room.mapType)
        {
            case MapType.Boss:isEnemyHere = true; break;
            case MapType.MiniBoss: isEnemyHere = true; break;
            case MapType.Default: isEnemyHere = true; break;
            default:break;
        }

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("player enter");
            enterRoom();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("player exit");
            exitRoom();
        }
    }

    void enterRoom()
    {

        //���ʹ湮�� room ������ �ִ��� ����
        spr.color = new Color(1, 1, 1, 1);
        spr.sortingOrder = -1;

        //�÷��̾� ��ġ ǥ��
        playerPos.SetActive(true);


        //enemy�� ���� ����
        if(isEnemyHere) 
        { 
            enemyGroup=room.thisRoom.transform.Find("Enemy").GetComponent<ObjectSpawn>().enemyGroup; 
            enemyGroup.SetActive(true);
        }
        //setActive(true);

    }
    void exitRoom()
    {
        playerPos.SetActive(false);
        //enemy ���� ����
        if (isEnemyHere) 
        {
            enemyGroup = room.thisRoom.transform.Find("Enemy").GetComponent<ObjectSpawn>().enemyGroup;
            enemyGroup.SetActive(false);

        }
        //SetActive(false);

    }
}
