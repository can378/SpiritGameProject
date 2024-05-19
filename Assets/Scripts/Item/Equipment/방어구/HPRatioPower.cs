using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HPRatioPower : Equipment
{
    // 체력 수치가 바뀌면 다시 계산해서 적용

    [SerializeField] float ratio;   // 잃은 체력 1%당 수치
    [SerializeField] float maxPowerUP;
    [SerializeField] float curLostHP;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null)
            return;

        if (curLostHP == (float)Math.Round((user.stats.HPMax - user.stats.HP) / user.stats.HPMax, 2))
            return;

        RePower();
    }

    void RePower()
    {
        user.playerStats.increasedAttackPower -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
        user.playerStats.increasedSkillPower -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);

        curLostHP = (float)Math.Round((user.stats.HPMax - user.stats.HP) / user.stats.HPMax, 2);

        user.playerStats.increasedAttackPower += Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
        user.playerStats.increasedSkillPower += Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);

        Debug.Log(Mathf.Clamp(ratio * curLostHP,0,maxPowerUP) * 100 + "% powerUp");
    }

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            this.user = target;
            Debug.Log("체력 비례 공격력, 도력 증가");
            curLostHP = 0;
            RePower();
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            user.playerStats.increasedAttackPower -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
            user.playerStats.increasedSkillPower -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
            this.user = null;
        }
    }


}
