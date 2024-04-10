using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : Equipment
{
    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 공격력 +50% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower += 0.5f;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 공격력 +50% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower += 0.5f;
        }
    }
}
