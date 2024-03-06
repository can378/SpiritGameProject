using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("플레이어 이동속도 증가");
        Player.instance.stats.speed += 0.5f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.stats.speed -= 0.5f;
    }
}
