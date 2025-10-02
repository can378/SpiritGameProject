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
    [field: SerializeField, TextArea,Tooltip("���� �κ� template")] public string description { get; private set; } // ���ڸ� �־ ���� ��Ȯ�� ǥ���� �� ����
    [field: SerializeField] public int price { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }


    // �ڽ� Ŭ����(Weapon, Skill ��)���� ���ڸ� �ִµ� ����� �� �ִ�.
    public virtual string Update_Description(Stats _Stats)
    {
        return string.Format(description);
    }
}
