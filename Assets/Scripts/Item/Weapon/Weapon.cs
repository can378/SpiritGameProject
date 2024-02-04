using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Shot, Sub }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { Slash, Blow, Pierce, Fire, Cold, Electricity, Divinity, Darkness }
public enum WeaponOption { None, Attack, Speed, Critical, CriticalDamage, Drain }

public class MainWeapon : SelectItem
{
    [field: SerializeField] public WeaponType weaponType {get; private set;}
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }
    [field: SerializeField] public WeaponOption weaponOption { get; private set; }
    [field: SerializeField] public int weaponCode { get; private set; }
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }
    [field: SerializeField] public float rate { get; private set; }              
    [field: SerializeField] public float postDelay { get; private set; }

    [field: SerializeField] public float knockBack { get; private set; }

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public int ammo { get; private set; }                // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    public void Reload()
    {
        ammo = maxAmmo;
    }

    public void ConsumeAmmo()
    {
        ammo--;
    }

}
