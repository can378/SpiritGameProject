using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public enum MapType { Default, Reward, Mission, Boss, None }
public enum RoomType { OneWay, TwoWay, ThreeWay, FourWay, None } 

public class Room : MonoBehaviour
{

    [SerializeField] bool lockTrigger;
    [SerializeField] bool unLockTrigger;

    [field: SerializeField] public MapType mapType { get; private set; }
    MapType preMapType;
    [field: SerializeField] public DoorType doorType { get; private set; }
    DoorType preDoorType;
    [field: SerializeField] public RoomType roomType { get; private set; }
    [field: SerializeField] public Door door { get; private set; }
    [field: SerializeField] public GameObject map { get; private set; }


    bool preTop;
    [field: SerializeField] public bool top {get; set;}
    bool preBottom;
    [field: SerializeField] public bool bottom { get; set; }
    bool preLeft;
    [field: SerializeField] public bool left { get; set; }
    bool preRight;
    [field: SerializeField] public bool right { get; set; }

    public GameObject floorArea;
    public GameObject minimapIcon;

    RoomManager roomManager;
    MapTemplates mapTemplates;
    TileBaseTemplate tileBaseTemplate;

    void Start() {
        roomManager = FindObj.instance.roomManagerScript;
        mapTemplates = FindObj.instance.roomManager.GetComponent<MapTemplates>();
        tileBaseTemplate = FindObj.instance.roomManager.GetComponent<TileBaseTemplate>();

        roomManager.rooms.Add(this.gameObject);
        
        //this.transform.SetParent(roomManager.transform);

        preMapType = mapType;
        preDoorType = doorType;

        SetSprite();

        preTop = top;
        preBottom = bottom;
        preLeft = left;
        preRight = right;
    }

    void Update()
    {
        SetReRoom();
        SetMap();
        SetDoor();
        LockTrigger();
        UnLockTrigger();
    }

    /// <summary>
    /// 방을 다시 변경한다.
    /// </summary>
    void SetReRoom()
    {
        if(roomManager.finish && preTop != top || preBottom != bottom || preLeft != left || preRight != right)
        {
            // 변경할 방을 생성
            int roomIndex = GetRoomIndex();
            GameObject instObj = Instantiate(roomManager.roomTemplates.rooms[roomIndex], transform.position, roomManager.roomTemplates.rooms[roomIndex].transform.rotation);
            instObj.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 1);
            instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;

            // 현재 방의 정보를 변경할 방에게 계승
            instObj.GetComponent<Room>().mapType = mapType;
            
            // 현재 방의 정보 삭제
            roomManager.rooms.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 방을 챕터에 맞는 스프라이트로 변경한다.
    /// </summary>
    void SetSprite()
    {
        int nowChapter = DataManager.instance.userData.nowChapter - 1;
        Tilemap m_Tilemap = GetComponentInChildren<Tilemap>();

        if (nowChapter == 0) 
            return;

        for (int i = 0; i < tileBaseTemplate.swapChapter[0].swapTileBase.Length; ++i)
        {
            m_Tilemap.SwapTile(tileBaseTemplate.swapChapter[0].swapTileBase[i], tileBaseTemplate.swapChapter[nowChapter].swapTileBase[i]);
        }
    }
    
    /// <summary>
    /// 방에 맵 타입에 맞게 변경한다.
    /// </summary>
    void SetMap()
    {
        if(preMapType != mapType)
        {
            int ran;

            Destroy(map);
            Destroy(minimapIcon);

            preMapType = mapType;

            if (mapType == MapType.Reward)
            {
                ran = UnityEngine.Random.Range(0, mapTemplates.RewardMap.Length);
                map = Instantiate(mapTemplates.RewardMap[ran], transform.position, transform.rotation);
                minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
                
            }
            else if (mapType == MapType.Mission)
            {
                doorType = DoorType.Trap;
                ran = UnityEngine.Random.Range(0, mapTemplates.MissionMap.Length);
                map = Instantiate(mapTemplates.MissionMap[ran], transform.position, transform.rotation);
                
                map.GetComponent<Mission>().roomScript = this;
                minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
                
            }
            else if (mapType == MapType.Boss)
            {
                doorType = DoorType.Trap;
                ran = UnityEngine.Random.Range(0, mapTemplates.BossMap.Length);
                map = Instantiate(mapTemplates.BossMap[ran], transform.position, transform.rotation);
                minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
                
            }
            else if(mapType == MapType.Default)
            {
                map = Instantiate(mapTemplates.GetDefaultMap(top, bottom, left, right), transform.position, transform.rotation);
                minimapIcon = null;
            }
            else if(mapType == MapType.None)
            {
                map = null;
                minimapIcon = null;
                return;
            }
          
            if (minimapIcon != null)
            {
                minimapIcon.transform.parent = transform;
                minimapIcon.transform.localScale = new Vector3(0.005f, 0.005f, 1f);
                minimapIcon.transform.position = this.transform.position;
            }
            
            map.GetComponent<ObjectSpawn>().SpawnEnemy(mapType);
            map.transform.SetParent(this.transform);
            map.SetActive(false);
            
        }
    }

    //
    // None
    // Key
    // Trap
    // Both
    // Shabby
    // Wall
    void SetDoor()
    {
        //
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

    // key, Trap
    void LockTrigger()
    {
        if(lockTrigger)
        {
            door.LockDoor();
            lockTrigger = false;
        }
    }

    // key, Trap
    void UnLockTrigger()
    {
        if(unLockTrigger)
        {
            door.UnLockDoor();
            unLockTrigger = false;
        }
    }

    //
    public void SetMapManager(MapType mapType)
    {
        this.mapType = mapType;
    }

    public int GetRoomIndex()
    {
        int BitNumber = (Convert.ToByte(top) << 0) | (Convert.ToByte(bottom) << 1) | (Convert.ToByte(left) << 2) | (Convert.ToByte(right) << 3);
        return BitNumber;
    }

}
