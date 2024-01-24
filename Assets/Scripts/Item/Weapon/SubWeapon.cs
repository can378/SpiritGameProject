using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubWeaponOption { None }
public enum SubWeaponType {Shield, Amulet, Fan}

public class SubWeapon : Weapon
{
    SubWeaponOption subWeaponOption;
    SubWeaponType subWeaponType;
}
