using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : SelectItem
{
    [field: SerializeField] public ConsumableData consumableData { get; protected set; }

    protected virtual void Awake()
    {
        itemData = consumableData;
    }

    public void UseItem(ObjectBasic user)
    {
        switch (consumableData.consumableType)
        {
            // ���� ��ġ ȸ��
            case ConsumableType.HP:
                {
                    if (user.stats.HP + consumableData.Value > user.stats.HPMax.Value)
                    {
                        user.stats.HP = user.stats.HPMax.Value;
                    }
                    else
                    {
                        user.stats.HP += consumableData.Value;
                    }
                }
                break;
            // ������ ȸ��
            case ConsumableType.HPP:
                {
                    if (user.stats.HP + user.stats.HPMax.Value * consumableData.Value > user.stats.HPMax.Value)
                    {
                        user.stats.HP = user.stats.HPMax.Value;
                    }
                    else
                    {
                        user.stats.HP += user.stats.HPMax.Value * consumableData.Value;
                    }
                }
                break;
            // �ִ� ü�� ����
            case ConsumableType.HPMax:
                {
                    user.stats.HPMax.AddValue += consumableData.Value;
                    user.stats.HP += consumableData.Value;
                }
                break;
            default:
                break;
        }


    }
}
