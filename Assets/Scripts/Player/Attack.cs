using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 얻은 무기 정보
    Weapon weapon;
    BoxCollider2D meleeArea;
    Transform shotPos;


    // 사용 가능 무기들
    public GameObject[] weaponList;
    public GameObject bullet;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 무기를 획득
    public void GainWeapon(Weapon gainWeapon)
    {
        weapon = gainWeapon;
        weaponList[weapon.weaponCode].SetActive(true);
        anim.SetFloat("rateSwing", weapon.rate);
        if (weapon.weaponType == WeaponType.Swing)
        {
            anim.SetBool("isSwing",true);
            meleeArea = weaponList[weapon.weaponCode].GetComponent<BoxCollider2D>();
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            anim.SetBool("isShot", true);
            shotPos = weaponList[weapon.weaponCode].GetComponent<Transform>();
        }
    }
    
    public void Use()
    {
        if(weapon.weaponType == WeaponType.Swing)
        {
            StartCoroutine("Swing");
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            StartCoroutine("Shot");
        }

    }

    IEnumerator Swing()
    {
        anim.SetTrigger("doSwing");
        yield return new WaitForSeconds(0.1f / weapon.rate);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f / weapon.rate);
        meleeArea.enabled = false;

    }

    IEnumerator Shot()
    {
        anim.SetTrigger("doShot");

        GameObject instantBullet = Instantiate(bullet, shotPos.position, shotPos.rotation);
        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().damage = weapon.damage;
        bulletRigid.velocity = shotPos.up * 25;
        Destroy(instantBullet, 2f);
        yield return null;

    }
}
