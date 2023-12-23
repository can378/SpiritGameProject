using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 얻은 무기 정보
    Weapon weapon;
    PolygonCollider2D meleeArea;
    Transform shotPos;


    // 사용 가능 무기들
    public GameObject[] weaponList;
    public GameObject bullet;

    void Awake()
    {
    }

    // 무기를 획득
    public void EquipWeapon(Weapon gainWeapon)
    {
        weapon = gainWeapon;
        weaponList[weapon.weaponCode].SetActive(true);
        if (weapon.weaponType == WeaponType.Swing)
        {
            meleeArea = weaponList[weapon.weaponCode].GetComponent<PolygonCollider2D>();
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            shotPos = weaponList[weapon.weaponCode].GetComponent<Transform>();
        }
    }

    public void UnEquipWeapon()
    {
        weaponList[weapon.weaponCode].SetActive(false);
        if (weapon.weaponType == WeaponType.Swing)
        {
            meleeArea = null;
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            shotPos = null;
        }
        weapon = null;
    }

    public void Use()
    {
        if (weapon.weaponType == WeaponType.Swing)
        {
            StartCoroutine("Swing");
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            weapon.ammo -= 1;

            StartCoroutine("Shot");
        }

    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f / weapon.rate);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f / weapon.rate);
        meleeArea.enabled = false;

    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.4f / weapon.rate);
        GameObject instantBullet = Instantiate(bullet, shotPos.position, shotPos.rotation);
        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().damage = weapon.damage;
        bulletRigid.velocity = shotPos.up * 25;
        Destroy(instantBullet, 2f);
        yield return null;

    }
}
