using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� �⺻ ����
// ����� �нú� ����
// ������ �⺻ ����
public class Weapon : SelectItem
{
    [field: SerializeField] public WeaponInstance weaponInstance { get; protected set; }

    protected void Awake()
    {
        itemInstance = weaponInstance;
        itemInstance.Init();
    }

    protected void OnValidate()
    {
        itemInstance = weaponInstance;
        itemInstance.Init();
    }
}
