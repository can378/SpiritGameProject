using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Equipment
{
    public override void Equip()
    {
        Debug.Log("플레이어 주문력 추가");
        Player.instance.stats.addSkillPower += 10f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.addSkillPower -= 10f;
    }
}
