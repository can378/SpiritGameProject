using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class DataManager : MonoBehaviour
{
    //++���Ŀ� ������ ��ȣȭ ���� �ʿ�

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
            //�� ��ȯ�� �Ǿ�����, ���� ���� �ν��Ͻ��� ��� ����ϱ� ����
            //���ο� ���� ���ӿ�����Ʈ ����
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
            //json���� �ҷ�����
            string userDataJsonData = File.ReadAllText(userDataFilePath);
            string persistentJsonData = File.ReadAllText(persistentDataFilePath);
            //������ȭ
            userData = JsonConvert.DeserializeObject<UserData>(userDataJsonData);
            persistentData=JsonConvert.DeserializeObject<PersistentData>(persistentJsonData);
        }
        else
        {
            Debug.Log("���ο� ������ ����");
            
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
        //���� ���� ���
        string userDataFilePath = Application.persistentDataPath + UserDataFileName;

        //������ ����ȭ
        string userJsonData = JsonConvert.SerializeObject(userData);

        File.WriteAllText(userDataFilePath, userJsonData);

        Debug.Log("user data : " + userDataFilePath);
    }

    public void SavePersistentData()
    {
        //���� ���� ���
        string persistentDataFilePath = Application.persistentDataPath + PersistentDataFileName;

        //������ ����ȭ
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
        //��������� ������ ����
        SavePersistentData();
    }

}