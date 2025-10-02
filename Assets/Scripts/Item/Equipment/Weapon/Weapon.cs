using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� �⺻ ����
// ����� �нú� ����
// ������ �⺻ ����
public class Weapon : SelectItem
{
    [field: SerializeField] public WeaponData weaponData { get; protected set; }

    protected void Awake()
    {
        itemData = weaponData;
    }
}
