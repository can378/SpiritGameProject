using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class WeaponInstance : ItemInstance
{
    public WeaponData weaponData; // SO ����

    public override void Init()
    {
        itemData = weaponData;
    }

}
