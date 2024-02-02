using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }    
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }                       //Bullet, Arrow, Rotor                       

    /// <summary>
    /// 
    /// </summary>
    /// <param name="weaponAttribute"></param>
    /// <param name="damage"></param>
    /// <param name="knockBack"></param>
    public void SetHitDetection(WeaponAttribute weaponAttribute, float damage, float knockBack)
    {
        this.damage = damage;
        this.weaponAttribute = weaponAttribute;
        this.knockBack = knockBack;
    }
}
