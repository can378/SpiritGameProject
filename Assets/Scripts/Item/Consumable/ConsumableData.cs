using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPP 퍼센트
/// HP 고정
/// HPMax 최대 체력
/// </summary>
public enum ConsumableType { HPP, HP, HPMax }


[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [field: SerializeField] public ConsumableType consumableType { get; private set; }
    [field: SerializeField] public float Value { get; private set; }


}
