using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerUp : Equipment
{
    // 공격력 증가
    // +%

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 공격력 +" + variation * 100 +"% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower -= variation;
        }
    }
}
