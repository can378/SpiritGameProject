using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public override void Equip()
    {
        print("플레이어 피해 감소 증가");
        Player.instance.stats.addReduction += 0.25f;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addReduction -= 0.25f;
    }


}
