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

    public UserData userData;
    private GameData gameVar;//���� ������ ���߿� ����

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
        
        gameVar = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();//���� ������ ���߿� ����
        LoadData();
    }

    public void LoadData()
    {
       
        string filePath = Application.persistentDataPath + UserDataFileName;

        if (File.Exists(filePath))
        {
            //json���� �ҷ�����
            string jsonData = File.ReadAllText(filePath);
            //������ȭ
            userData = JsonConvert.DeserializeObject<UserData>(jsonData);
        }
        else
        {
            Debug.Log("���ο� ������ ����");
            userData = new UserData();
            InitData();
            SaveData();
        }

    }

    public void SaveData()
    {
        //���� ���� ���
        string filePath = Application.persistentDataPath + UserDataFileName;

        //������ ����ȭ
        string jsonData = JsonConvert.SerializeObject(userData);

        File.WriteAllText(filePath, jsonData);
        Debug.Log("���� ��ġ : " + filePath);
    }

    public void InitData()
    {
        userData.coin = 0;
        userData.health = 100;
    }

    void OnApplicationQuit()
    {
        //��������� ������ ����
        SaveData();
    }

}
