using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Equipment
{
    public override void Equip()
    {
        Debug.Log("�÷��̾� �ֹ��� ����");
        Player.instance.stats.addSkillPower += 1f;
    }

    // Update is called once per frame
    public override void UnEquip()
    {
        Player.instance.stats.addSkillPower -= 1f;
    }
}
