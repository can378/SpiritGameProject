using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPowerPlus : Equipment
{
    // 도력 증가
    // + 수치
    [SerializeField] int variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 도력 +" + variation + " 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillPower += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillPower -= variation;
        }
    }
}
