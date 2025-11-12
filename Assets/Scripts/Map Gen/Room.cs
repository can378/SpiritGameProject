using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public enum MapType { Default, Reward, Mission, Boss, None }

public class Room : MonoBehaviour
{

    [SerializeField] bool lockTrigger;
    [SerializeField] bool unLockTrigger;

    [SerializeField] event System.Action UnLockEvent;
    [SerializeField] event System.Action LockEvent;


    [field: SerializeField] public MapType mapType { get; private set; }
    MapType preMapType = MapType.None;
    [field: SerializeField] public DoorType doorType { get; private set; }
    DoorType preDoorType;
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

    public GameObject floorArea { get; set; }
    public GameObject minimapIcon { get; set; }

    RoomManager roomManager;
    MapTemplates mapTemplates;
    TileBaseTemplate tileBaseTemplate;

    void Start() {
        roomManager = FindObj.instance.roomManagerScript;
        mapTemplates = FindObj.instance.roomManager.GetComponent<MapTemplates>();
        tileBaseTemplate = FindObj.instance.roomManager.GetComponent<TileBaseTemplate>();

        roomManager.rooms.Add(this.gameObject);
        
        //this.transform.SetParent(roomManager.transform);

        //SetSprite();

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
    /// 諛⑹쓣 ?떎?떆 蹂?寃쏀븳?떎.
    /// </summary>
    void SetReRoom()
    {
        if(preTop == top && preBottom == bottom && preLeft == left && preRight == right)
            return;

        // 蹂?寃쏀븷 諛⑹쓣 ?깮?꽦
        int roomIndex = GetRoomIndex();
        GameObject instObj = Instantiate(roomManager.roomTemplates.rooms[roomIndex], transform.position, roomManager.roomTemplates.rooms[roomIndex].transform.rotation);
        instObj.transform.localScale = new Vector3(roomManager.roomSize, roomManager.roomSize, 1);
        instObj.transform.parent = GameObject.FindWithTag("roomParent").transform;

        // ?쁽?옱 諛⑹쓽 ?젙蹂대?? 蹂?寃쏀븷 諛⑹뿉寃? 怨꾩듅
        instObj.GetComponent<Room>().mapType = roomManager.ResetMapType(GetRoomWayType());
            
        // ?쁽?옱 諛⑹쓽 ?젙蹂? ?궘?젣
        roomManager.rooms.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 諛⑹쓣 梨뺥꽣?뿉 留욌뒗 ?뒪?봽?씪?씠?듃濡? 蹂?寃쏀븳?떎.
    /// </summary>
    void SetSprite()
    {
        int nowChapter = DataManager.instance.userData.nowChapter - 1;
        
        if (nowChapter == 0) 
            return;

        Tilemap m_Tilemap = GetComponentInChildren<Tilemap>();

        for (int i = 0; i < tileBaseTemplate.swapChapter[0].swapTileBase.Length; ++i)
        {
            m_Tilemap.SwapTile(tileBaseTemplate.swapChapter[0].swapTileBase[i], tileBaseTemplate.swapChapter[nowChapter].swapTileBase[i]);
        }
    }
    
    /// <summary>
    /// 諛⑹뿉 留? ????엯?뿉 留욊쾶 蹂?寃쏀븳?떎.
    /// </summary>
    void SetMap()
    {
        if(preMapType == mapType)
            return;

        int ran;

        Destroy(map);
        Destroy(minimapIcon);

        preMapType = mapType;

        if (mapType == MapType.Reward)
        {
            ran = UnityEngine.Random.Range(0, mapTemplates.RewardMap.Length);
            map = Instantiate(mapTemplates.GetRewardMap(top, bottom, left, right), transform.position, transform.rotation); 
            minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
          
        }
        else if (mapType == MapType.Mission)
        {
            doorType = DoorType.Trap;
            //ran = UnityEngine.Random.Range(0, mapTemplates.MissionMap.Length);
            GameObject missionMapPrefab = mapTemplates.GetMissionMap(top, bottom, left, right);

            if (missionMapPrefab == null)
            {
                //Debug.LogError("GetMissionMap() returned null. Skipping instantiation.");
                return;
            }

            if (missionMapPrefab.name.Contains("Maze"))//is this okay...?😅
            {
                Debug.Log("maze - no door trap");
                doorType = DoorType.None;
            }

            map = Instantiate(missionMapPrefab, transform.position, transform.rotation);
            map.GetComponent<RoomMissionBase>().SetRoom(this.GetComponent<Room>());
            minimapIcon = Instantiate(map.GetComponent<MinimapIcon>().minimapIcon);
            AddLockEvent(map.GetComponent<RoomMissionBase>().LockDoor);
        }

        else if (mapType == MapType.Boss)
        {
            // 임시로 안닫히게 처리
            doorType = DoorType.Trap;
            ran = UnityEngine.Random.Range(0, mapTemplates.BossMap.Length);
            map = Instantiate(mapTemplates.GetBossMap(top, bottom, left, right), transform.position, transform.rotation);
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
        //map.SetActive(false);
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

    public void AddLockEvent(System.Action lockAction)
    {
        LockEvent += lockAction;
    }

    public void AddUnLockEvent(System.Action unLockAction)
    {
        UnLockEvent += unLockAction;
    }


    // 문을 닫는다.
    public void LockDoor()
    {
        // 이미 닫혀있다면 반환
        if (door.IsClosed())
        {
            return;
        }
        LockEvent?.Invoke();
        lockTrigger = true;
    }

    public void UnLockDoor()
    {
        // 이미 열려있다면 반환
        if (!door.IsClosed())
        {
            return;
        }
        UnLockEvent?.Invoke();
        unLockTrigger = true;
    }

    // key, Trap
    void LockTrigger()
    {
        if(!lockTrigger)
            return;

        door.LockDoor();
        lockTrigger = false;
    }

    // key, Trap
    void UnLockTrigger()
    {
        if(!unLockTrigger)
            return;
        door.UnLockDoor();
        unLockTrigger = false;
    }

    //
    public void SetMapManager(MapType mapType)
    {
        this.mapType = mapType;
    }

    public int GetRoomIndex()
    {
        return (Convert.ToByte(top) << 0) | (Convert.ToByte(bottom) << 1) | (Convert.ToByte(left) << 2) | (Convert.ToByte(right) << 3);
    }

    public int GetRoomWayType()
    {
        return Convert.ToInt32(top) +  Convert.ToInt32(bottom) + Convert.ToInt32(left) + Convert.ToInt32(right);
    }

}
