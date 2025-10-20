using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class WeaponInstance : ItemInstance
{
    public WeaponData weaponData; // SO ÂüÁ¶

    public override void Init()
    {
        itemData = weaponData;
    }

}
