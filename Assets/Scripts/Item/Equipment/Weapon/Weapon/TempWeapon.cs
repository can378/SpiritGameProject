using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeapon : Weapon
{
    public override void Equip()
    {
        Player.instance.stats.power+= 0.25f;
    }

    public override void UnEquip()
    {
        Player.instance.stats.power -= 0.25f;
    }
}
