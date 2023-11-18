using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { None,Swing, Stab, Shot }
public enum WeaponOption { None,Critical, Damage}

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public int weaponCode;
    public float damage;
    public float rate;
}
