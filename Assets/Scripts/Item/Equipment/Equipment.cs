using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : SelectItem
{
    [field: SerializeField] public int equipmentRating { get; private set; }
    public Player user;        // ������ ���

    public abstract void Equip(Player user);
    public abstract void UnEquip(Player user);
    
}
