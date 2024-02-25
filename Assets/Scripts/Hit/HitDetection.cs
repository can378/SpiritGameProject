using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻ ���� ����
public class HitDetection : MonoBehaviour
{
    // 0 : ���Ӽ�, 1 : ����, 2 : Ÿ��, 3 : ����, 4 : ȭ��, 5 : �ñ�, 6 : ����, 7 : ����, 8 : �ż�, 9 : ���
    [field: SerializeField] public int attackAttribute { get; private set; } 
    //[field: SerializeField] public WeaponAttribute addedWeaponAttribute { get; private set; } //�ΰ� �Ӽ�?
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
