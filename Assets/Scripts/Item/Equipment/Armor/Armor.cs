using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            print("플레이어 방어력 +25% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower += 0.25f;
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower -= 0.25f;
        }
    }


}
