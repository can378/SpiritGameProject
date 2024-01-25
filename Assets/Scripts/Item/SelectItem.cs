using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectItemClass {Weapon, Consumable, Skill };

public class SelectItem : MonoBehaviour
{
    [field: SerializeField] public SelectItemClass selectItemClass {get; private set;}
}

