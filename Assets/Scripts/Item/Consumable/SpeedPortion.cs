using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPortion : Consumable
{
    public override void UseItem(Player user)
    {
        if(finishUse)
            return;
        print("�̵��ӵ�, ���ݼӵ� ����");
    }
}
