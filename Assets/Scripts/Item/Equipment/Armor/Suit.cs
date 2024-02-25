using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suit : Armor
{
    public override void EquipArmor()
    {
        Player.instance.userData.playerReductionRatio += 0.25f;
    }

    // Update is called once per frame
    public override void UnEquipArmor()
    {
        Player.instance.userData.playerReductionRatio -= 0.25f;
    }
}
