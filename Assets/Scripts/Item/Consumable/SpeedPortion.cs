using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPortion : Consumable
{
    public override void UseItem(Player user)
    {
        if(finishUse)
            return;
        print("이동속도, 공격속도 증가");
    }
}
