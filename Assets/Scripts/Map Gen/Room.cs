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
    public bool lockTrigger;        //���� ��Ź�����
    public bool unLockTrigger;      //���� ����� �����Ѵ�.
    public bool setRoom;            //���� ������ �� �ٲپ��ٸ� ������.

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

    // �� ����
    // None �ƹ��͵� ���� ���� ����
    // Key Ű�� �� �� �ִ� ��
    // Trap ������ �޼��ؾ߸� �� �� �ִ� ��
    // Shabby ��ź���� �ı� ����
    // Wall ���� �μ��� ���� ��
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

    // key, Trap�̸� ���� �ᱼ �� �ִ�.
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

    // key, Trap�̸� ���� ���� �ִ�.
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
