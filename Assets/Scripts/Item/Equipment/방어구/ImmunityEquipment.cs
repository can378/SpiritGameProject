using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityEquipment : Equipment
{

    [SerializeField] int[] statusEffectID;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            for(int i = 0; i < statusEffectID.Length;i++)
            {
                user.stats.immunity[statusEffectID[i]]++;
                Debug.Log(statusEffectID[i] + "��ȣ ������� �鿪");
            }
            
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            for (int i = 0; i < statusEffectID.Length; i++)
            {
                user.stats.immunity[statusEffectID[i]]--;
            }
            this.user = null;
        }
    }
}
