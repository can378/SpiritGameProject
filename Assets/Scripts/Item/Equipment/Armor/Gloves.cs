using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("�÷��̾� ���ݷ� ����");
        Player.instance.stats.power += 0.5f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.stats.power -= 0.5f;
    }
}
