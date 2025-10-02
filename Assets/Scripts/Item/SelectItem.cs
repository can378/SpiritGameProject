using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SelectItem : MonoBehaviour, Interactable
{
    [HideInInspector] public ItemData itemData { get; protected set; }
    public string GetInteractText()
    {
        return "�ݱ�";
    }

    public void Interact()
    {
        Player.instance.GainSelectItem(this);
    }

    public Color GetRatingColor()
    {
        switch (itemData.selectItemRating)
        {
            case SelectItemRating.Temp:
                return Color.gray;
            case SelectItemRating.Normal:
                return Color.black;
            case SelectItemRating.Rare:
                return Color.blue;
            case SelectItemRating.Epic:
                return new Color(0.5f, 0.0f, 0.5f);
            case SelectItemRating.Legend:
                return new Color(1.0f, 0.8f, 0.0f);
            default:
                return Color.white;
        }
    }

    #region Sort CompareFunc
    static int compareClass(SelectItem a, SelectItem b)
    {
        if (a.itemData.selectItemType < b.itemData.selectItemType)
        {
            return -1;
        }
        else if (a.itemData.selectItemType > b.itemData.selectItemType)
        {
            return 1;
        }
        return 0;
    }

    static int compareRating(SelectItem a, SelectItem b)
    {
        if (a.itemData.selectItemRating < b.itemData.selectItemRating)
        {
            return -1;
        }
        else if (a.itemData.selectItemRating > b.itemData.selectItemRating)
        {
            return 1;
        }
        return 0;
    }

    static int compareID(SelectItem a, SelectItem b)
    {
        return a.itemData.selectItemID < b.itemData.selectItemID ? -1 : 1;
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

