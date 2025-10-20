using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class EquipmentInstance : ItemInstance
{
    public EquipmentData equipmentData; // SO ÂüÁ¶

    public override void Init()
    {
        itemData = equipmentData;
    }
}
