using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���
public class SubWeaponController : MonoBehaviour
{
    // ���� �������� ����
    PlayerStatus status;
    public SubWeapon subWeapon;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // ���⸦ ȹ��
    public void EquipSubWeapon(SubWeapon gainSubWeapon)
    {
        subWeapon = gainSubWeapon;
        subWeapon.gameObject.SetActive(false);
    }

    // ���� ����
    public void UnEquipWeapon()
    {
        subWeapon.gameObject.transform.position = gameObject.transform.position;
        subWeapon.gameObject.SetActive(true);
        subWeapon = null;
    }

    // ���
    public void Use()
    {
        if(subWeapon.subWeaponType == SubWeaponType.Guard)
        {
            
        }
        else if (subWeapon.subWeaponType == SubWeaponType.Parry)
        {
            
        }
        else if (subWeapon.subWeaponType == SubWeaponType.Teleport)
        {
            // �÷��̾� ��ġ ����
        }

    }

}
