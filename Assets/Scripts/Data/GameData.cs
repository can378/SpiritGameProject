using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameData : MonoBehaviour
{

    public static GameData instance;

    public List<GameObject> selectItemList;
    public List<GameObject> weaponList;
    public List<GameObject> equipmentList;
    public List<GameObject> skillList;
    public List<GameObject> testList;
    public List<GameObject> statusEffectList;
    public List<GameObject> simulEffect;            // 0은 동그라미, 1은 방향

    void Start()
    {
        instance = this;
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
