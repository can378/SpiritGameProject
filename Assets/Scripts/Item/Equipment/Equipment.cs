using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsRating {General, Rare, Unique, Legend }

public abstract class Equipment : SelectItem
{
    [field: SerializeField] public int equipmentId { get; private set; }
    [field: SerializeField] public string equipmentName { get; private set; }
    [field: SerializeField] public EquipmentsRating equipmentRating { get; private set; }
    protected Player target;        // 장착한 대상

    public abstract void Equip(Player target);
    protected abstract void Passive();
    public abstract void UnEquip(Player target);
    
}
