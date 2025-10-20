using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // �нú�� ��ġ ���� �� ����
    [field: SerializeField] public WeaponInstance weaponInstance { get; private set; }      // itemData�� null �̶�� ���⸦ �����ϰ� ���� ����
    [field: SerializeField] public bool isMultiHit { private get; set; }                // ������ �ٴ���Ʈ�� ����
    [field: SerializeField] public Stat DPS { private get; set; } = new Stat(0, 30, 1);                     // ������ �߰� DPS ��ġ
    [field: SerializeField] public Stat attackSize { private get; set; } = new Stat(0, 5f, 0.1f);       // ����, ����ü �߰� ũ��
    [field: SerializeField] public Stat knockBack { private get; set; } = new Stat(0, 1000, 0);       // ������ �߰� �˹� 
    [field: SerializeField] public Stat maxAmmo { private get; set; } = new Stat(0, 100, 1);                 // ������ �߰� ��ź ��
    [field: SerializeField] public Stat reloadTime { private get; set; } = new Stat(0, float.MaxValue, 0.1f);
    [field: SerializeField] public Stat projectileSpeed { private get; set; } = new Stat(0, 10, 0.1f);     // ����ü �߰� �ӵ�
    [field: SerializeField] public Stat projectileTime { private get; set; } = new Stat(0, 10, 0.1f);     // ����ü �߰� ���� �ð�
    [field: SerializeField] public Stat maxPenetrations { private get; set; } = new Stat(0, float.MaxValue, 0);          // ����ü �߰� �ִ� ���� Ƚ��
    [field: SerializeField] public int ammo { get; private set; }                   // ������ �ʿ� ���� ����� ������ ǥ��

    public void SetWeaponData(Weapon _Weapon)
    {
        if (_Weapon == null)
        {
            weaponInstance.weaponData = null;
            return;
        }
        weaponInstance = _Weapon.weaponInstance;
        DPS.SetDefaultValue(weaponInstance.weaponData.DPS);
        attackSize.SetDefaultValue(weaponInstance.weaponData.attackSize);
        knockBack.SetDefaultValue(weaponInstance.weaponData.knockBack);
        maxAmmo.SetDefaultValue(weaponInstance.weaponData.maxAmmo);
        reloadTime.SetDefaultValue(weaponInstance.weaponData.reloadTime);
        projectileSpeed.SetDefaultValue(weaponInstance.weaponData.projectileSpeed);
        projectileTime.SetDefaultValue(weaponInstance.weaponData.projectileTime);
        maxPenetrations.SetDefaultValue(weaponInstance.weaponData.maxPenetrations);
        ammo = weaponInstance.weaponData.maxAmmo;
    }

    public bool GetMultiHit()
    {
        return isMultiHit || weaponInstance.weaponData.isMultiHit;
    }

    public int GetDPS()
    {
        float i = DPS.Value;
        return Mathf.CeilToInt(DPS.Value);
    }

    public float GetAttackSize()
    {
        return attackSize.Value;
    }

    public float GetKnockBack()
    {
        return knockBack.Value;
    }

    public int GetMaxAmmo()
    {
        return Mathf.CeilToInt(maxAmmo.Value);
    }

    public float GetReloadTime()
    {
        return reloadTime.Value;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed.Value;
    }

    public float GetProjectileTime()
    {
        return projectileTime.Value;
    }

    public int GetMaxPenetrations()
    {
        return Mathf.CeilToInt(maxPenetrations.Value);
    }

    public void Reload()
    {
        if (weaponInstance.weaponData.maxAmmo < 0)
            return;
        ammo = weaponInstance.weaponData.maxAmmo;
    }

    public void ConsumeAmmo()
    {
        if (weaponInstance.weaponData.maxAmmo < 0)
            return;
        ammo--;
    }

    public void Equip(Player user)
    {
        // �����Ϳ� �ִ� �нú긦 ���� ��Ų��.
        for (int i = 0; i < weaponInstance.weaponData.passives.Count; ++i)
        {
            user.ApplyPassive(weaponInstance.weaponData.passives[i]);
        }
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.SetDefaultValue(weaponInstance.weaponData.attackPower);
    }

    public void UnEquip(Player user)
    {
        for (int i = 0; i < weaponInstance.weaponData.passives.Count; ++i)
        {
            user.RemovePassive(weaponInstance.weaponData.passives[i]);
        }
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.SetDefaultValue(0);
    }
}
