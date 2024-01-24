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
    public GameObject[] meleeWeaponList;
    public GameObject[] shotWeaponList;
    //public GameObject bullet;

    //검 공격 방향 설정
    public GameObject SwordPos;

    void Awake()
    {
        
    }

    // 무기를 획득
    public void EquipWeapon(Weapon gainWeapon)
    {
        weapon = gainWeapon;

        if (weapon.weaponType == WeaponType.Melee)
        {
            meleeWeaponList[weapon.weaponCode].SetActive(true);
            meleeArea = meleeWeaponList[weapon.weaponCode].GetComponent<PolygonCollider2D>();
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            shotWeaponList[weapon.weaponCode].SetActive(true);
            shotPos = shotWeaponList[weapon.weaponCode].GetComponent<Transform>();
        }
    }

    public void UnEquipWeapon()
    {
        if (weapon.weaponType == WeaponType.Melee)
        {
            meleeWeaponList[weapon.weaponCode].SetActive(false);
            meleeArea = null;
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            shotWeaponList[weapon.weaponCode].SetActive(false);
            shotPos = null;
        }
        weapon = null;
    }

    public void Use()
    {
        if (weapon.weaponType == WeaponType.Melee)
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
        Debug.Log("Swing");
        yield return new WaitForSeconds(0.1f / weapon.rate);
        SwordPos.transform.rotation= Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f / weapon.rate);
        meleeArea.enabled = false;

    }

    IEnumerator Shot()
    {
        Debug.Log("Shot");

        // 탄 종류 가져오기
        // 비효율적이면 바꾸기
        ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();

        yield return new WaitForSeconds(0.4f / weapon.rate);
        GameObject instantBullet = Instantiate(shotWeapon.bullet, shotPos.position, shotPos.rotation);
        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        shotWeapon.bullet.GetComponent<Bullet>().damage = weapon.damage;
        //bulletRigid.velocity = shotPos.up * 25;
        bulletRigid.velocity = Player.instance.mouseDir * 25;
        Destroy(instantBullet, 2f);
        yield return null;
    }
}
