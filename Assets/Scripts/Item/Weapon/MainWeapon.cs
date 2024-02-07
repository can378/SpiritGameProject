using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeaponType { Melee, Shot }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { Slash, Blow, Pierce, Fire, Cold, Electricity, Divinity, Darkness }
public enum MainWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Drain }

public class MainWeapon : SelectItem
{
    [field: SerializeField] public int mainWeaponCode { get; private set; }

    [field: SerializeField] public MainWeaponType weaponType {get; private set;}
    [field: SerializeField] public int attackType { get; private set; }             //근거리 : 0 - 휘두르기, 1 - 찌르기, 2 - 기타
                                                                                    //원거리 : 0 - 총, 1 - 활, 2 - 던지기, 3 - 범위 공격
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }
    [field: SerializeField] public MainWeaponOption mainweaponOption { get; private set; }
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // 공격속도

    [field: SerializeField] public float preDelay { get; private set; }             // 선딜레이
    [field: SerializeField] public float rate { get; private set; }                 // 공격 시간
    [field: SerializeField] public float postDelay { get; private set; }            // 대기 시간

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public int ammo { get; private set; }                // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    public void Reload()
    {
        if(maxAmmo < 0)
            return;
        ammo = maxAmmo;
    }

    public void ConsumeAmmo()
    {
        if (maxAmmo < 0)
            return;
        ammo--;
    }

}
