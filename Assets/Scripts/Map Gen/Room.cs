using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType {Default, Shop, Treasure, Mission, MiniBoss, Boss}

public class Room : MonoBehaviour
{
    public bool lockTrigger;        //맵을 잠궈버린다
    public bool unLockTrigger;      //맵의 잠금을 해제한다.
    public MapType mapType;
    public DoorType doorType;
    public Door[] doors;

    // 문 존재 여부 설정용
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    RoomManager roomManager;
    GameObject mapIcon;
    MapType preMapType;
    DoorType preDoorType;

    void Start() {
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        roomManager.room.Add(this.gameObject);

        preMapType = mapType;
        preDoorType = doorType;
    }

    void Update()
    {
        SetMap();
        SetDoor();
        LockTrigger();
        UnLockTrigger();
    }

    // 방 설정
    // shop
    // treasure
    // mission
    // miniBoss
    // boss
    void SetMap()
    {
        if(preMapType != mapType)
        {
            Destroy(mapIcon);
            if(mapType == MapType.Shop)
            {
                mapIcon = Instantiate(roomManager.templates.shopIcon, transform.position, transform.rotation);
                mapIcon.transform.localScale = new Vector3(roomManager.roomSize,roomManager.roomSize,0);
            }
            else if(mapType == MapType.Treasure)
            {
                mapIcon = Instantiate(roomManager.templates.treasureIcon, transform.position, transform.rotation);
                mapIcon.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 0);
            }
            else if (mapType == MapType.Mission)
            {
                mapIcon = Instantiate(roomManager.templates.missionIcon, transform.position, transform.rotation);
                mapIcon.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 0);
            }
            else if (mapType == MapType.MiniBoss)
            {
                mapIcon = Instantiate(roomManager.templates.miniBossIcon, transform.position, transform.rotation);
                mapIcon.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 0);
            }
            else if (mapType == MapType.Boss)
            {
                mapIcon = Instantiate(roomManager.templates.bossIcon, transform.position, transform.rotation);
                mapIcon.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 0);
            }
            else if(mapType == MapType.Default)
            {
                mapIcon = null;
            }
            preMapType = mapType;
        }
    }

    // 문 설정
    // None 아무것도 존재 하지 않음
    // Key 키로 열 수 있는 문, 즉시 잠김
    // Trap 특정 조건에 따라 닫히고 열림
    // Both Key와 Trap의 특성을 모두 가짐
    // Shabby 폭탄으로 파괴 가능
    // Wall 절대 부술수 없는 문
    void SetDoor()
    {
        // 벽으로 설정 불가
        if (doorType == DoorType.ShabbyWall || doorType == DoorType.Wall)
        {
            doorType = preDoorType;
            return;
        }

        // 변경
        if (preDoorType != doorType)
        {
            foreach (Door door in doors)
            {
                door.SetDoorType(doorType);
            }

            if(doorType == DoorType.Key)
                lockTrigger = true;

            preDoorType = doorType;
        }
    }

    // key, Trap이면 문을 잠굴 수 있다.
    public void LockTrigger()
    {
        if(lockTrigger)
        {
            foreach (Door door in doors)
            {
                door.LockDoor();
                lockTrigger = false;
            }
        }
    }

    // key, Trap이면 문을 열 수 있다.
    public void UnLockTrigger()
    {
        if(unLockTrigger)
        {
            foreach (Door door in doors)
            {
                door.UnLockDoor();
                unLockTrigger = false;
            }
        }
    }

}
