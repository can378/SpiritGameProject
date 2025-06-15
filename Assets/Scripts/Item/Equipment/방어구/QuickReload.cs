using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickReload : Equipment
{
    //bool isQuick = false;

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

    /*
        if(!isQuick && user.playerStatus.isReload)
        {
            print("buff");
            isQuick = true;
            GameObject attackSpeedBuff = Instantiate(GameData.instance.statusEffectList[12]);
            AttackSpeedBuff ASBSE = attackSpeedBuff.GetComponent<AttackSpeedBuff>();
            ASBSE.SetDefaultDuration(2f);
            ASBSE.increasedAttackSpeed = 0.5f;
            user.ApplyBuff(attackSpeedBuff);
        }
        else if(isQuick && !user.playerStatus.isReload)
        {
            isQuick = false;
        }
        */
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
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
