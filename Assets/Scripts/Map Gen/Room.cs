using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType { None, Default, Shop, Treasure, Event, Mission, Boss }
public enum RoomType { None, OneWay, TwoWay, ThreeWay, FourWay } // 문의 개수

public class Room : MonoBehaviour
{

    [SerializeField] bool lockTrigger;        //맵을 잠궈버린다
    [SerializeField] bool unLockTrigger;      //맵의 잠금을 해제한다.
    [field: SerializeField] public MapType mapType { get; private set; }          //맵 타입을 바꾸면 현재 방이 변경됨
    [field: SerializeField] public DoorType doorType { get; private set; }                         //나중에 설정
    [field: SerializeField] public RoomType roomType { get; private set; }
    [field: SerializeField] public Door door { get; private set; }
    [field: SerializeField] public GameObject map { get; private set; }

    // 문 존재 여부 설정용
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

    // 문 설정
    // None 아무것도 존재 하지 않음
    // Key 키로 열 수 있는 문, 즉시 잠김
    // Trap 특정 조건에 따라 닫히고 열림
    // Both Key와 Trap의 특성을 모두 가짐
    // Shabby 폭탄으로 파괴 가능
    // Wall 절대 부술수 없는 문
    void SetDoor()
    {
        // 변경
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

    // key, Trap이면 문을 잠굴 수 있다.
    void LockTrigger()
    {
        if(lockTrigger)
        {
            door.LockDoor();
            lockTrigger = false;
        }
    }

    // key, Trap이면 문을 열 수 있다.
    void UnLockTrigger()
    {
        if(unLockTrigger)
        {
            door.UnLockDoor();
            unLockTrigger = false;
        }
    }

    // 매니저용
    public void SetMapManager(MapType mapType)
    {
        this.mapType = mapType;
    }

}
