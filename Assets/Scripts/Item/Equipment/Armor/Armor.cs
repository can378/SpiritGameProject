using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    public override void Equip()
    {
        print("�÷��̾� �޴� ���� +25% ����");
        Player.instance.stats.addDefensivePower -= 0.25f;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addDefensivePower -= 0.25f;
    }


}
