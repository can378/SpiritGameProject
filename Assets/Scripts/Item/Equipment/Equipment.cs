using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : SelectItem
{
    [field: SerializeField] public EquipmentData equipmentData { get; protected set; }

    protected virtual void Awake()
    {
        itemData = equipmentData;
    }

    protected virtual void OnValidate()
    {
        itemData = equipmentData;
    }

    /*
    public virtual void Equip(Player user)
    {
        // 데이터에 있는 패시브를 적용 시킨다.
        for (int i = 0; i < equipmentData.passives.Count; ++i)
        {
            user.ApplyPassive(equipmentData.passives[i]);
        }
    }
    public virtual void UnEquip(Player user)
    {
        // 데이터에 있는 패시브를 해제시킨다.
        for (int i = 0; i < equipmentData.passives.Count; ++i)
        {
            user.RemovePassive(equipmentData.passives[i]);
        }
    }
    */
}
