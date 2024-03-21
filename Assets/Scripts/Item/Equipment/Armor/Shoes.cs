using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Equipment
{
    public override void Equip()
    {
        Debug.Log("�÷��̾� �̵��ӵ� X50% ����");
        Player.instance.stats.increasedMoveSpeed += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.increasedMoveSpeed -= 0.5f;
    }
}
