using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Equipment
{
    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null)
        {
            return;
        }

        if (10 <= user.weaponList[user.playerStats.weapon].weaponType || user.playerStatus.isAttack)
        {
            
        }
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("보호막 생성");
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = null;
        }
    }
}
