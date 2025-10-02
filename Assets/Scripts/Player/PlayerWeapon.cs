using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // 패시브로 수치 변경 시 적용
    [field: SerializeField] public WeaponData weaponData { get; private set; }
    [field: SerializeField] public bool isMultiHit { private get; set; }                // 무기의 다단히트로 변경
    [field: SerializeField] public Stat DPS { private get; set; } = new Stat(0, 30, 1);                     // 무기의 추가 DPS 수치
    [field: SerializeField] public Stat attackSize { private get; set; } = new Stat(0, 5f, 0.1f);       // 무기, 투사체 추가 크기
    [field: SerializeField] public Stat knockBack { private get; set; } = new Stat(0, 1000, 0);       // 무기의 추가 넉백 
    [field: SerializeField] public Stat maxAmmo { private get; set; } = new Stat(0, 100, 1);                 // 무기의 추가 장탄 수
    [field: SerializeField] public Stat reloadTime { private get; set; } = new Stat(0, float.MaxValue, 0.1f);
    [field: SerializeField] public Stat projectileSpeed { private get; set; } = new Stat(0, 10, 0.1f);     // 투사체 추가 속도
    [field: SerializeField] public Stat projectileTime { private get; set; } = new Stat(0, 10, 0.1f);     // 투사체 추가 유지 시간
    [field: SerializeField] public Stat maxPenetrations { private get; set; } = new Stat(0, float.MaxValue, 0);          // 투사체 추가 최대 관통 횟수
    [field: SerializeField] public int ammo { get; private set; }                   // 재장전 필요 없는 무기는 음수로 표기

    public void SetWeaponData(Weapon _Weapon)
    {
        if (_Weapon == null)
        {
            weaponData = null;
            return;
        }
        weaponData = _Weapon.weaponData;
        DPS.SetDefaultValue(weaponData.DPS);
        attackSize.SetDefaultValue(weaponData.attackSize);
        knockBack.SetDefaultValue(weaponData.knockBack);
        maxAmmo.SetDefaultValue(weaponData.maxAmmo);
        reloadTime.SetDefaultValue(weaponData.reloadTime);
        projectileSpeed.SetDefaultValue(weaponData.projectileSpeed);
        projectileTime.SetDefaultValue(weaponData.projectileTime);
        maxPenetrations.SetDefaultValue(weaponData.maxPenetrations);
        ammo = weaponData.maxAmmo;
    }

    public bool GetMultiHit()
    {
        return isMultiHit || weaponData.isMultiHit;
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
        if (weaponData.maxAmmo < 0)
            return;
        ammo = weaponData.maxAmmo;
    }

    public void ConsumeAmmo()
    {
        if (weaponData.maxAmmo < 0)
            return;
        ammo--;
    }

    public void Equip(Player user)
    {
        // 데이터에 있는 패시브를 적용 시킨다.
        for (int i = 0; i < weaponData.passives.Count; ++i)
        {
            user.ApplyPassive(weaponData.passives[i]);
        }
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.SetDefaultValue(weaponData.attackPower);
    }

    public void UnEquip(Player user)
    {
        for (int i = 0; i < weaponData.passives.Count; ++i)
        {
            user.RemovePassive(weaponData.passives[i]);
        }
        Stats stats = user.GetComponent<Stats>();
        stats.AttackPower.SetDefaultValue(0);
    }
}
