using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType {Swing, Stab, Specialness }
public enum WeaponSize {Big, Middle, Small}
public enum MeleeWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Recovery }

public class MeleeWeapon : Weapon
{
    [field: SerializeField] public AttackType attackType {get; private set;}
    [field: SerializeField] public WeaponSize weaponSize { get; private set; }
    [field: SerializeField] public MeleeWeaponOption meleeWeaponOption { get; set;}
}
