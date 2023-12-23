using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { None, Swing, Stab, Shot }

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    public int weaponCode;
    public float damage;
    public float rate;              // 1/rate �� ���� ����
    public float delay;             // delay �� �Ŀ� ������ �����ϴ�.
    public int maxAmmo;             // ������ �ʿ� ���� ����� ������ ǥ��
    public int ammo;                // ������ �ʿ� ���� ����� ������ ǥ��
    public float reloadTime;

}
