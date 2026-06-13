using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[System.Serializable]
public class SelectItemList
{
    //public string SortCondition;
    //int[] StartIndex;
    public List<SelectItem> list;

    /*
    /// <summary>
    /// 분류할 개수와 분류할 기준 변수를 적으면 분류값의 시작을 알아서 저장한다.
    /// </summary>
    /// <param name="_EnumNum">분류할 개수</param>
    /// <param name="_Name">변수의 이름</param>
    public void SetEnumNum(int _EnumNum, string _Name)
    {
        // 분류 개수는 최소 2개여야 한다.
        if(_EnumNum < 2)
            return;

        StartIndex = new int[_EnumNum + 1];

        for (int i = 0; i < _EnumNum; ++i)
        {
            // x의 필드에서 _Name을 가진 Property를 찾고 값을 반환한다.
            // 그리고 i 완 비교하고 i를 찾으면 저장한다.
            StartIndex[i] = list.FindIndex(x => (int)x.GetType().GetProperty(_Name).GetValue(x) == i);
        }

        StartIndex[_EnumNum] = list.Count;
    }

    /// <summary>
    /// 해당 종류 아이템 중 무작위로 뽑는다.
    /// </summary>
    /// <param name="_Type">뽑을 종류</param>
    /// <returns></returns>
    public SelectItem DrawRandomItem(int _Type)
    {
        // 종류가 0이하거나 길이보다 큰 값이라면 무시한다.
        if(_Type < 0 || StartIndex.Length <= _Type )
            return null;

        // 해당 종류의 아이템이 존재 하지 않다면 무시한다.
        if (StartIndex[_Type] == -1)
            return null;

        int ItemIndex = UnityEngine.Random.Range(StartIndex[_Type], StartIndex[_Type + 1]);

        return list[ItemIndex];
    }
    */
}

public class GameData : MonoBehaviour
{
    // 나중에 이것들 외부에서 변경 못하게 해야할 듯

    public static GameData instance;

    [Header("Equipment")]
    public List<GameObject> equipmentList;          // 장비 : 1부터 시작
    [Header("Weapon")]
    public List<GameObject> weaponList;             // 무기 : 2부터 시작
    [Header("Consumable")]
    public List<GameObject> selectItemList;         // 소모품 : 3부터 시작
    [Header("Skill")]
    public List<GameObject> skillList;              // 스킬 : 4부터 시작
    //public List<GameObject> testList;
    [field: SerializeField, Header("Buff")]
    public List<BuffData>   buffList { get; private set; }               // 버프

    [field: SerializeField, Header("Player Altar Stat")]
    public List<SOStatData> statList { get; private set; }               // 제단의 스탯 증감 정보
    
    [field :SerializeField, Header("All Item")]
    public List<SelectItem> ItemList {get; private set;}

    /// <summary>
    /// 빠른 불러오기 용 Dictionary
    /// 찾은 아이템의 리스트를 저장
    /// </summary>zeField]
    [field:SerializeField]SerializedDictionary<string,List<SelectItem>> ItemList_Sort = new SerializedDictionary<string, List<SelectItem>>();

    void Awake()
    {


        /*
        // 유형별 정렬 ===============================================
        // 0번째 리스트는 유형별로 정렬되어있음
        SelectItemList[0].SortCondition = "유형";
        SelectItemList[0].list.Sort(SelectItem.ClassSort);

        // Equipments, Weapon, Consumable, Skill, END
        // 유형별 시작 위치를 미리 저장한다.
        SelectItemList[0].SetEnumNum((int)SelectItemType.END, "selectItemType");

        // 등급별 정렬 =============================================
        // 1번째 리스트는 등급별로 정렬되어있음
        SelectItemList[1].SortCondition = "등급";
        SelectItemList[1].list = SelectItemList[0].list.ToList();
        SelectItemList[1].list.Sort(SelectItem.RatingSort);

        // Temp, Normal, Rare, Epic, Legend, END
        // 등급별 시작 위치를 미리 저장한다.
        SelectItemList[1].SetEnumNum((int)SelectItemRating.END, "selectItemRating");
        */
    }

    void Start()
    {
        instance = this;

        // 유형 -> 등급 순으로 정렬
        ItemList.Sort(SelectItem.ClassSort);

        // 혹시 모를 중복 제거
        ItemList = ItemList.Distinct().ToList();

    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    #region Random

    public List<SelectItem> FindItemList(Dictionary<string, int> _NameType)
    {
        string asString = string.Join(Environment.NewLine, _NameType);
        if (!ItemList_Sort.ContainsKey(asString))
        {
            return null;
        }
        return ItemList_Sort[asString];
    }

    public List<SelectItem> FindItemList(KeyValuePair<string, int> _NameType)
    {
        string asString = string.Join(Environment.NewLine, _NameType);

        if (!ItemList_Sort.ContainsKey(asString))
        {
            return null;
        }
        return ItemList_Sort[asString];
    }

    public List<SelectItem> FindItemList(string _Name, int _Type)
    {
        KeyValuePair<string, int> _NameType = new KeyValuePair<string, int>(_Name, _Type);

        string asString = string.Join(Environment.NewLine, _NameType);

        if (!ItemList_Sort.ContainsKey(asString))
        {
            return null;
        }
        return ItemList_Sort[asString];
    }


    /// <summary>
    /// SelectItem의 변수명과 해당 변수의 값을 입력하면 조건에 맞는 아이템 중 무작위로 뽑음
    /// </summary>
    /// <param name="_Name">조건으로 쓸 변수 이름</param>
    /// <param name="_Type">해당 변수의 값</param>
    /// <returns></returns>
    public GameObject DrawRandomItem(string _Name, int _Type)
    {
        KeyValuePair<string, int> _NameType = new KeyValuePair<string, int>(_Name, _Type);

        var asString = string.Join(Environment.NewLine, _NameType);

        List<SelectItem> FindList = FindItemList(_NameType);

        // 리스트가 없다면
        // 리스트 생성
        if (FindList == null)
        {
            FindList = ItemList.ToList();

            FindList = FindList.FindAll(x => (int)x.itemInstance.itemData.GetType().GetProperty(_NameType.Key).GetValue(x.itemInstance.itemData) == _NameType.Value);

            ItemList_Sort.Add(asString, FindList);
        }

        if (FindList.Count == 0)
        {
            return null;
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex].gameObject;
    }

    /// <summary>
    /// SelectItem의 변수명과 해당 변수의 값을 입력하면 조건에 맞는 아이템 중 무작위로 뽑음
    /// </summary>
    /// <param name="_NameType">조건으로 쓸 변수 이름, 해당 변수의 값</param>
    /// <returns></returns>
    public GameObject DrawRandomItem(KeyValuePair<string, int> _NameType)
    {
        var asString = string.Join(Environment.NewLine, _NameType);

        List<SelectItem> FindList = FindItemList(_NameType);

        // 리스트가 없다면
        // 리스트 생성
        if (FindList == null)
        {
            FindList = ItemList.ToList();

            FindList = FindList.FindAll(x => (int)x.itemInstance.itemData.GetType().GetProperty(_NameType.Key).GetValue(x.itemInstance.itemData) == _NameType.Value);

            ItemList_Sort.Add(asString, FindList);
        }

        if (FindList.Count == 0)
        {
            return null;
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex].gameObject;
    }

    /// <summary>
    /// 조건이 여러개 일 때
    /// SelectItem의 변수명과 해당 변수의 값을 입력하면 조건에 맞는 아이템 중 무작위로 뽑음
    /// </summary>
    /// <param name="_NameType">조건으로 쓸 변수 이름, 해당 변수의 값</param>
    /// <returns></returns>
    public GameObject DrawRandomItem(Dictionary<string, int> _NameType)
    {
        var asString = string.Join(Environment.NewLine, _NameType);

        List<SelectItem> FindList = FindItemList(_NameType);

        // 리스트가 없다면
        // 리스트 생성
        if(FindList == null)
        {
            FindList = ItemList.ToList();

            foreach (KeyValuePair<string, int> Pair in _NameType)
            {
                FindList = FindList.FindAll(x => (int)x.itemInstance.itemData.GetType().GetProperty(Pair.Key).GetValue(x.itemInstance.itemData) == Pair.Value);
            }
            
            ItemList_Sort.Add(asString, FindList);
        }

        if (FindList.Count == 0)
        {
            // return null;
            // 임시로 쿠키 제공으로
            // Debug.Log("아이템 없네요... 이거나 드셔");
            FindList = ItemList.FindAll(x => (int)x.itemInstance.itemData.GetType().GetProperty("selectItemType").GetValue(x.itemInstance.itemData) == (int)SelectItemType.Consumable);
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex].gameObject;
    }

    // 기본 회복 아이템 제공
    public GameObject GetDefaultConsumableItem()
    {
        List<SelectItem> FindList = ItemList.FindAll(x => (int)x.itemInstance.itemData.GetType().GetProperty("selectItemType").GetValue(x.itemInstance.itemData) == (int)SelectItemType.Consumable);
        int ItemIndex = 0;
        return FindList[ItemIndex].gameObject;
    }

    #endregion Random

}
