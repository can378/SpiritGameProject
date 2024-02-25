using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
public class HitDetection : MonoBehaviour
{
    // 0 : 무속성, 1 : 참격, 2 : 타격, 3 : 관통, 4 : 화염, 5 : 냉기, 6 : 전기, 7 : 역장, 8 : 신성, 9 : 어둠
    [field: SerializeField] public int attackAttribute { get; private set; } 
    //[field: SerializeField] public WeaponAttribute addedWeaponAttribute { get; private set; } //부가 속성?
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float critical { get; private set; }
    [field: SerializeField] public float criticalDamage { get; private set; }
    //[field: SerializeField] public int drain { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="weaponAttribute"></param>
    /// <param name="damage"></param>
    /// <param name="knockBack"></param>
    /// <param name="critical"></param>
    /// <param name="criticalDamage"></param>
    public void SetHitDetection(int attackAttribute, float damage, float knockBack, float critical, float criticalDamage)
    {
        this.damage = damage;
        this.attackAttribute = attackAttribute;
        this.knockBack = knockBack;
        this.critical = critical;
        this.criticalDamage = criticalDamage;
    }
}
