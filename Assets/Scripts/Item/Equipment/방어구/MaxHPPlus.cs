using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPPlus : Equipment
{
    // 최대 체력 증가
    // + 수치
    [SerializeField] int variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 최대체력 +" + variation + " 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.HPMax += variation;
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.HPMax -= variation;
            this.user = null;
        }
    }


}
