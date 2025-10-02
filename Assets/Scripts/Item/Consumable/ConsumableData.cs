using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPP �ۼ�Ʈ
/// HP ����
/// HPMax �ִ� ü��
/// </summary>
public enum ConsumableType { HPP, HP, HPMax }


[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    [field: SerializeField] public ConsumableType consumableType { get; private set; }
    [field: SerializeField] public float Value { get; private set; }


}
