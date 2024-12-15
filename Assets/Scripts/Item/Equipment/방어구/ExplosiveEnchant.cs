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
            user.AddEnchant_Projectile(PROJECTILE_TYPE.Explosion);
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            user.RemoveEnchant_Projectile(PROJECTILE_TYPE.Explosion);
        }
    }
}
