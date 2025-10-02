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
            // 고정 수치 회복
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
            // 비율로 회복
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
            // 최대 체력 증가
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
