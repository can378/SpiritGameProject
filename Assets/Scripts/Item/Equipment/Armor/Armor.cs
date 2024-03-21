using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public override void Equip()
    {
        print("플레이어 받는 피해 +25% 증가");
        Player.instance.stats.addDefensivePower -= 0.25f;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addDefensivePower -= 0.25f;
    }


}
