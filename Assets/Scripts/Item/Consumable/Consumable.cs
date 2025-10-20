using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : SelectItem
{
    [field: SerializeField] public ConsumableInstance consumableInstance { get; protected set; }

    protected void Awake()
    {
        itemInstance = consumableInstance;
        itemInstance.Init();
    }

    protected void OnValidate()
    {
        itemInstance = consumableInstance;
        itemInstance.Init();
    }

    public void UseItem(ObjectBasic user)
    {
        switch (consumableInstance.consumableData.consumableType)
        {
            // ���� ��ġ ȸ��
            case ConsumableType.HP:
                {
                    if (user.stats.HP + consumableInstance.consumableData.Value > user.stats.HPMax.Value)
                    {
                        user.stats.HP = user.stats.HPMax.Value;
                    }
                    else
                    {
                        user.stats.HP += consumableInstance.consumableData.Value;
                    }
                }
                break;
            // ������ ȸ��
            case ConsumableType.HPP:
                {
                    if (user.stats.HP + user.stats.HPMax.Value * consumableInstance.consumableData.Value > user.stats.HPMax.Value)
                    {
                        user.stats.HP = user.stats.HPMax.Value;
                    }
                    else
                    {
                        user.stats.HP += user.stats.HPMax.Value * consumableInstance.consumableData.Value;
                    }
                }
                break;
            // �ִ� ü�� ����
            case ConsumableType.HPMax:
                {
                    user.stats.HPMax.AddValue += consumableInstance.consumableData.Value;
                    user.stats.HP += consumableInstance.consumableData.Value;
                }
                break;
            default:
                break;
        }


    }
}
