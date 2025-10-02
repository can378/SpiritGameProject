using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 종류 늘릴시 MELEE과 RANGE 조정할 것
public enum WEAPON_TYPE
{
    NONE = -1,
    // 근거리 
    SWORD, SHORT_SWORD, LONG_SWORD, SCYTHE, HAMMER, GREAT_HAMMER, SPEAR, MELEE,
    // 원거리
    BOW, CROSS_BOW, RIFLE, HAND_GUN, THROW, RANGE,
    END
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Item/Weapon")]
public class WeaponData : ItemData
{
    [field: SerializeField] public WEAPON_TYPE weaponType { get; private set; }

    [field: SerializeField] public bool isMultiHit { get; private set; }            // 다단히트 여부
    [field: SerializeField] public int DPS { get; private set; }                    // 초당 타격 횟수 필요 없을 시 음수
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float attackSize { get; private set; }       // 무기, 투사체 크기

    [field: SerializeField] public float knockBack { get; private set; }

    //[field: SerializeField] public float preDelay { get; private set; }             // 선딜레이
    //[field: SerializeField] public float rate { get; private set; }                 // 공격 시간
    //[field: SerializeField] public float postDelay { get; private set; }            // 대기 시간
    //public float SPA { get { return preDelay + rate + postDelay; } }                // 1회 공격에 걸리는 시간

    [field: SerializeField] public int maxAmmo { get; private set; }             // 재장전 필요 없는 무기는 음수로 표기
    [field: SerializeField] public float reloadTime { get; private set; }

    [field: SerializeField] public GameObject projectile { get; private set; }
    [field: SerializeField] public float projectileSpeed { get; private set; }      // 투사체 속도
    [field: SerializeField] public float projectileTime { get; private set; }       // 투사체 유지 시간
    [field: SerializeField] public int maxPenetrations { get; private set; }           // 투사체 최대 관통 횟수

    [field: SerializeField] public List<PassiveData> passives { get; private set; }


    public int SwingEffectType
    {
        get
        {
            if ((int)WEAPON_TYPE.NONE < (int)weaponType && (int)weaponType < (int)WEAPON_TYPE.HAMMER)
                return 0;
            else if ((int)WEAPON_TYPE.SCYTHE < (int)weaponType && (int)weaponType < (int)WEAPON_TYPE.SPEAR)
                return 2;
            else if (weaponType == WEAPON_TYPE.SPEAR)
                return 1;
            else
                return -1;
        }
    }

    public int ShotPosType
    {
        get
        {
            switch (weaponType)
            {
                case WEAPON_TYPE.BOW:
                    return 3;
                case WEAPON_TYPE.CROSS_BOW:
                    return 4;
                case WEAPON_TYPE.RIFLE:
                    return 5;
                case WEAPON_TYPE.HAND_GUN:
                    return 6;
                case WEAPON_TYPE.THROW:
                    return 7;
                default:
                    return -1;
            }
        }
    }

    public string TypeToKorean()
    {
        {
            switch (weaponType)
            {
                case WEAPON_TYPE.SWORD:
                    return "검";
                case WEAPON_TYPE.SHORT_SWORD:
                    return "단검";
                case WEAPON_TYPE.LONG_SWORD:
                    return "대검";
                case WEAPON_TYPE.SCYTHE:
                    return "낫";
                case WEAPON_TYPE.HAMMER:
                    return "둔기";
                case WEAPON_TYPE.GREAT_HAMMER:
                    return "대형 둔기";
                case WEAPON_TYPE.SPEAR:
                    return "창";
                case WEAPON_TYPE.BOW:
                    return "활";
                case WEAPON_TYPE.CROSS_BOW:
                    return "쇠뇌";
                case WEAPON_TYPE.HAND_GUN:
                    return "총";
                case WEAPON_TYPE.RIFLE:
                    return "장총";
                case WEAPON_TYPE.THROW:
                    return "투척";
                default:
                    return "???";
            }
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        string des = description;

        if (passives.Count > 0)
            des += "\n패시브";
        for (int i = 0; i < passives.Count; ++i)
            {
                des += "\n" + passives[i].Update_Description(_Stats);
            }
        return des;
    }

}
