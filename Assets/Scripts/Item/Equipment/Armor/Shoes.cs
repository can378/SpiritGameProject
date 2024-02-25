using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Armor
{
    public override void EquipArmor()
    {
        Player.instance.userData.playerSpeed += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquipArmor()
    {
        Player.instance.userData.playerSpeed -= 0.5f;
    }
}
