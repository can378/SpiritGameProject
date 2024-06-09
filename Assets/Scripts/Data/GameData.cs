using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameData : MonoBehaviour
{

    public static GameData instance;

    public List<GameObject> selectItemList;         //1���� ����
    public List<GameObject> weaponList;             //2���� ����
    public List<GameObject> equipmentList;          //3���� ����
    public List<GameObject> skillList;              //4���� ����
    public List<GameObject> testList;
    public List<GameObject> statusEffectList;       //5���� ����

    void Start()
    {
        instance = this;
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
