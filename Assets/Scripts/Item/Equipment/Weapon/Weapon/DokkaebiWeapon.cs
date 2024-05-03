using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DokkaebiWeapon : Weapon
{
    // 5�� ���� ���⸦ �ֵθ��� ������ ��ġ�� ������ ȹ���Ѵ�.
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
