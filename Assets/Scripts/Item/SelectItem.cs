using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SelectItem : MonoBehaviour, Interactable
{
    [HideInInspector] public ItemInstance itemInstance;
    public string GetInteractText()
    {
        return "줍기";
    }

    public void Interact()
    {
        Player.instance.GainSelectItem(this);
    }

    #region Sort CompareFunc
    static int compareClass(SelectItem a, SelectItem b)
    {
        if (a.itemInstance.itemData.selectItemType < b.itemInstance.itemData.selectItemType)
        {
            return -1;
        }
        else if (a.itemInstance.itemData.selectItemType > b.itemInstance.itemData.selectItemType)
        {
            return 1;
        }
        return 0;
    }

    static int compareRating(SelectItem a, SelectItem b)
    {
        if (a.itemInstance.itemData.selectItemRating < b.itemInstance.itemData.selectItemRating)
        {
            return -1;
        }
        else if (a.itemInstance.itemData.selectItemRating > b.itemInstance.itemData.selectItemRating)
        {
            return 1;
        }
        return 0;
    }

    static int compareID(SelectItem a, SelectItem b)
    {
        return a.itemInstance.itemData.selectItemID < b.itemInstance.itemData.selectItemID ? -1 : 1;
    }
    
    // 후에 가격별 정렬 추가?

    #endregion Sort ComapreFunc

    #region Sort
    public static int ClassSort(SelectItem a, SelectItem b)
    {
        // 유형
        if(compareClass(a, b) != 0)
            return compareClass(a,b);
        // 유형이 같다면 등급으로
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // 등급도 같다면 ID로
        return compareID(a, b);
    }

    public static int RatingSort(SelectItem a, SelectItem b)
    {
        // 등급
        if (compareRating(a, b) != 0)
            return compareRating(a, b);
        // 등급이 같다면 유형으로
        if (compareClass(a, b) != 0)
            return compareClass(a, b);
        // 유형도 같다면 ID로
        return compareID(a, b);
    }
    #endregion

}

