using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType {Default, Shop, Treasure, Mission, MiniBoss, Boss}

public class Room : MonoBehaviour
{
    public bool lockTrigger;        //���� ��Ź�����
    public bool unLockTrigger;      //���� ����� �����Ѵ�.
    public MapType mapType;
    public DoorType doorType;
    public Door[] doors;

    // �� ���� ���� ������
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

    // �� ����
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

    // �� ����
    // None �ƹ��͵� ���� ���� ����
    // Key Ű�� �� �� �ִ� ��, ��� ���
    // Trap Ư�� ���ǿ� ���� ������ ����
    // Both Key�� Trap�� Ư���� ��� ����
    // Shabby ��ź���� �ı� ����
    // Wall ���� �μ��� ���� ��
    void SetDoor()
    {
        // ������ ���� �Ұ�
        if (doorType == DoorType.ShabbyWall || doorType == DoorType.Wall)
        {
            doorType = preDoorType;
            return;
        }

        // ����
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

    // key, Trap�̸� ���� �ᱼ �� �ִ�.
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

    // key, Trap�̸� ���� �� �� �ִ�.
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
