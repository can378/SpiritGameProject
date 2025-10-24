using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public string id; // UUID

    public ItemData itemData; // SO 참조

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

    public void SetII(ItemData _ItemData)
    {
        itemData = _ItemData;
    }

    public bool IsValid()
    {
        // itemData는 ScriptableObject (UnityEngine.Object)이므로
        // Unity의 ==/!= 오버로딩을 사용해 파괴된(가짜) null을 올바르게 감지한다.
        return itemData != null;
    }
}
