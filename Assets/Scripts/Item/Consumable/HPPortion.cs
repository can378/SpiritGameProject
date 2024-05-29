using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPortion : Consumable
{
    public float healValue;

    public override void UseItem(Player user)
    {
        if(finishUse)
            return;
        user.playerStats.HP += healValue;
    }
}
