using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReloadType { None, Reload}
public enum BulletType { Bullet, Arrow, Explosion, Specialness }

public class ShotWeapon : Weapon
{
    public ReloadType reloadType;
    public BulletType bulletType;
    public MainWeaponOption mainWeaponOption;
    public GameObject bullet;
}
