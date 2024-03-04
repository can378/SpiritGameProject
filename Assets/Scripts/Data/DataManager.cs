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

        gameData = this.GetComponent<GameData>();
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
            // userData�� player ���� �����ϱ�
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
        // userData�� player �������� ������
        userData.playerLevel = Player.instance.stats.level;
        userData.playerExp = Player.instance.stats.exp;
        userData.playerPoint = Player.instance.stats.point;

        userData.playerHP = Player.instance.stats.HPMax;
        userData.playerTempHP = Player.instance.stats.tempHP;

        userData.playerCoin = Player.instance.stats.coin;
        userData.playerKey = Player.instance.stats.key;
        userData.playerDice = Player.instance.stats.dice;

        if (Player.instance.stats.mainWeapon != null) userData.playerMainWeapon = Player.instance.stats.mainWeapon.equipmentsId;
        // if (Player.instance.stats.playerArmor != null) userData.playerArmor = Player.instance.stats.armor.equipmentsId;
        if (Player.instance.stats.skill != null) userData.playerSkill = Player.instance.stats.skill.skillID;

        for (int i = 0; i < 8; i++)
        {
            DataManager.instance.userData.playerStat[i] = Player.instance.stats.playerStat[i];
        }

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
        userData.playerExp = 1;
        userData.playerPoint = 5;

        userData.playerHPMax = 100f;
        userData.playerHP = 100f;
        userData.playerTempHP = 0;

        userData.playerCoin = 0;
        userData.playerKey = 0;
        userData.playerDice = 0;

        userData.playerItem = "";

        userData.playerMainWeapon = 0;
        for (int i = 0; i < 3; i++)
        {
            userData.playerArmor[i] = 0;
        }

        userData.playerSkill = 0;

        userData.nowChapter = 0;

        userData.playerStat = new int[8];
        for (int i = 0; i < 8; i++)
        {
            userData.playerStat[i] = 0;
        }


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