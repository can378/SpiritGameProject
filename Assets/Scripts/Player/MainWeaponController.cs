using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponController : MonoBehaviour
{
    PlayerStatus status;

    // ���� ���� ����
    public MainWeapon mainWeapon;

    // ���� ����
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // ��� ���� �����
    [SerializeField] GameObject[] meleeWeaponList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // ���⸦ ȹ��
    public void EquipWeapon(MainWeapon gainWeapon)
    {
        mainWeapon = gainWeapon;

        if (mainWeapon.weaponType == MainWeaponType.Melee)
        {
            HitDetectionGameObject = meleeWeaponList[mainWeapon.attackType];
        }
        else if (mainWeapon.weaponType == MainWeaponType.Shot)
        {
            ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
            projectileGameObject = shotWeapon.projectile;
        }
        mainWeapon.gameObject.SetActive(false);
    }

    public void UnEquipWeapon()
    {
        if (mainWeapon.weaponType == MainWeaponType.Melee)
        {
            HitDetectionGameObject = null;
        }
        else if (mainWeapon.weaponType == MainWeaponType.Shot)
        {
            projectileGameObject = null;
        }
        mainWeapon.gameObject.transform.position = gameObject.transform.position;
        mainWeapon.gameObject.SetActive(true);
        mainWeapon = null;
    }

    public void Use(Vector3 clickPos)
    {
        mainWeapon.ConsumeAmmo();
        if (mainWeapon.weaponType == MainWeaponType.Melee)
        {
            // �÷��̾� �ִϸ��̼� ����
            StartCoroutine("Swing");
        }
        else if (mainWeapon.weaponType == MainWeaponType.Shot)
        {
            // �÷��̾� �ִϸ��̼� ����
            if(mainWeapon.attackType == 3)
                StartCoroutine("Throw", clickPos);
            else 
                StartCoroutine("Shot");
        }

    }

    public void UseItem(GameObject explosive, Vector3 clickPos)
    {
        //StartCoroutine("ThrowItem", clickPos);
        StartCoroutine("ShotItem", explosive);

    }

    // ������ �߻�
    // �������� ����ü�� ����
    IEnumerator ShotItem(GameObject explosive)
    {
        Debug.Log("ShotItem");

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

    // ������ ��ô
    // Ŭ���� ��ġ�� ������ ����
    IEnumerator ThrowItem(GameObject explosive)
    {
        Debug.Log("ThrowItem");

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
    

    // ����Ʈ ����
    // Ŭ���� �������� ����Ʈ ����
    IEnumerator Swing()
    {
        Debug.Log("Swing");

        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;

        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // ���� ����Ʈ ũ�� ����
        MeleeWeapon meleeWeapon = mainWeapon.GetComponent<MeleeWeapon>();
        HitDetectionGameObject.transform.localScale = new Vector3(meleeWeapon.weaponSize, meleeWeapon.weaponSize, 1);

        // ����Ʈ ��ġ ����
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(mainWeapon.weaponAttribute, mainWeapon.damage * DataManager.instance.userData.playerPower, mainWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage);

        // ���� ���� 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);

        // ���� ����Ʈ ����
        HitDetectionGameObject.SetActive(true);

        yield return new WaitForSeconds(mainWeapon.rate / attackSpeed);

        HitDetectionGameObject.SetActive(false);

    }

    // ���� ����ü �߻�
    // ����ü�� Ŭ���� �������� �߻�
    IEnumerator Shot()
    {
        Debug.Log("Shot");
        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // ���� ����ü ����
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);

        //����ü ����
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection projectile = instantProjectile.GetComponent<HitDetection>();


        //bulletRigid.velocity = shotPos.up * 25;
        // ����ü ����
        projectile.SetHitDetection(shotWeapon.weaponAttribute, shotWeapon.damage * DataManager.instance.userData.playerPower, shotWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage); //�⺻ ����
        instantProjectile.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);  // ���� ����
        instantProjectile.transform.localScale = new Vector3(shotWeapon.projectileSize, shotWeapon.projectileSize, 1);  // ũ�� ����
        bulletRigid.velocity = Player.instance.mouseDir * 10 * shotWeapon.projectileSpeed;  // �ӵ� ����
        Destroy(instantProjectile, shotWeapon.projectileTime);  //��Ÿ� ����

        yield return new WaitForSeconds(mainWeapon.postDelay / attackSpeed);

        yield return null;
    }

    // ���� ����
    // Ŭ���� ��ġ�� ���� ���� ����
    IEnumerator Throw(Vector3 clickPos)
    {
        Debug.Log("Throw");
        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // ���� ����ü ����
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        GameObject instantExplosive = Instantiate(projectileGameObject, clickPos, transform.rotation);
        Explosive explosive = instantExplosive.GetComponent<Explosive>();

        // ���� ���� ����
        explosive.SetExplosive(shotWeapon.weaponAttribute, shotWeapon.damage * DataManager.instance.userData.playerPower, shotWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage, shotWeapon.projectileSize, shotWeapon.projectileTime);
        // �� �ð� �� ����
        Destroy(instantExplosive, shotWeapon.projectileSpeed);  

        yield return new WaitForSeconds(mainWeapon.postDelay / attackSpeed);

        yield return null;
    }
}
