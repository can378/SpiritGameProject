using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWand : Weapon
{
    public override void Equip()
    {
        base.Equip();
        Player.instance.stats.addSkillPower += 10;
    }

    public override void UnEquip()
    {
        base.UnEquip();
        Player.instance.stats.addSkillPower -= 10;
    }
}
