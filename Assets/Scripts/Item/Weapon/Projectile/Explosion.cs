using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }
    [field: SerializeField] public float damage { get; private set; }

    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    public void setExplosion(float damage, WeaponAttribute weaponAttribute)
    {
        this.damage = damage;
        this.weaponAttribute = weaponAttribute;
    }
}
