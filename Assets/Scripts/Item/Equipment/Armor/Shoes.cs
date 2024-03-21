using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Equipment
{
    public override void Equip()
    {
        Debug.Log("플레이어 이동속도 X50% 증가");
        Player.instance.stats.increasedMoveSpeed += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.increasedMoveSpeed -= 0.5f;
    }
}
