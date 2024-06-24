using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { None, Default, Shop, Treasure, Event, Mission, Boss }
public enum RoomType { None, OneWay, TwoWay, ThreeWay, FourWay } // ���� ����

public class Room : MonoBehaviour
{

    [SerializeField] bool lockTrigger;        //���� ��Ź�����
    [SerializeField] bool unLockTrigger;      //���� ����� �����Ѵ�.
    [field: SerializeField] public MapType mapType { get; private set; }          //�� Ÿ���� �ٲٸ� ���� ���� �����
    [field: SerializeField] public DoorType doorType { get; private set; }                         //���߿� ����
    [field: SerializeField] public RoomType roomType { get; private set; }
    [field: SerializeField] public Door door { get; private set; }
    [field: SerializeField] public GameObject map { get; private set; }

    // �� ���� ���� ������
    [field: SerializeField] public bool top {get; private set;}
    [field: SerializeField] public bool bottom { get; private set; }
    [field: SerializeField] public bool left { get; private set; }
    [field: SerializeField] public bool right { get; private set; }

    public GameObject floorArea;

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
                doorType = DoorType.Trap;
                ran = Random.Range(0, mapTemplates.missionMap.Length);
                map = Instantiate(mapTemplates.missionMap[ran], transform.position, transform.rotation);
                
                map.GetComponent<Mission>().roomScript = this;
            }
            else if (mapType == MapType.Boss)
            {
                doorType = DoorType.Key;
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
        // ����
        if (preDoorType != doorType)
        {
            door.SetDoorType(doorType);

            if (doorType == DoorType.Key)
                lockTrigger = true;

            preDoorType = doorType;
        }
    }

    public void LockDoor()
    {
        lockTrigger = true;
    }

    public void UnLockDoor()
    {
        unLockTrigger = true;
    }

    // key, Trap�̸� ���� �ᱼ �� �ִ�.
    void LockTrigger()
    {
        if(lockTrigger)
        {
            door.LockDoor();
            lockTrigger = false;
        }
    }

    // key, Trap�̸� ���� �� �� �ִ�.
    void UnLockTrigger()
    {
        if(unLockTrigger)
        {
            door.UnLockDoor();
            unLockTrigger = false;
        }
    }

    // �Ŵ�����
    public void SetMapManager(MapType mapType)
    {
        this.mapType = mapType;
    }

}
