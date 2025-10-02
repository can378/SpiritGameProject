using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectItemType { Equipments, Weapon, Consumable, Skill, END };
public enum SelectItemRating { Temp, Normal, Rare, Epic, Legend, END }

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/GenericItem")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public SelectItemType selectItemType { get; private set; }
    [field: SerializeField] public SelectItemRating selectItemRating { get; private set; }
    [field: SerializeField] public int selectItemID { get; private set; }

    [field: SerializeField] public string selectItemName { get; private set; }
    [field: SerializeField, TextArea,Tooltip("설명 부분 template")] public string description { get; private set; } // 인자를 넣어서 값을 정확히 표시할 수 있음
    [field: SerializeField] public int price { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }


    // 자식 클래스(Weapon, Skill 등)에서 인자를 넣는데 사용할 수 있다.
    public virtual string Update_Description(Stats _Stats)
    {
        return string.Format(description);
    }
}
