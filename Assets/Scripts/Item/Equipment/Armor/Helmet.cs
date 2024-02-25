using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Armor
{
    public override void EquipArmor()
    {
        Player.instance.userData.skillPower += 1f;
    }

    // Update is called once per frame
    public override void UnEquipArmor()
    {
        Player.instance.userData.skillPower -= 1f;
    }
}
