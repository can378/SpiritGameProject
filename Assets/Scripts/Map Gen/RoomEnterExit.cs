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
    public List<SpriteRenderer> aisleSprite;


    void Start()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterRoom();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            exitRoom();
        }
    }

    void enterRoom()
    {

        //최초방문시 miniMap에서 가리고 있던것 없앰
        forMiniMapSprite.color = new Color(1, 1, 1, 1);
        forMiniMapSprite.sortingOrder = -1;
        foreach (SpriteRenderer spr in aisleSprite) 
        { spr.color = new Color(1, 1, 1, 1); }

        forMap.SetActive(false);

        //플레이어 위치 표시
        playerPos.SetActive(true);


        if (room.doorType == DoorType.Trap)
        {
            room.LockDoor();
        }

        if (room.map!=null) 
        { 
            room.map.SetActive(true);
        }

        //플레이어가 현재 있는 맵 위치
        GameManager.instance.nowRoom = room.floorArea;

    }
    void exitRoom()
    {
        playerPos.SetActive(false);


        if (room.map != null)
        {
            room.map.SetActive(false);
        }
        
        forMap.SetActive(true);

    }

}
