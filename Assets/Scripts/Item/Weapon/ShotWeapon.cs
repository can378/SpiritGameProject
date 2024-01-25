using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReloadType { None, Reload}
public enum BulletType { Bullet, Arrow, Explosion, Specialness }

public class ShotWeapon : Weapon
{
    [field: SerializeField] public ReloadType reloadType { get; private set; }
    [field: SerializeField] public BulletType bulletType { get; private set; }
    [field: SerializeField] public MainWeaponOption mainWeaponOption { get; set; }
    [field: SerializeField] public GameObject bullet { get; private set; }
}
