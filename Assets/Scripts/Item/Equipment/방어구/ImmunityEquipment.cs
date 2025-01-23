using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityEquipment : Equipment
{

    [SerializeField] BuffType ImmunBuffType;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            user.stats.addSEResist[(int)ImmunBuffType]++;
            Debug.Log(ImmunBuffType + "��ȣ ������� �鿪");
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            user.stats.addSEResist[(int)ImmunBuffType]--;
            this.user = null;
        }
    }
}
