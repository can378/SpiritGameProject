using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType {Default, Shop, Treasure, Mission, MiniBoss, Boss}

public class Room : MonoBehaviour
{
    RoomManager roomManager;
    public MapType mapType;
    public DoorType doorType;

    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    public Door[] doors;
    public bool lockTrigger;        //맵을 잠궈버린다
    public bool unLockTrigger;      //맵의 잠금을 해제한다.
    public bool setRoom;            //맵의 설정을 다 바꾸었다면 누른다.

    void Start() {
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        roomManager.room.Add(this.gameObject);
    }

    void Update()
    {
        SetDoor();
        LockTrigger();
        UnLockTrigger();
    }

    // 문 설정
    // None 아무것도 존재 하지 않음
    // Key 키로 열 수 있는 문
    // Trap 조건을 달성해야만 열 수 있는 문
    // Shabby 폭탄으로 파괴 가능
    // Wall 절대 부술수 없는 문
    void SetDoor()
    {
        if(setRoom)
        {
            foreach(Door door in doors)
            {
                door.SetDoorType(doorType);
                setRoom = false;
            }
        }
    }

    // key, Trap이면 문을 잠굴 수 있다.
    void LockTrigger()
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

    // key, Trap이면 문을 열수 있다.
    void UnLockTrigger()
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
