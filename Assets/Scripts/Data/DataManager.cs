using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class DataManager : MonoBehaviour
{
    //++추후에 데이터 암호화 구현 필요

    public static DataManager instance = null;

    string UserDataFileName = "UserData.json";
    string PersistentDataFileName = "PersistentData.json";

    public UserData userData;
    public PersistentData persistentData;
    public GameData gameData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //씬 전환이 되었을때, 이전 씬의 인스턴스를 계속 사용하기 위해
            //새로운 씬의 게임오브젝트 제거
            Destroy(this.gameObject);
        }

        gameData = this.GetComponent<GameData>();
        LoadData();
    }

    public void LoadData()
    {

        string userDataFilePath = Application.persistentDataPath + UserDataFileName;
        string persistentDataFilePath = Application.persistentDataPath + PersistentDataFileName;

        if (File.Exists(userDataFilePath)&&File.Exists(persistentDataFilePath))
        {
            //json파일 불러오기
            string userDataJsonData = File.ReadAllText(userDataFilePath);
            string persistentJsonData = File.ReadAllText(persistentDataFilePath);
            //역직렬화
            userData = JsonConvert.DeserializeObject<UserData>(userDataJsonData);
            // userData로 player 스탯 적용하기
            persistentData=JsonConvert.DeserializeObject<PersistentData>(persistentJsonData);
        }
        else
        {
            Debug.Log("새로운 데이터 생성");
            
            userData = new UserData();
            persistentData =new PersistentData();

            InitData();
            InitPersistentData();

            SaveUserData();
            SavePersistentData();
        }

    }

    public void SaveUserData()
    {
        // userData를 player 스탯으로 덮어씌우기
        //userData.playerLevel = Player.instance.playerStats.level;
        //userData.playerExp = Player.instance.playerStats.exp;
        //userData.playerPoint = Player.instance.playerStats.point;

        userData.playerHP = Player.instance.playerStats.HP;
        userData.playerTempHP = Player.instance.playerStats.tempHP;

        userData.playerCoin = Player.instance.playerStats.coin;
        userData.playerKey = Player.instance.playerStats.key;
        userData.playerDice = Player.instance.playerStats.dice;

        userData.playerWeapon = Player.instance.playerStats.weapon.weaponInstance.weaponData == null ? 1 : Player.instance.playerStats.weapon.weaponInstance.weaponData.selectItemID;

        for(int i = 0;i < Player.instance.playerStats.skill.Length ; i++)
        {
            if(Player.instance.playerStats.skill[i].IsValid())
                userData.playerSkill[i] = Player.instance.playerStats.skill[i].itemData.selectItemID;
        }
    
        for(int i = 0;i < Player.instance.playerStats.equipments.Length; i++)
        {
            userData.playerEquipments[i] = Player.instance.playerStats.equipments[i].IsValid() ? Player.instance.playerStats.equipments[i].equipmentData.selectItemID : 0;
        }

        for (int i = 0; i < 8; i++)
        {
            userData.playerStat[i] = Player.instance.playerStats.playerStat[i];
        }

        //파일 저장 경로
        string userDataFilePath = Application.persistentDataPath + UserDataFileName;

        //데이터 직렬화
        string userJsonData = JsonConvert.SerializeObject(userData);

        File.WriteAllText(userDataFilePath, userJsonData);

        Debug.Log("user data : " + userDataFilePath);
    }

    public void SavePersistentData()
    {
        //파일 저장 경로
        string persistentDataFilePath = Application.persistentDataPath + PersistentDataFileName;

        //데이터 직렬화
        string persistentJsonData = JsonConvert.SerializeObject(persistentData);

        File.WriteAllText(persistentDataFilePath, persistentJsonData);

        Debug.Log("persistent data : " + persistentDataFilePath);
    }

    public void InitData()
    {
        print("Init");
        //userData.playerLevel = 1;
        //userData.playerExp = 1;
        //userData.playerPoint = 5;

        userData.playerHP = 100f;
        userData.playerTempHP = 0;

        userData.playerCoin = 0;
        userData.playerKey = 0;
        userData.playerDice = 0;

        userData.playerItem = 0;

        userData.playerWeapon = 0;

        userData.playerMaxEquipments = 3;
        for (int i = 0; i < userData.playerEquipments.Length; i++)
        {
            userData.playerEquipments[i] = 0;
        }

        userData.playerMaxSkillSlot = 1;
        for (int i = 0; i < userData.playerSkill.Length; i++)
        {
            userData.playerSkill[i] = 0;
        }

        for (int i = 0; i < userData.playerStat.Length; i++)
        {
            userData.playerStat[i] = 0;
        }


        userData.nowChapter = 0;

    }

    public void InitPersistentData()
    {
        persistentData.BGSoundVolume = 10;
        persistentData.SFXSoundVolume = 10;
    }



    void OnApplicationQuit()
    {
        //게임종료시 데이터 저장
        SavePersistentData();
    }

}