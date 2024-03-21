using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : Equipment
{
    public override void Equip()
    {
        Debug.Log("�÷��̾� ���ݷ� X50% ����");
        Player.instance.stats.increasedAttackPower += 0.5f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.increasedAttackPower -= 0.5f;
    }
}
