using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suit : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("플레이어 받는 피해 감소");
        Player.instance.userData.playerReductionRatio += 0.25f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.userData.playerReductionRatio -= 0.25f;
    }
}
