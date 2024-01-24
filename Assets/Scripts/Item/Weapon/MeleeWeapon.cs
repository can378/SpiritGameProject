using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType {Swing, Stab, Specialness }
public enum WeaponSize {Big, Middle, Small}

public class MeleeWeapon : Weapon
{
    public AttackType attackType;
    public WeaponSize weaponSize;
    public MainWeaponOption mainWeaponOption;
}
