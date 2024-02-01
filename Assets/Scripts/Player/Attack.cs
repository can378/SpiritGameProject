using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 얻은 무기 정보
    Weapon weapon;
    GameObject weaponGameObject;

    // 공격 정보
    GameObject projectileGameObject;

    // 사용 가능 무기들
    [SerializeField] GameObject[] meleeWeaponList;
    [SerializeField] GameObject[] shotWeaponList;
    [SerializeField] GameObject[] throwWeaponList;

    void Awake()
    {
        
    }

    // 무기를 획득
    public void EquipWeapon(Weapon gainWeapon)
    {
        weapon = gainWeapon;

        if (weapon.weaponType == WeaponType.Melee)
        {
            weaponGameObject = meleeWeaponList[weapon.weaponCode];
        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            weaponGameObject = shotWeaponList[weapon.weaponCode];

            ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();
            projectileGameObject = shotWeapon.projectile;
        }
    }

    public void UnEquipWeapon()
    {
        if (weapon.weaponType == WeaponType.Melee)
        {

        }
        else if (weapon.weaponType == WeaponType.Shot)
        {
            projectileGameObject = null;
        }
        weaponGameObject.SetActive(false);
        weaponGameObject = null;
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
            weapon.ConsumeAmmo();
            StartCoroutine("Shot");
        }

    }


    
    public IEnumerator ThrowWeapon(GameObject explosive) 
    { 
        
        yield return new WaitForSeconds(0.1f);

        explosive.tag = "Weapon";
        explosive.SetActive(true);
        explosive.transform.position = transform.position;
        explosive.GetComponent<Rigidbody2D>().velocity = Player.instance.mouseDir * 25;

        float originalRadius = explosive.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < originalRadius * 1.5; i++)
        { 
            explosive.GetComponent<CircleCollider2D>().radius++;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(explosive);
        yield return null;

    }


    IEnumerator Swing()
    {
        Debug.Log("Swing");

        yield return new WaitForSeconds(weapon.preDelay / weapon.attackSpeed);

        weaponGameObject.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        weaponGameObject.SetActive(true);

        yield return new WaitForSeconds(weapon.rate / weapon.attackSpeed);

        weaponGameObject.SetActive(false);

    }

    IEnumerator Shot()
    {
        Debug.Log("Shot");

        yield return new WaitForSeconds(weapon.preDelay / weapon.attackSpeed);

        GameObject instantProjectile = Instantiate(projectileGameObject, weaponGameObject.transform.position, weaponGameObject.transform.rotation);
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();

        //bulletRigid.velocity = shotPos.up * 25;
        projectile.SetProjectile(shotWeapon.damage, shotWeapon.speed, shotWeapon.size, shotWeapon.weaponAttribute);
        instantProjectile.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        Destroy(instantProjectile, shotWeapon.time);

        bulletRigid.velocity = Player.instance.mouseDir * 25 * shotWeapon.speed;

        yield return new WaitForSeconds(weapon.postDelay / weapon.attackSpeed);

        yield return null;
    }
}
