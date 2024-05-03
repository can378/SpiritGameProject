using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DokkaebiWeapon : Weapon
{
    // 5초 마다 무기를 휘두르고 공격을 마치면 코인을 획득한다.
    [SerializeField] float coinCoolTime;
    [SerializeField] int gainCoin;

    float coinDelay;
    bool attacking;

    public override void Equip(Player target)
    {
        base.Equip(target);
    }

    void Update() {
        Passive();
    }

    void Passive()
    {
        if (target == null)
            return;

        if (target.isAttack)
        {
            attacking = true;
        }

        if (attacking == true && target.isAttack == false)
        {
            attacking = false;
            if (coinDelay <= 0)
            {
                target.playerStats.coin += gainCoin;
                MapUIManager.instance.UpdateCoinUI();
                coinDelay = coinCoolTime;
            }
        }
        coinDelay -= Time.deltaTime;
    }

    public override void UnEquip(Player target)
    {
        base.UnEquip(target);
    }
}
