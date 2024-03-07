using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsRating { Temporary, General, Rare, Unique }

public abstract class Equipment : SelectItem
{
    [field: SerializeField] public int equipmentId { get; private set; }
    [field: SerializeField] public string equipmentName { get; private set; }
    [field: SerializeField] public EquipmentsRating equipmentRating { get; private set; }
    [field: SerializeField] public int equipmentOption { get; set; }

    public abstract void Equip();
    public abstract void RandomOption(); 
    public abstract void UnEquip();
    
}
