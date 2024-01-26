using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // ���� ���� ����
    Weapon weapon;
    PolygonCollider2D meleeArea;
    Transform shotPos;
    GameObject bullet;

    // ��� ���� �����
    [SerializeField] GameObject[] meleeWeaponList;
    [SerializeField] GameObject[] shotWeaponList;
    [SerializeField] GameObject[] throwWeaponList;

    //�� ���� ���� ����
    public GameObject SwordPos;

    void Awake()
    {
        
    }

    // ���⸦ ȹ��
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
            bullet = shotWeapon.bullet;
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
        yield return new WaitForSeconds(0.1f / weapon.rate);
        SwordPos.transform.rotation= Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.8f / weapon.rate);
        meleeArea.enabled = false;

    }

    IEnumerator Shot()
    {
        Debug.Log("Shot");

        yield return new WaitForSeconds(0.4f / weapon.rate);
        GameObject instantBullet = Instantiate(bullet, shotPos.position, shotPos.rotation);
        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().SetBullet(weapon.damage);
        //bulletRigid.velocity = shotPos.up * 25;
        bulletRigid.velocity = Player.instance.mouseDir * 25;
        Destroy(instantBullet, 2f);
        yield return null;
    }
}
