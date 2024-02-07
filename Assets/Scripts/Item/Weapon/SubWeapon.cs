using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubWeaponType { Guard, Parry, Teleport }

public class SubWeapon : SelectItem
{
    [field: SerializeField] public int SubweaponCode { get; private set; }

    [field: SerializeField] public SubWeaponType subWeaponType { get; private set; }
    [field: SerializeField] public WeaponRating weaponRating { get; private set; }

    [field: SerializeField] public float preDelay { get; private set; }     // ��������
    [field: SerializeField] public float rate { get; private set; }         // ��� �ð�

    [field: SerializeField] public float coolTime { get; private set; }     // ���� ������ ��� �ð�

    [field: SerializeField] public float ratio { get; private set; }     // ��ġ ���� : ���� ������      �ݰ� : �� ���� �ð�     �ڷ���Ʈ : �Ÿ�
}
