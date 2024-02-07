using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponController : MonoBehaviour
{
    // 얻은 보조무기 정보
    PlayerStatus status;
    public SubWeapon subWeapon;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // 무기를 획득
    public void EquipSubWeapon(SubWeapon gainSubWeapon)
    {
        subWeapon = gainSubWeapon;
        subWeapon.gameObject.SetActive(false);
    }

    // 무기 해제
    public void UnEquipWeapon()
    {
        subWeapon.gameObject.SetActive(true);
        subWeapon = null;
    }

    // 사용
    public void Use()
    {
        if(subWeapon.subWeaponType == SubWeaponType.Guard)
        {
            status.isGuard = true;
        }
        else if (subWeapon.subWeaponType == SubWeaponType.Parry)
        {
            status.isParry = true;
        }
        else if (subWeapon.subWeaponType == SubWeaponType.Teleport)
        {
            // 플레이어 위치 변경
        }

    }

    void GuardOut()
    {

    }

    void ParryOut()
    {
        
    }

}
