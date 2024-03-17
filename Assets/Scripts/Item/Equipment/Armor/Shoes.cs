using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Equipment
{
    public override void Equip()
    {
        Debug.Log("�÷��̾� �̵��ӵ� ����");
        Player.instance.stats.addMoveSpeed += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.addMoveSpeed -= 0.5f;
    }
}
