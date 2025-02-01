using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeapon : Weapon
{
    public override void Equip(Player user)
    {
        base.Equip(user);
        Player.instance.stats.AttackPower.IncreasedValue += 0.25f;
    }

    public override void UnEquip(Player user)
    {
        base.UnEquip(user);
        Player.instance.stats.AttackPower.IncreasedValue -= 0.25f;
    }
}
