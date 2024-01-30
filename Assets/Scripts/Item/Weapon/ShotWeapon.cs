using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReloadType { None, Reload }
public enum ShotWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Recovery }

public class ShotWeapon : Weapon
{
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float time { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    
    [field: SerializeField] public ReloadType reloadType { get; private set; }
    [field: SerializeField] public ShotWeaponOption shotWeaponOption { get; set; }
    [field: SerializeField] public GameObject projectile { get; private set; }
}
