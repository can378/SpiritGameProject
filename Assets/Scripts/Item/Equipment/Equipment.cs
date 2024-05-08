using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsRating {General, Rare, Unique, Legend }

public abstract class Equipment : SelectItem
{
    [field: SerializeField] public int equipmentId { get; private set; }
    [field: SerializeField] public string equipmentName { get; private set; }
    [field: SerializeField] public EquipmentsRating equipmentRating { get; private set; }
    public Player user;        // 장착한 대상

    public abstract void Equip(Player user);
    public abstract void UnEquip(Player user);
    
}
