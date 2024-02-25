using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("�÷��̾� �̵��ӵ� ����");
        Player.instance.userData.playerSpeed += 0.5f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.userData.playerSpeed -= 0.5f;
    }
}
