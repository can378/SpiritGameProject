using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePowerPlus : Equipment
{
    // 방어력 증가
    // +%p 수치
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 방어력+" + variation * 100 + "%p 증가");
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
            this.user = null;
        }
    }


}
