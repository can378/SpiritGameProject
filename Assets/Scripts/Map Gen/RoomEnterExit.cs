using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterExit : MonoBehaviour
{
    
    public GameObject playerPos;
    
    public Room room;
    GameObject enemyGroup;
    private GameObject minimapIcon;


    public SpriteRenderer forMiniMapSprite;
    //public GameObject forMap;
    public List<SpriteRenderer> aisleSprite;


    private string minimapIconString = "MinimapIcon";

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

        //최초방문시 miniMap에서 보이도록
        forMiniMapSprite.color = new Color(1, 1, 1, 1);
        forMiniMapSprite.sortingOrder = -1;
        foreach (SpriteRenderer spr in aisleSprite) 
        { spr.color = new Color(1, 1, 1, 1); }

        //show minimap icon
        if (room.minimapIcon != null)
        {
            room.minimapIcon.SetActive(true);
        }
        /*
        if (room.map!=null && HasComponent(room.map, "MinimapIcon")) 
        { room.map.GetComponent<MinimapIcon>().minimapIcon.SetActive(true); }
        */


        //map가리고 있는 검정색 제거
        //forMap.SetActive(false);

        //플레이어 위치 표시
        playerPos.SetActive(true);


        if (room.doorType == DoorType.Trap)
        {
            room.LockDoor();
        }
        
        if (room.map!=null) 
        {
            room.map.GetComponent<ObjectSpawn>().EnableEnemy();
        }

        if(room.mapType==MapType.Boss) 
        {
            enterBossRoom();
        }

        //플레이어가 현재 있는 맵 위치
        GameManager.instance.nowRoom = room.gameObject;
        GameManager.instance.nowRoomScript = room;
    }
    void exitRoom()
    {
        playerPos.SetActive(false);

        /*
        if (HasComponent(room.map.gameObject, minimapIconString))
        {
            GameObject miniIcon = room.map.gameObject.GetComponent<MinimapIcon>().minimapIcon;
            miniIcon.transform.parent = room.transform;
        }
        */
        if (room.map != null)
        {
            room.map.GetComponent<ObjectSpawn>().DisableEnemy();
        }

        //forMap.SetActive(true);

    }

    void enterBossRoom() 
    {
        GameObject boss = room.map.GetComponent<ObjectSpawn>().enemys[0];
        //start audio
        AudioManager.instance.Bgm_boss(DataManager.instance.userData.nowChapter);
        //camera moving
        CameraManager.instance.isCameraChasing = false;
        StartCoroutine(CameraManager.instance.BossRoomEnterEffect(boss,room.gameObject));
    }

    bool HasComponent(GameObject obj, string componentType)
    {
        // Component 타입을 가져옴
        var type = System.Type.GetType(componentType);

        if (type != null)
        {
            // GetComponent 메서드를 사용하여 Component가 있는지 확인
            var component = obj.GetComponent(type);
            return component != null;
        }
        else
        {
            Debug.LogError("Component type not found.");
            return false;
        }
    }

}
