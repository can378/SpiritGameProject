using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

[System.Serializable]
public class SelectItemList
{
    int[] StartIndex;
    public List<SelectItem> list;

    /// <summary>
    /// 분류할 개수를 적으면 분류값의 시작을 알아서 저장한다.
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

        int ItemIndex = Random.Range(StartIndex[_Type], StartIndex[_Type + 1]);

        return list[ItemIndex];
    }
}

public class GameData : MonoBehaviour
{

    public static GameData instance;

    public List<GameObject> equipmentList;          //1부터 시작
    public List<GameObject> weaponList;             //2부터 시작
    public List<GameObject> selectItemList;         //3부터 시작
    public List<GameObject> skillList;              //4부터 시작
    public List<GameObject> testList;
    public List<GameObject> statusEffectList;       //5부터 시작
    
    /// <summary>
    /// 0번째 인덱스는 유형별로 정렬되어있음
    /// 1번째 인덱스는 등급별로 정렬되어있음
    /// 
    /// 우선은 다른사람은 건들지 말것
    /// 건드릴 필요 있으면 말하고
    /// </summary>
    [field :SerializeField]
    public List<SelectItemList> SelectItemList {get; private set;}

    void Awake()
    {
        // 유형별 정렬 ===============================================
        // 0번째 리스트는 유형별로 정렬되어있음
        SelectItemList[0].list.Sort(SelectItem.ClassSort);

        // Equipments, Weapon, Consumable, Skill, END
        // 유형별 시작 위치를 미리 저장한다.
        SelectItemList[0].SetEnumNum((int)SelectItemType.END, "selectItemType");

        /*
        for (int i = 0 ; i < (int)SelectItemType.END ; ++i)
        {
            ClassStartIndex[0] = SelectItemList[0].List.FindIndex(x => x.selectItemType == (SelectItemType)i);
        }
        ClassStartIndex[(int)SelectItemType.END] = SelectItemList[0].List.Count;
        */


        // 등급별 정렬 =============================================
        // 1번째 리스트는 등급별로 정렬되어있음
        SelectItemList[1].list = SelectItemList[0].list.ToList();
        SelectItemList[1].list.Sort(SelectItem.RatingSort);

        // Temp, Normal, Rare, Epic, Legend, END
        // 등급별 시작 위치를 미리 저장한다.
        SelectItemList[1].SetEnumNum((int)SelectItemRating.END, "selectItemRating");
        /*
        for (int i = 0; i < (int)SelectItemType.END; ++i)
        {
            RatingStartIndex[0] = SelectItemList[1].list.FindIndex(x => x.selectItemRating == (SelectItemRating)i);
        }
        ClassStartIndex[(int)SelectItemType.END] = SelectItemList[0].list.Count;
        RatingStartIndex[5] = SelectItemList[1].list.Count;
        */
    }

    void Start()
    {
        instance = this;

    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
