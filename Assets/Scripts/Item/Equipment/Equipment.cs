using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentsRating { Temporary, General, Rare, Unique }

public abstract class Equipments : SelectItem
{
    [field: SerializeField] public int equipmentsId { get; private set; }
    [field: SerializeField] public string equipmentsName { get; private set; }
    [field: SerializeField] public EquipmentsRating equipmentsRating { get; private set; }
    public int equipmentsOption { get; set; }

    public abstract void Equip();
    public abstract void RandomOption(); 
    public abstract void UnEquip();
    
}
