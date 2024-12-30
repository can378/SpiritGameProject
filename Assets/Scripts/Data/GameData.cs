using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

[System.Serializable]
public class SelectItemList
{
    //public string SortCondition;
    //int[] StartIndex;
    public List<SelectItem> list;

    /*
    /// <summary>
    /// �з��� ������ �з��� ���� ������ ������ �з����� ������ �˾Ƽ� �����Ѵ�.
    /// </summary>
    /// <param name="_EnumNum">�з��� ����</param>
    /// <param name="_Name">������ �̸�</param>
    public void SetEnumNum(int _EnumNum, string _Name)
    {
        // �з� ������ �ּ� 2������ �Ѵ�.
        if(_EnumNum < 2)
            return;

        StartIndex = new int[_EnumNum + 1];

        for (int i = 0; i < _EnumNum; ++i)
        {
            // x�� �ʵ忡�� _Name�� ���� Property�� ã�� ���� ��ȯ�Ѵ�.
            // �׸��� i �� ���ϰ� i�� ã���� �����Ѵ�.
            StartIndex[i] = list.FindIndex(x => (int)x.GetType().GetProperty(_Name).GetValue(x) == i);
        }

        StartIndex[_EnumNum] = list.Count;
    }

    /// <summary>
    /// �ش� ���� ������ �� �������� �̴´�.
    /// </summary>
    /// <param name="_Type">���� ����</param>
    /// <returns></returns>
    public SelectItem DrawRandomItem(int _Type)
    {
        // ������ 0���ϰų� ���̺��� ū ���̶�� �����Ѵ�.
        if(_Type < 0 || StartIndex.Length <= _Type )
            return null;

        // �ش� ������ �������� ���� ���� �ʴٸ� �����Ѵ�.
        if (StartIndex[_Type] == -1)
            return null;

        int ItemIndex = UnityEngine.Random.Range(StartIndex[_Type], StartIndex[_Type + 1]);

        return list[ItemIndex];
    }
    */
}

public class GameData : MonoBehaviour
{

    public static GameData instance;

    public List<GameObject> equipmentList;          //1���� ����
    public List<GameObject> weaponList;             //2���� ����
    public List<GameObject> selectItemList;         //3���� ����
    public List<GameObject> skillList;              //4���� ����
    public List<GameObject> testList;
    public List<GameObject> statusEffectList;       //5���� ����
    
    /// <summary>
    /// 0��° �ε����� �������� ���ĵǾ�����
    /// 1��° �ε����� ��޺��� ���ĵǾ�����
    /// 
    /// �켱�� �ٸ������ �ǵ��� ����
    /// �ǵ帱 �ʿ� ������ ���ϰ�
    /// </summary>
    [field :SerializeField]
    public List<SelectItem> ItemList {get; private set;}

    void Awake()
    {
        // ���� -> ��� ������ ����
        ItemList.Sort(SelectItem.ClassSort);

        // Ȥ�� �� �ߺ� ����
        ItemList = ItemList.Distinct().ToList();

        /*
        // ������ ���� ===============================================
        // 0��° ����Ʈ�� �������� ���ĵǾ�����
        SelectItemList[0].SortCondition = "����";
        SelectItemList[0].list.Sort(SelectItem.ClassSort);

        // Equipments, Weapon, Consumable, Skill, END
        // ������ ���� ��ġ�� �̸� �����Ѵ�.
        SelectItemList[0].SetEnumNum((int)SelectItemType.END, "selectItemType");

        // ��޺� ���� =============================================
        // 1��° ����Ʈ�� ��޺��� ���ĵǾ�����
        SelectItemList[1].SortCondition = "���";
        SelectItemList[1].list = SelectItemList[0].list.ToList();
        SelectItemList[1].list.Sort(SelectItem.RatingSort);

        // Temp, Normal, Rare, Epic, Legend, END
        // ��޺� ���� ��ġ�� �̸� �����Ѵ�.
        SelectItemList[1].SetEnumNum((int)SelectItemRating.END, "selectItemRating");
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

    #region Random


    /// <summary>
    /// SelectItem�� ������� �ش� ������ ���� �Է��ϸ� ���ǿ� �´� ������ �� �������� ����
    /// </summary>
    /// <param name="_Name">�������� �� ���� �̸�</param>
    /// <param name="_Type">�ش� ������ ��</param>
    /// <returns>�ش� ���ǿ� �´� ������ ������</returns>
    public SelectItem DrawRandomItem(string _Name, int _Type)
    {
        List<SelectItem> FindList = ItemList.FindAll(x => (int)x.GetType().GetProperty(_Name).GetValue(x) == _Type);

        if (FindList.Count == 0)
        {   Debug.Log("�ش� ���ǿ� �´� ������ ����");
            return null;
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex];
    }

    /// <summary>
    /// SelectItem�� ������� �ش� ������ ���� �Է��ϸ� ���ǿ� �´� ������ �� �������� ����
    /// </summary>
    /// <param name="_NameType">�������� �� ���� �̸�, �ش� ������ ��</param>
    /// <returns></returns>
    public SelectItem DrawRandomItem(KeyValuePair<string, int> _NameType)
    {
        List<SelectItem> FindList = ItemList.FindAll(x => (int)x.GetType().GetProperty(_NameType.Key).GetValue(x) == _NameType.Value);

        if (FindList.Count == 0)
        {
            Debug.Log("�ش� ���ǿ� �´� ������ ����");
            return null;
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex];
    }

    /// <summary>
    /// ������ ������ �� ��
    /// SelectItem�� ������� �ش� ������ ���� �Է��ϸ� ���ǿ� �´� ������ �� �������� ����
    /// </summary>
    /// <param name="_NameType">�������� �� ���� �̸�, �ش� ������ ��</param>
    /// <returns></returns>
    public SelectItem DrawRandomItem(Dictionary<string, int> _NameType)
    {
        List<SelectItem> FindList = ItemList.ToList();

        foreach (KeyValuePair<string, int> Pair in _NameType)
        {
            FindList = FindList.FindAll(x => (int)x.GetType().GetProperty(Pair.Key).GetValue(x) == Pair.Value);
        }

        if (FindList.Count == 0)
        {
            Debug.Log("�ش� ���ǿ� �´� ������ ����");
            return null;
        }

        int ItemIndex = UnityEngine.Random.Range(0, FindList.Count);

        return FindList[ItemIndex];
    }

    #endregion Random

}
