using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : SelectItem
{
    [field: SerializeReference] public EquipmentInstance equipmentInstance { get; protected set; } = new EquipmentInstance();

    protected virtual void Awake()
    {
        itemInstance = equipmentInstance;
        itemInstance.Init();
    }

    protected virtual void OnValidate()
    {
        itemInstance = equipmentInstance;
        itemInstance.Init();
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
