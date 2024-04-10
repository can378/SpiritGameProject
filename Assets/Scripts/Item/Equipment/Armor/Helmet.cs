using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Equipment
{
    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 주문력 +50% 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillPower += 10f;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillPower -= 10f;
        }
    }
}
