using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnchant : Equipment
{
    // Åõ»çÃ¼°¡ ÆÄ±« ½Ã Æø¹ß

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            user.weaponController.enchant = 21;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            user.weaponController.enchant = 0;
        }
    }
}
