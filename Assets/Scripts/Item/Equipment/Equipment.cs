using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsRating {General, Rare, Unique, Legend }

public abstract class Equipment : SelectItem
{
    [field: SerializeField] public int equipmentId { get; private set; }
    [field: SerializeField] public string equipmentName { get; private set; }
    [field: SerializeField] public EquipmentsRating equipmentRating { get; private set; }

    public abstract void Equip();
    public abstract void UnEquip();
    
}
