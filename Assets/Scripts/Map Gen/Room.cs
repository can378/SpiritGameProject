using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { None, Default, Shop, Treasure, Event, Mission, Boss }
public enum RoomType { None, OneWay, TwoWay, ThreeWay, FourWay } // ���� ����

public class Room : MonoBehaviour
{
    public bool lockTrigger;        //���� ��Ź�����
    public bool unLockTrigger;      //���� ����� �����Ѵ�.
    public MapType mapType;          //�� Ÿ���� �ٲٸ� ���� ���� �����
    public DoorType doorType;
    public RoomType roomType;
    public Door[] doors;
    public GameObject map;

    // �� ���� ���� ������
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;

    RoomManager roomManager;
    MapTemplates mapTemplates;
    MapType preMapType;
    DoorType preDoorType;
    

    void Start() {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        mapTemplates = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<MapTemplates>();

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
            Destroy(map);
            if(mapType == MapType.Shop)
            {
                ran = Random.Range(0, mapTemplates.shopMap.Length);
                map = Instantiate(mapTemplates.shopMap[ran], transform.position, transform.rotation);
                
            }
            else if(mapType == MapType.Treasure)
            {
                ran = Random.Range(0, mapTemplates.treasureMap.Length);
                map = Instantiate(mapTemplates.treasureMap[ran], transform.position, transform.rotation);
            }
            else if(mapType == MapType.Event)
            {
                ran = Random.Range(0, mapTemplates.eventMap.Length);
                map = Instantiate(mapTemplates.eventMap[ran], transform.position, transform.rotation);
            }
            else if (mapType == MapType.Mission)
            {
                ran = Random.Range(0, mapTemplates.missionMap.Length);
                map = Instantiate(mapTemplates.missionMap[ran], transform.position, transform.rotation);
            }
            else if (mapType == MapType.Boss)
            {
                ran = Random.Range(0, mapTemplates.bossMap.Length);
                map = Instantiate(mapTemplates.bossMap[ran], transform.position, transform.rotation);
            }
            else if(mapType == MapType.Default)
            {
                ran = Random.Range(0, mapTemplates.defaultMap.Length);
                map = Instantiate(mapTemplates.defaultMap[ran],transform.position,transform.rotation);
            }
            map.GetComponent<ObjectSpawn>().SpawnEnemy(mapType);
            map.transform.SetParent(this.transform);
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
