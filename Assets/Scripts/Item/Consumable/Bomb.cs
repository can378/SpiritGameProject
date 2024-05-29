using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Consumable
{
    public override void UseItem(Player user)
    {
        if(finishUse)
            return;
        Destroy(this.gameObject, 5f);
    }
}
