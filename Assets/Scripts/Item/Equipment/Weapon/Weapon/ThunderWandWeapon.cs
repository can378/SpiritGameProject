using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWandWeapon : Weapon
{
    public override void Equip(Player user)
    {
        base.Equip(user);
        Player.instance.playerStats.addSkillPower += 10;
        Player.instance.SetEnchant(11);
    }

    public override void UnEquip(Player user)
    {
        base.UnEquip(user);
        Player.instance.playerStats.addSkillPower -= 10;
        Player.instance.SetEnchant(0);
    }
}
