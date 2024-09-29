using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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


    [field: SerializeField] public bool top {get; private set;}
    [field: SerializeField] public bool bottom { get; private set; }
    [field: SerializeField] public bool left { get; private set; }
    [field: SerializeField] public bool right { get; private set; }

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
    }

    void Update()
    {
        SetMap();
        SetDoor();
        LockTrigger();
        UnLockTrigger();
    }

    void SetSprite()
    {
        int nowChapter = DataManager.instance.userData.nowChapter - 1;
        Tilemap m_Tilemap = GetComponentInChildren<Tilemap>();

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
                ran = Random.Range(0, mapTemplates.RewardMap.Length);
                map = Instantiate(mapTemplates.RewardMap[ran], transform.position, transform.rotation);
                minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
                
            }
            else if (mapType == MapType.Mission)
            {
                doorType = DoorType.Trap;
                ran = Random.Range(0, mapTemplates.MissionMap.Length);
                map = Instantiate(mapTemplates.MissionMap[ran], transform.position, transform.rotation);
                
                map.GetComponent<Mission>().roomScript = this;
                minimapIcon = Instantiate(map.gameObject.GetComponent<MinimapIcon>().minimapIcon);
                
            }
            else if (mapType == MapType.Boss)
            {
                doorType = DoorType.Trap;
                ran = Random.Range(0, mapTemplates.BossMap.Length);
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

}
