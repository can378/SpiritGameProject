using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameData : MonoBehaviour
{

    public static GameData instance;

    public List<GameObject> selectItemList;         //1부터 시작
    public List<GameObject> weaponList;             //2부터 시작
    public List<GameObject> equipmentList;          //3부터 시작
    public List<GameObject> skillList;              //4부터 시작
    public List<GameObject> testList;
    public List<GameObject> statusEffectList;       //5부터 시작

    void Start()
    {
        instance = this;
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
