using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suit : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("�÷��̾� ���� �ӵ� ����");
        Player.instance.userData.playerAttackSpeed += 1f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.userData.playerAttackSpeed -= 1f;
    }
}
