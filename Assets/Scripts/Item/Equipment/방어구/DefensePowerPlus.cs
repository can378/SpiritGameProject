using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePowerPlus : Equipment
{
    // 방어력 증가
    // +% 수치
    float variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            Debug.Log("플레이어 방어력+" + variation * 100 + "% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower += variation;
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower -= variation;
        }
    }


}
