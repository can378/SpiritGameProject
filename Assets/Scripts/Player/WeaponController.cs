using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    // ���� ����
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // �ٰŸ� ���� ����Ʈ 
    [SerializeField] GameObject[] meleeEffectList;

    // ���� ����
    [field:SerializeField] public Weapon[] weaponList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // ���⸦ ȹ��
    public bool EquipWeapon(int weaponID)
    {
        // ���� ����
        stats.weapon = weaponID;
        // ��� �ɷ�ġ ����
        weaponList[stats.weapon].gameObject.SetActive(true);
        weaponList[stats.weapon].Equip(this.gameObject.GetComponent<Player>());

        if (weaponList[stats.weapon].weaponType < 10)
        {
            HitDetectionGameObject = meleeEffectList[weaponList[stats.weapon].weaponType];
        }
        else if (10 <= weaponList[stats.weapon].weaponType)
        {
            projectileGameObject = weaponList[stats.weapon].projectile;
        }

        // ��� UI ����
        MapUIManager.instance.UpdateWeaponUI();
        return true;
    }

    public void UnEquipWeapon()
    {
        HitDetectionGameObject = null;
        projectileGameObject = null;

        // ���� ��ġ�� ��� ���´�.
        Instantiate(GameData.instance.weaponList[stats.weapon],gameObject.transform.position,gameObject.transform.localRotation);

        // ���� �ɷ�ġ ����
        weaponList[stats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
        weaponList[stats.weapon].gameObject.SetActive(false);
        
        // ���� ����
        stats.weapon = 0;
        MapUIManager.instance.UpdateWeaponUI();
    }

    public void Use(Vector3 clickPos)
    {
        weaponList[stats.weapon].ConsumeAmmo();
        if (weaponList[stats.weapon].weaponType < 10 )
        {
            // �÷��̾� �ִϸ��̼� ����
            StartCoroutine("Swing");
        }
        else if ( 10 <= weaponList[stats.weapon].weaponType)
        {
            // �÷��̾� �ִϸ��̼� ����
            if(weaponList[stats.weapon].weaponType == 13)
                StartCoroutine("Throw", clickPos);
            else 
                StartCoroutine("Shot");
        }

        //Debug.Log(stats.weapon.attackSpeed);
        //Debug.Log(stats.attackSpeed);

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
        //Debug.Log("ShotItem");

        yield return new WaitForSeconds(0.1f);

        explosive.tag = "Weapon";
        explosive.SetActive(true);
        explosive.transform.position = transform.position;
        explosive.GetComponent<Rigidbody2D>().velocity = status.mouseDir * 25;

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
    /*
    IEnumerator ThrowItem(GameObject explosive)
    {
        Debug.Log("ThrowItem");

        yield return new WaitForSeconds(0.1f);

        explosive.tag = "Weapon";
        explosive.SetActive(true);
        explosive.transform.position = transform.position;
        explosive.GetComponent<Rigidbody2D>().velocity = status.mouseDir * 25;

        float originalRadius = explosive.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < originalRadius * 1.5; i++)
        {
            explosive.GetComponent<CircleCollider2D>().radius++;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(explosive);
        yield return null;

    }
    */    

    // ����Ʈ ����
    // Ŭ���� �������� ����Ʈ ����
    IEnumerator Swing()
    {
        //Debug.Log("Swing");

        float attackDir = status.mouseAngle;

        //����
        yield return new WaitForSeconds(weaponList[stats.weapon].preDelay / stats.attackSpeed);

        // ���� ����Ʈ ũ�� ����
        HitDetectionGameObject.transform.localScale = new Vector3(weaponList[stats.weapon].attackSize, weaponList[stats.weapon].attackSize, 1);

        // ����Ʈ ��ġ ����
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(false,-1, weaponList[stats.weapon].isMultiHit, weaponList[stats.weapon].DPS, stats.attackPower, weaponList[stats.weapon].knockBack, stats.criticalChance, stats.criticalDamage,weaponList[stats.weapon].statusEffect);

        // ���� ���� 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(attackDir - 90, Vector3.forward);

        // ���� ����Ʈ ����
        HitDetectionGameObject.SetActive(true);

        // ���� �ð�
        yield return new WaitForSeconds(weaponList[stats.weapon].rate / stats.attackSpeed);

        // ���� ����Ʈ ����
        HitDetectionGameObject.SetActive(false);

    }

    // ���� ����ü �߻�
    // ����ü�� Ŭ���� �������� �߻�
    IEnumerator Shot()
    {
        //Debug.Log("Shot");

        float attackDir = status.mouseAngle;

        // ����
        yield return new WaitForSeconds(weaponList[stats.weapon].preDelay / stats.attackSpeed);

        // ���� ����ü ����
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);

        //����ü ����
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

        //bulletRigid.velocity = shotPos.up * 25;
        // ����ü ����
        hitDetection.SetHitDetection(true, weaponList[stats.weapon].penetrations, weaponList[stats.weapon].isMultiHit, weaponList[stats.weapon].DPS, stats.attackPower, weaponList[stats.weapon].knockBack, stats.criticalChance, stats.criticalDamage, weaponList[stats.weapon].statusEffect);
        instantProjectile.transform.rotation = Quaternion.AngleAxis(attackDir - 90, Vector3.forward);  // ���� ����
        instantProjectile.transform.localScale = new Vector3(weaponList[stats.weapon].attackSize, weaponList[stats.weapon].attackSize,1);
        bulletRigid.velocity = status.mouseDir * 10 * weaponList[stats.weapon].projectileSpeed;  // �ӵ� ����
        Destroy(instantProjectile, weaponList[stats.weapon].projectileTime);  //��Ÿ� ����

        // �ĵ�
        // ���ֵ� �ɵ�
        yield return new WaitForSeconds(weaponList[stats.weapon].postDelay / stats.attackSpeed);

        yield return null;
    }

    // ���� ����
    // Ŭ���� ��ġ�� ���� ���� ����
    /*
    IEnumerator Throw(Vector3 clickPos)
    {
        Debug.Log("Throw");
        float attackSpeed = weapon.attackSpeed * Player.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(weapon.preDelay / attackSpeed);

        // ���� ����ü ����
        ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();
        GameObject instantExplosive = Instantiate(projectileGameObject, clickPos, transform.rotation);
        Explosive explosive = instantExplosive.GetComponent<Explosive>();

        // ���� ���� ����
        explosive.SetExplosive(shotWeapon.weaponAttribute, shotWeapon.damage * Player.instance.userData.playerPower, shotWeapon.knockBack, Player.instance.userData.playerCritical, Player.instance.userData.playerCriticalDamage, shotWeapon.projectileSize, shotWeapon.projectileTime);
        // �� �ð� �� ����
        Destroy(instantExplosive, shotWeapon.projectileSpeed);  

        yield return new WaitForSeconds(weapon.postDelay / attackSpeed);

        yield return null;
    }
    */
}
