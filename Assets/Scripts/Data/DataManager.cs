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

        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
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
            persistentData=JsonConvert.DeserializeObject<PersistentData>(persistentJsonData);
        }
        else
        {
            Debug.Log("새로운 데이터 생성");
            
            userData = new UserData();
            persistentData=new PersistentData();

            InitData();
            InitPersistentData();

            SaveUserData();
            SavePersistentData();
        }

    }

    public void SaveUserData()
    {
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
        userData.playerLevel = 1;
        userData.playerExp = 0;

        userData.playerHPMax = 100;
        userData.playerHP = 100;
        userData.playerTempHP = 0;

        userData.playerPower = 1;
        userData.playerCritical = 0;
        userData.playerCriticalDamage = 0.5f;
        userData.playerDrain = 0;

        userData.playerSpeed = 5;
        userData.playerRunSpeed = 1.66f;
        userData.playerRunCoolTime = 5;

        userData.playerDodgeSpeed = 2;
        userData.playerDodgeTime = 0.6f;

        userData.playerLuck = 0;

        userData.coin = 0;
        userData.key = 0;
        userData.dice = 0;

        userData.playerItem = "";

        userData.mainWeapon = "";
        userData.subWeapon = "";

        userData.activeSkill = "";
        userData.passiveSkill = "";

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