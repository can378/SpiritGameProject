using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SelectItem : MonoBehaviour, Interactable
{
    [HideInInspector] public ItemData itemData { get; protected set; }
    public string GetInteractText()
    {
        return "줍기";
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

