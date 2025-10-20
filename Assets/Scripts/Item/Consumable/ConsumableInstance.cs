using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ConsumableInstance : ItemInstance
{
    public ConsumableData consumableData; // SO ÂüÁ¶

    public override void Init()
    {
        itemData = consumableData;
    }

}
