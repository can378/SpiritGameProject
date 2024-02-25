using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : Armor
{
    public override void EquipArmor()
    {
        Player.instance.userData.playerPower += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquipArmor()
    {
        Player.instance.userData.playerPower -= 0.5f;
    }
}
