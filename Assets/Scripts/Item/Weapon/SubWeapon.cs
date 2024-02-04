using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubWeaponOption { None }
public enum SubWeaponType {Shield, Amulet, Fan}

public class SubWeapon : MainWeapon
{
    [field: SerializeField] public SubWeaponOption subWeaponOption { get; set;}
    [field: SerializeField] public SubWeaponType subWeaponType { get; private set; }
}
