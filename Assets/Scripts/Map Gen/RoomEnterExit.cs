using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterExit : MonoBehaviour
{
    
    public GameObject playerPos;
    
    public Room room;
    GameObject enemyGroup;


    public SpriteRenderer forMiniMapSprite;
    public GameObject forMap;


    void Start()
    {

    }

    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //print("player enter");
            enterRoom();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //print("player exit");
            exitRoom();
        }
    }

    void enterRoom()
    {

        //���ʹ湮�� miniMap���� ������ �ִ��� ����
        forMiniMapSprite.color = new Color(1, 1, 1, 1);
        forMiniMapSprite.sortingOrder = -1;

        forMap.SetActive(false);

        //�÷��̾� ��ġ ǥ��
        playerPos.SetActive(true);



        //enemy�� ���� ����
        if (room.mapType==MapType.Boss || room.mapType == MapType.Default) 
        { 

            //enemyGroup=room.thisRoom.transform.GetComponent<ObjectSpawn>().enemyGroup; 

            enemyGroup=room.map.transform.Find("Enemy").GetComponent<ObjectSpawn>().enemyGroup; 

            enemyGroup.SetActive(true);
        }
        

    }
    void exitRoom()
    {
        playerPos.SetActive(false);
        //enemy ���� ����
        if (room.mapType == MapType.Boss || room.mapType == MapType.Default)
        {

            //enemyGroup = room.thisRoom.transform.GetComponent<ObjectSpawn>().enemyGroup;

            enemyGroup = room.map.transform.Find("Enemy").GetComponent<ObjectSpawn>().enemyGroup;

            enemyGroup.SetActive(false);
        }
        
        forMap.SetActive(true);

    }
    */
}
