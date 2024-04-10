using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DokkaebiWeapon : Weapon
{
    [SerializeField] float coinCoolTime;

    public override void Equip(Player target)
    {
        base.Equip(target);
    }

    private void Update() {

        Passive();
    }

    protected override void Passive()
    {
        if (target == null)
            return;
        
        if (target.tag == "Player")
        {
            coinCoolTime -= Time.deltaTime;
            if (target.status.isAttack && coinCoolTime <= 0)
            {
                target.stats.coin++;
                coinCoolTime = 5;
            }
        }
    }

    public override void UnEquip(Player target)
    {
        base.UnEquip(target);
    }
}
