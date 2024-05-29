using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectItemClass { Equipments, Weapon, Consumable, Skill, ThrowWeapon };

public class SelectItem : MonoBehaviour
{
    [field: SerializeField] public SelectItemClass selectItemClass {get; private set;}
    [field: SerializeField] public string selectItemName { get; private set; }
    [field: SerializeField] public string price {get; private set;}
}

