using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Shot, Sub }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { ColdArm, Divinity, FireArm }
public enum MainWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Recovery }

public class Weapon : SelectItem
{
    public WeaponType weaponType;
    public WeaponRating weaponRating;
    public WeaponAttribute weaponAttribute;
    public int weaponCode;
    public float damage;
    public float rate;              // 1/rate �� ���� ����
    public float delay;             // delay �� �Ŀ� ������ �����ϴ�.
    public int maxAmmo;             // ������ �ʿ� ���� ����� ������ ǥ��
    public int ammo;                // ������ �ʿ� ���� ����� ������ ǥ��
    public float reloadTime;

}
