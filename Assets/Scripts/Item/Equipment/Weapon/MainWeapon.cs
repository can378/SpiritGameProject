using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeaponType { Melee, Shot }

public class MainWeapon : Equipments
{
    [field: SerializeField] public MainWeaponType weaponType {get; private set;}
    [field: SerializeField] public int attackType { get; private set; }             //근거리 : 0 - 휘두르기, 1 - 찌르기, 2 - 기타
                                                                                    //원거리 : 0 - 총, 1 - 활, 2 - 던지기, 3 - 범위 공격
    // 0 : 무속성, 1 : 참격, 2 : 타격, 3 : 관통, 4 : 화염, 5 : 냉기, 6 : 전기, 7 : 독, 8 : 역장, 9 : 신성, 10 : 어둠 
    [field: SerializeField] public List<int> attackAttribute { get; private set; }        
    
    [field: SerializeField] public bool isMultiHit { get; private set; }            // 다단히트 여부
    [field: SerializeField] public int DPS { get; private set; }                    // 초당 타격 횟수 필요 없을 시 음수
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // 공격속도

    [field: SerializeField] public float preDelay { get; private set; }             // 선딜레이
    [field: SerializeField] public float rate { get; private set; }                 // 공격 시간
    [field: SerializeField] public float postDelay { get; private set; }            // 대기 시간

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public int ammo { get; private set; }                // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject deBuff { get; private set; }
    

    public override void Equip()
    {
        switch(equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power += 0.5f;
                break;
            case 2:
                Player.instance.stats.attackSpeed += 0.5f;
                break;
            case 3:
                Player.instance.stats.skillPower += 1f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime -= 0.5f;
                break;
            case 5:
                Player.instance.stats.critical += 0.5f;
                break;
            case 6:
                Player.instance.stats.criticalDamage += 0.5f;
                break;
            default:
                break;
        }
    }

    public override void RandomOption()
    {
        Equip();
        equipmentsOption = Random.Range(1,6);
        UnEquip();
    }

    public override void UnEquip()
    {
        switch (equipmentsOption)
        {
            case 0:
                break;
            case 1:
                Player.instance.stats.power -= 0.5f;
                break;
            case 2:
                Player.instance.stats.attackSpeed -= 0.5f;
                break;
            case 3:
                Player.instance.stats.skillPower -= 1f;
                break;
            case 4:
                Player.instance.stats.skillCoolTime += 0.5f;
                break;
            case 5:
                Player.instance.stats.critical -= 0.5f;
                break;
            case 6:
                Player.instance.stats.criticalDamage -= 0.5f;
                break;
            default:
                break;
        }
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
