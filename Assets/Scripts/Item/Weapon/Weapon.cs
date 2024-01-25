using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Shot, Sub }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { ColdArm, Divinity, FireArm }
public enum MainWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Recovery }

public class Weapon : SelectItem
{

    [field: SerializeField] public WeaponType weaponType {get; private set;}
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }
    [field: SerializeField] public int weaponCode { get; private set; }
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float rate { get; private set; }              // 1/rate �� ���� ����
    [field: SerializeField] public float delay { get; private set; }             // delay �� �Ŀ� ������ �����ϴ�.
    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    public void Reload()
    {
        ammo = maxAmmo;
    }

    public void ConsumeAmmo()
    {
        ammo--;
    }

}
