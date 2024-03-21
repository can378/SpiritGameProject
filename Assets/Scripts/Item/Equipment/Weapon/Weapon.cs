using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    [field: SerializeField] public int weaponType {get; private set;}               //근거리 : 0 - 베기, 1 - 찌르기, 2 - 휘두르기
                                                                                    //원거리 : 10 - 총, 11 - 활, 12 - 던지기, 13 - 범위 공격
    // 0 : 무속성, 1 : 참격, 2 : 타격, 3 : 관통, 4 : 화염, 5 : 냉기, 6 : 전기, 7 : 독, 8 : 역장, 9 : 신성, 10 : 어둠 
    [field: SerializeField] public List<int> attackAttribute { get; private set; }        
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // 다단히트 여부
    [field: SerializeField] public int DPS { get; private set; }                    // 초당 타격 횟수 필요 없을 시 음수
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // 공격속도

    [field: SerializeField] public float preDelay { get; private set; }             // 선딜레이
    [field: SerializeField] public float rate { get; private set; }                 // 공격 시간
    [field: SerializeField] public float postDelay { get; private set; }            // 대기 시간

    [field: SerializeField] public GameObject deBuff { get; private set; }

    [field: SerializeField] public float attackSize { get; private set; }       // 무기, 투사체 크기

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public int ammo { get; private set; }                // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile {get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // 투사체 속도
    [field: SerializeField] public float projectileTime { get; private set; }       // 투사체 유지 시간
    [field: SerializeField] public int penetrations { get; private set; }           // 투사체 관통 횟수


    public override void Equip()
    {
        Player.instance.stats.addAttackPower = attackPower;
    }

    public override void UnEquip()
    {
        Player.instance.stats.addAttackPower -= attackPower;
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
