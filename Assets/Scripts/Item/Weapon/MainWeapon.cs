using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeaponType { Melee, Shot }
public enum WeaponRating { Temporary, General, Rare, Unique }
public enum WeaponAttribute { Slash, Blow, Pierce, Fire, Cold, Electricity, Divinity, Darkness }
public enum MainWeaponOption { None, Attack, Speed, Critical, CriticalDamage, Drain }

public class MainWeapon : SelectItem
{
    [field: SerializeField] public int mainWeaponCode { get; private set; }

    [field: SerializeField] public MainWeaponType weaponType {get; private set;}
    [field: SerializeField] public int attackType { get; private set; }             //�ٰŸ� : 0 - �ֵθ���, 1 - ���, 2 - ��Ÿ
                                                                                    //���Ÿ� : 0 - ��, 1 - Ȱ, 2 - ������, 3 - ���� ����
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }
    [field: SerializeField] public WeaponAttribute weaponAttribute { get; private set; }
    [field: SerializeField] public MainWeaponOption mainweaponOption { get; private set; }
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float knockBack { get; private set; }
    [field: SerializeField] public float attackSpeed { get; private set; }          // ���ݼӵ�

    [field: SerializeField] public float preDelay { get; private set; }             // ��������
    [field: SerializeField] public float rate { get; private set; }                 // ���� �ð�
    [field: SerializeField] public float postDelay { get; private set; }            // ��� �ð�

    [field: SerializeField] public int maxAmmo { get; private set; }             // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public int ammo { get; private set; }                // ������ �ʿ� ���� ����� ������ ǥ��
    [field: SerializeField] public float reloadTime { get; private set; }

    public void Reload()
    {
        if(maxAmmo < 0)
            return;
        ammo = maxAmmo;
    }

    public void ConsumeAmmo()
    {
        if (maxAmmo < 0)
            return;
        ammo--;
    }

}
