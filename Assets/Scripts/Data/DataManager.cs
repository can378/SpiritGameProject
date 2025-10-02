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
            persistentData =new PersistentData();

            InitData();
            InitPersistentData();

            SaveUserData();
            SavePersistentData();
        }

    }

    public void SaveUserData()
    {
        // userData�� player �������� ������
        //userData.playerLevel = Player.instance.playerStats.level;
        //userData.playerExp = Player.instance.playerStats.exp;
        //userData.playerPoint = Player.instance.playerStats.point;

        userData.playerHP = Player.instance.playerStats.HP;
        userData.playerTempHP = Player.instance.playerStats.tempHP;

        userData.playerCoin = Player.instance.playerStats.coin;
        userData.playerKey = Player.instance.playerStats.key;
        userData.playerDice = Player.instance.playerStats.dice;

        userData.playerWeapon = Player.instance.playerStats.weapon.weaponData == null ? 0 : Player.instance.playerStats.weapon.weaponData.selectItemID;

        for(int i = 0;i < Player.instance.playerStats.skill.Length ; i++)
        {
            userData.playerSkill[i] = Player.instance.playerStats.skill[i];
        }
    
        for(int i = 0;i < Player.instance.playerStats.equipments.Length; i++)
        {
            userData.playerEquipments[i] = Player.instance.playerStats.equipments[i] != null ? Player.instance.playerStats.equipments[i].selectItemID : 0;
        }

        for (int i = 0; i < 8; i++)
        {
            userData.playerStat[i] = Player.instance.playerStats.playerStat[i];
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
        //��������� ������ ����
        SavePersistentData();
    }

}