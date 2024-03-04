using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("플레이어 주문력 증가");
        Player.instance.stats.skillPower += 1f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.stats.skillPower -= 1f;
    }
}
