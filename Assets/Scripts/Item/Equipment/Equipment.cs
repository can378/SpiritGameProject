using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : SelectItem
{
    [field: SerializeField] public EquipmentInstance equipmentInstance { get; protected set; }

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
        // �����Ϳ� �ִ� �нú긦 ���� ��Ų��.
        for (int i = 0; i < equipmentData.passives.Count; ++i)
        {
            user.ApplyPassive(equipmentData.passives[i]);
        }
    }
    public virtual void UnEquip(Player user)
    {
        // �����Ϳ� �ִ� �нú긦 ������Ų��.
        for (int i = 0; i < equipmentData.passives.Count; ++i)
        {
            user.RemovePassive(equipmentData.passives[i]);
        }
    }
    */
}
