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

    public UserData userData;
    private GameData gameVar;//쓸일 없으면 나중에 삭제

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
        
        gameVar = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();//쓸일 없으면 나중에 삭제
        LoadData();
    }

    public void LoadData()
    {
       
        string filePath = Application.persistentDataPath + UserDataFileName;

        if (File.Exists(filePath))
        {
            //json파일 불러오기
            string jsonData = File.ReadAllText(filePath);
            //역직렬화
            userData = JsonConvert.DeserializeObject<UserData>(jsonData);
        }
        else
        {
            Debug.Log("새로운 데이터 생성");
            userData = new UserData();
            InitData();
            SaveData();
        }

    }

    public void SaveData()
    {
        //파일 저장 경로
        string filePath = Application.persistentDataPath + UserDataFileName;

        //데이터 직렬화
        string jsonData = JsonConvert.SerializeObject(userData);

        File.WriteAllText(filePath, jsonData);
        Debug.Log("저장 위치 : " + filePath);
    }

    public void InitData()
    {
        userData.coin = 0;
        userData.health = 100;
    }

    void OnApplicationQuit()
    {
        //게임종료시 데이터 저장
        SaveData();
    }

}
