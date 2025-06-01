using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SelectItemType { Equipments, Weapon, Consumable, Skill, END };
public enum SelectItemRating { Temp, Normal, Rare, Epic, Legend, END}

public class SelectItem : MonoBehaviour, Interactable
{
    [field: SerializeField] public SelectItemType selectItemType {get; private set;}
    [field: SerializeField] public SelectItemRating selectItemRating { get; private set; }
    [field: SerializeField] public string selectItemName { get; private set; }
    [field: SerializeField] public string description { get; private set; }
    [field: SerializeField] public int price {get; private set;}
    [field: SerializeField] public int selectItemID { get; private set; }

    public string GetInteractText()
    {
        return "�ݱ�";
    }

    public void Interact()
    {
        Player.instance.GainSelectItem(this);
    }

    #region Sort CompareFunc
    static int compareClass (SelectItem a, SelectItem b)
    {
        if (a.selectItemType < b.selectItemType)
        {
            return -1;
        }
        else if (a.selectItemType > b.selectItemType)
        {
            return 1;
        }
        return 0;
    }

    static int compareRating(SelectItem a, SelectItem b)
    {
        if (a.selectItemRating < b.selectItemRating)
        {
            return -1;
        }
        else if (a.selectItemRating > b.selectItemRating)
        {
            return 1;
        }
        return 0;
    }

    static int compareID(SelectItem a, SelectItem b)
    {
        return a.selectItemID < b.selectItemID ? -1 : 1;
    }
    
    // �Ŀ� ���ݺ� ���� �߰�?

    #endregion Sort ComapreFunc

    #region Sort
    public static int ClassSort(SelectItem a, SelectItem b)
    {
        // ����
        if(compareClass(a, b) != 0)
            return compareClass(a,b);
        // ������ ���ٸ� �������
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // ��޵� ���ٸ� ID��
        return compareID(a, b);
    }

    public static int RatingSort(SelectItem a, SelectItem b)
    {
        // ���
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // ����� ���ٸ� ��������
        if (compareClass(a, b) != 0)
            return compareClass(a, b);
        // ������ ���ٸ� ID��
        return compareID(a, b);
    }
    #endregion

}

