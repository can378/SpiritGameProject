using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWand : Weapon
{
    public override void Equip(Player target)
    {
        base.Equip(target);
        Player.instance.stats.addSkillPower += 10;
    }

    public override void UnEquip(Player target)
    {
        base.UnEquip(target);
        Player.instance.stats.addSkillPower -= 10;
    }
}
