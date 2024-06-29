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
        if (user.playerStats.HP + healValue > user.playerStats.HPMax)
        {
            user.playerStats.HP = user.playerStats.HPMax;
        }
        else
        {
            user.playerStats.HP += healValue;
        }
    }
}
