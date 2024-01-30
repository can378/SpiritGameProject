using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 얻은 무기 정보
    Weapon weapon;
    PolygonCollider2D meleeArea;
    Transform shotPos;
    GameObject projectileGameObject;

    // 사용 가능 무기들
    [SerializeField] GameObject[] meleeWeaponList;
    [SerializeField] GameObject[] shotWeaponList;
    [SerializeField] GameObject[] throwWeaponList;

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
            ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();
            projectileGameObject = shotWeapon.projectile;
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
            weapon.ConsumeAmmo();
            StartCoroutine("Shot");
        }

    }


    // 던질 아이템 구현하겠습니다.
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

        SwordPos.transform.rotation= Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(weapon.rate / weapon.attackSpeed);

        meleeArea.enabled = false;

    }

    IEnumerator Shot()
    {
        Debug.Log("Shot");

        yield return new WaitForSeconds(weapon.preDelay / weapon.attackSpeed);

        GameObject instantProjectile = Instantiate(projectileGameObject, shotPos.position, shotPos.rotation);
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();

        //bulletRigid.velocity = shotPos.up * 25;
        projectile.SetProjectile(shotWeapon.damage, shotWeapon.speed, shotWeapon.size, shotWeapon.weaponAttribute);
        Destroy(instantProjectile, shotWeapon.time);

        bulletRigid.velocity = Player.instance.mouseDir * 25 * shotWeapon.speed;

        yield return null;
    }
}
