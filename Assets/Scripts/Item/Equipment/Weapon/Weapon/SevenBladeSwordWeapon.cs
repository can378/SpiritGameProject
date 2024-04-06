using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeapon : Weapon
{
    public override void Equip()
    {
        base.Equip();
        Player.instance.stats.increasedAttackPower += 0.25f;
    }

    public override void UnEquip()
    {
        base.UnEquip();
        Player.instance.stats.increasedAttackPower -= 0.25f;
    }
}
