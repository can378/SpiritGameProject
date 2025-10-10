using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentItem", menuName = "Item/Equipment")]
public class EquipmentData : ItemData
{
    [field: SerializeField] public List<PassiveData> passives { get; private set; }

    public override string Update_Description(Stats _Stats)
    {
        string des = description;

        if (passives.Count > 0)
            des += "\n";

        for (int i = 0; i < passives.Count; ++i)
        {
            des += "\n" + passives[i].Update_Description(_Stats);
        }
        return des;
    }
}
