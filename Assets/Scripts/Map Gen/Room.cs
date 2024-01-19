using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { None, Default, Shop, Treasure, Event, Mission, MiniBoss, Boss }
public enum RoomType { None, OneWay, TwoWay, ThreeWay, FourWay } // ���� ����

public class Room : MonoBehaviour
{
    public bool lockTrigger;        //���� ��Ź�����
    public bool unLockTrigger;      //���� ����� �����Ѵ�.
    public MapType mapType;         //�� Ÿ���� �ٲٸ� ���� ���� �����
    public DoorType doorType;
    public RoomType roomType;
    public Door[] doors;

    // �� ���� ���� ������
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    RoomManager roomManager;
    MapType preMapType;
    DoorType preDoorType;
    public GameObject thisRoom;

    void Start() {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        roomManager.rooms.Add(this.gameObject);

        //this.transform.SetParent(roomManager.transform);

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
            int ran;
            Destroy(thisRoom);
            if(mapType == MapType.Shop)
            {
                ran = Random.Range(0, roomManager.roomTemplates.shopMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.shopMap[ran], transform.position, transform.rotation);
                
            }
            else if(mapType == MapType.Treasure)
            {
                ran = Random.Range(0, roomManager.roomTemplates.treasureMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.treasureMap[ran], transform.position, transform.rotation);
            }
            else if(mapType == MapType.Event)
            {
                ran = Random.Range(0, roomManager.roomTemplates.eventMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.eventMap[ran], transform.position, transform.rotation);
            }
            else if (mapType == MapType.Mission)
            {
                ran = Random.Range(0, roomManager.roomTemplates.missionMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.missionMap[ran], transform.position, transform.rotation);
            }
            else if (mapType == MapType.MiniBoss)
            {
                ran = Random.Range(0, roomManager.roomTemplates.miniBossMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.miniBossMap[ran], transform.position, transform.rotation);
            }
            else if (mapType == MapType.Boss)
            {
                ran = Random.Range(0, roomManager.roomTemplates.bossMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.bossMap[ran], transform.position, transform.rotation);
            }
            else if(mapType == MapType.Default)
            {
                ran = Random.Range(0, roomManager.roomTemplates.defaultMap.Length);
                thisRoom = Instantiate(roomManager.roomTemplates.defaultMap[ran],transform.position,transform.rotation);
            }
            thisRoom.GetComponent<ObjectSpawn>().SpawnEnemy(mapType);
            thisRoom.transform.SetParent(this.transform);
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
