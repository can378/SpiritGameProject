using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DokkaebiWeapon : Weapon
{
    // 기본 공격 적중 시 재화 + 1
    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null || !user.hitTarget)
            return;

        if (user.hitTarget.layer == LayerMask.NameToLayer("Wall"))
            return;
        
        user.playerStats.coin += 1;
        MapUIManager.instance.UpdateCoinUI();
    }

    public override void Equip(Player user)
    {
        base.Equip(user);
    }

    public override void UnEquip(Player user)
    {
        base.UnEquip(user);
    }
}
