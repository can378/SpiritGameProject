using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Shot, Sub }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { ColdArm, Divinity, FireArm }
public enum MainWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Recovery }

public class Weapon : SelectItem
{
    public WeaponType weaponType;
    public WeaponRating weaponRating;
    public WeaponAttribute weaponAttribute;
    public int weaponCode;
    public float damage;
    public float rate;              // 1/rate 초 동안 공격
    public float delay;             // delay 초 후에 공격이 가능하다.
    public int maxAmmo;             // 재장전 필요 없는 무기는 음수로 표기
    public int ammo;                // 재장전 필요 없는 무기는 음수로 표기
    public float reloadTime;

}
