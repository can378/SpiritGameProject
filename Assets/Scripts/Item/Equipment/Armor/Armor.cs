using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public override void Equip()
    {
        Player.instance.stats.addReduction += 0.25f;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addReduction -= 0.25f;
    }


}
