using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnchant : Equipment
{
    // ����ü�� �ı� �� ����

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            user.SetEnchant(ENCHANT_TYPE.Explosion);
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            user.SetEnchant(ENCHANT_TYPE.END);
        }
    }
}
