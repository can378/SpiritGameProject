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

    void Start()
    {
        instance = this;
    }
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
