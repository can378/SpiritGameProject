using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SelectItemClass { Equipments, Weapon, Consumable, Skill };
public enum SelectItemRating { Temp, Normal, Rare, Epic, Legend }

public class SelectItem : MonoBehaviour
{
    [field: SerializeField] public SelectItemClass selectItemClass {get; private set;}
    [field: SerializeField] public int selectItemID { get; private set; }
    [field: SerializeField] public string selectItemName { get; private set; }
    [field: SerializeField] public int price {get; private set;}
    [field: SerializeField] public SelectItemRating selectItemRating { get; private set; }
}

