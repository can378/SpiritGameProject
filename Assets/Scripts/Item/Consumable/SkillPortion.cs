using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPortion : Consumable
{
    public override void UseItem(Player user)
    {
        if(finishUse)
            return;
        print("���� ����, ���� ���ð� ����");
    }
}
