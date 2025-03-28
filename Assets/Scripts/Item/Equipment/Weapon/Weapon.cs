using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 종류 늘릴시 MELEE과 RANGE 조정할 것
public enum WEAPON_TYPE {
    NONE = -1,
    // 근거리 
    SWING, STAB, CRACK, MELEE,
    // 원거리
    GUN, BOW, THROW, CANNON, RANGE,
    END}

public class Weapon : Equipment
{
    [field: SerializeField] public WEAPON_TYPE weaponType {get; private set;}       //근거리 : 0 - 베기, 1 - 찌르기, 2 - 휘두르기
                                                                                    //원거리 : 10 - 총, 11 - 활, 12 - 던지기
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // 다단히트 여부
    [field: SerializeField] public int DPS { get; private set; }                    // 초당 타격 횟수 필요 없을 시 음수
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float attackSize { get; private set; }       // 무기, 투사체 크기

    [field: SerializeField] public float knockBack { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }             // 선딜레이
    [field: SerializeField] public float rate { get; private set; }                 // 공격 시간
    [field: SerializeField] public float postDelay { get; private set; }            // 대기 시간
    public float SPA { get { return preDelay + rate + postDelay; } }                // 1회 공격에 걸리는 시간

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public int ammo { get; private set; }                // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile {get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // 투사체 속도
    [field: SerializeField] public float projectileTime { get; private set; }       // 투사체 유지 시간
    [field: SerializeField] public int penetrations { get; private set; }           // 투사체 관통 횟수

    [field: SerializeField] public SE_TYPE statusEffect {get; private set;}


    public override void Equip(Player user)
    {
        this.user = user;
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.AddValue += attackPower;
        if(statusEffect != SE_TYPE.NONE)
            user.AddEnchant_SE(statusEffect);
    }

    public override void UnEquip(Player user)
    {
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.AddValue -= attackPower;
        this.user = null;
        if (statusEffect != SE_TYPE.NONE)
            user.RemoveEnchant_SE(statusEffect);
    }

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
