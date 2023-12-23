using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { None, Swing, Stab, Shot }

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public int weaponCode;
    public float damage;
    public float rate;              // 1/rate 초 동안 공격
    public float delay;             // delay 초 후에 공격이 가능하다.
    public int maxAmmo;             // 재장전 필요 없는 무기는 음수로 표기
    public int ammo;                // 재장전 필요 없는 무기는 음수로 표기
    public float reloadTime;

}
