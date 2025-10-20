using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public string id; // UUID

    [HideInInspector] public ItemData itemData; // SO ÂüÁ¶

    public virtual void Init()
    {}

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
}
