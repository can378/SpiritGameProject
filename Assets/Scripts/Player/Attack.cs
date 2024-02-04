using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // 얻은 무기 정보
    MainWeapon mainWeapon;
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
    public void EquipWeapon(MainWeapon gainWeapon)
    {
        mainWeapon = gainWeapon;

        if (mainWeapon.weaponType == WeaponType.Melee)
        {
            weaponGameObject = meleeWeaponList[mainWeapon.weaponCode];
        }
        else if (mainWeapon.weaponType == WeaponType.Shot)
        {
            weaponGameObject = shotWeaponList[mainWeapon.weaponCode];

            ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
            projectileGameObject = shotWeapon.projectile;
        }
    }

    public void UnEquipWeapon()
    {
        if (mainWeapon.weaponType == WeaponType.Melee)
        {

        }
        else if (mainWeapon.weaponType == WeaponType.Shot)
        {
            projectileGameObject = null;
        }
        weaponGameObject.SetActive(false);
        weaponGameObject = null;
        mainWeapon = null;
    }

    public void Use()
    {
        if (mainWeapon.weaponType == WeaponType.Melee)
        {
            StartCoroutine("Swing");
        }
        else if (mainWeapon.weaponType == WeaponType.Shot)
        {
            mainWeapon.ConsumeAmmo();
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

        yield return new WaitForSeconds(mainWeapon.preDelay / mainWeapon.attackSpeed);

        HitDetection hitDetection = weaponGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(mainWeapon.weaponAttribute, mainWeapon.damage, mainWeapon.knockBack , 0, 0 );
        weaponGameObject.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        weaponGameObject.SetActive(true);

        yield return new WaitForSeconds(mainWeapon.rate / mainWeapon.attackSpeed);

        weaponGameObject.SetActive(false);

    }

    IEnumerator Shot()
    {
        Debug.Log("Shot");

        yield return new WaitForSeconds(mainWeapon.preDelay / mainWeapon.attackSpeed);

        GameObject instantProjectile = Instantiate(projectileGameObject, weaponGameObject.transform.position, weaponGameObject.transform.rotation);
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection projectile = instantProjectile.GetComponent<HitDetection>();
        
        //bulletRigid.velocity = shotPos.up * 25;
        // 투사체 설정
        projectile.SetHitDetection(shotWeapon.weaponAttribute,shotWeapon.damage, shotWeapon.knockBack, 0, 0); //기본 설정
        instantProjectile.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);  // 방향 설정
        instantProjectile.transform.localScale = new Vector3(shotWeapon.projectileSize, shotWeapon.projectileSize, 1);  // 크기 설정
        bulletRigid.velocity = Player.instance.mouseDir * 25 * shotWeapon.projectileSpeed;  // 속도 설정
        Destroy(instantProjectile, shotWeapon.projectileTime);  //사거리 설정

        yield return new WaitForSeconds(mainWeapon.postDelay / mainWeapon.attackSpeed);

        yield return null;
    }
}
