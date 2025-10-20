using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템의 기본 정보
// 장비의 패시브 정보
// 무기의 기본 정보
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
