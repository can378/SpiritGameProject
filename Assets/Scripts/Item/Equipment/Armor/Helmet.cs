using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("�÷��̾� �ֹ��� ����");
        Player.instance.userData.skillPower += 1f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.userData.skillPower -= 1f;
    }
}