using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeapon : Weapon
{
    public override void Equip(Player target)
    {
        base.Equip(target);
        Player.instance.stats.increasedAttackPower += 0.25f;
    }

    public override void UnEquip(Player target)
    {
        base.UnEquip(target);
        Player.instance.stats.increasedAttackPower -= 0.25f;
    }
}
