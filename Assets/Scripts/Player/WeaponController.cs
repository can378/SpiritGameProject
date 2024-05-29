using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats playerStats;

    // ���� ����
    public int enchant;
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // �ٰŸ� ���� ����Ʈ 
    [SerializeField] GameObject[] meleeEffectList;

    // ���� ����
    [field:SerializeField] public Weapon[] weaponList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
    }

    // ���⸦ ȹ��
    public bool EquipWeapon(int weaponID)
    {
        // ���� ����
        playerStats.weapon = weaponID;
        // ��� �ɷ�ġ ����
        weaponList[playerStats.weapon].gameObject.SetActive(true);
        weaponList[playerStats.weapon].Equip(this.gameObject.GetComponent<Player>());

        if (weaponList[playerStats.weapon].weaponType < 10)
        {
            HitDetectionGameObject = meleeEffectList[weaponList[playerStats.weapon].weaponType];
        }
        else if (10 <= weaponList[playerStats.weapon].weaponType)
        {
            projectileGameObject = weaponList[playerStats.weapon].projectile;
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
        Instantiate(GameData.instance.weaponList[playerStats.weapon],gameObject.transform.position,gameObject.transform.localRotation);

        // ���� �ɷ�ġ ����
        weaponList[playerStats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
        weaponList[playerStats.weapon].gameObject.SetActive(false);
        
        // ���� ����
        playerStats.weapon = 0;
        MapUIManager.instance.UpdateWeaponUI();
    }

    public void Use(Vector3 clickPos)
    {
        weaponList[playerStats.weapon].ConsumeAmmo();
        if (weaponList[playerStats.weapon].weaponType < 10 )
        {
            // �÷��̾� �ִϸ��̼� ����
            StartCoroutine("Swing");
        }
        else if ( 10 <= weaponList[playerStats.weapon].weaponType)
        {
            // �÷��̾� �ִϸ��̼� ����
            if(weaponList[playerStats.weapon].weaponType == 13)
                StartCoroutine("Throw", clickPos);
            else 
                StartCoroutine("Shot");
        }

        //Debug.Log(playerStats.weapon.attackSpeed);
        //Debug.Log(playerStats.attackSpeed);

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

        explosive.tag = "PlayerAttack";
        explosive.layer = LayerMask.NameToLayer("PlayerAttack");
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

        float attackAngle = status.mouseAngle;
        //Vector2 attackDir = status.mouseDir;

        //����
        yield return new WaitForSeconds(weaponList[playerStats.weapon].preDelay / playerStats.attackSpeed);

        // ���� ����Ʈ ũ�� ����
        HitDetectionGameObject.transform.localScale = new Vector3(weaponList[playerStats.weapon].attackSize, weaponList[playerStats.weapon].attackSize, 1);
        HitDetectionGameObject.GetComponentInChildren<Enchant>().index = enchant;

        // ����Ʈ ��ġ ����
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(false,-1, weaponList[playerStats.weapon].isMultiHit, weaponList[playerStats.weapon].DPS, playerStats.attackPower, weaponList[playerStats.weapon].knockBack, playerStats.criticalChance, playerStats.criticalDamage,weaponList[playerStats.weapon].statusEffect);
        hitDetection.user = this.gameObject;

        // ���� ���� 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);

        // ���� ����Ʈ ����
        HitDetectionGameObject.SetActive(true);

        // ���� �ð�
        yield return new WaitForSeconds(weaponList[playerStats.weapon].rate / playerStats.attackSpeed);

        // ���� ����Ʈ ����
        HitDetectionGameObject.SetActive(false);
        HitDetectionGameObject.GetComponentInChildren<Enchant>().index = 0;

    }

    // ���� ����ü �߻�
    // ����ü�� Ŭ���� �������� �߻�
    IEnumerator Shot()
    {
        //Debug.Log("Shot");

        float attackAngle = status.mouseAngle;
        Vector2 attackDir = status.mouseDir;

        // ����
        yield return new WaitForSeconds(weaponList[playerStats.weapon].preDelay / playerStats.attackSpeed);

        // ���� ����ü ����
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);
        instantProjectile.GetComponent<Enchant>().index = enchant;

        //����ü ����
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

        //bulletRigid.velocity = shotPos.up * 25;
        // ����ü ����
        hitDetection.SetHitDetection(true, weaponList[playerStats.weapon].penetrations, weaponList[playerStats.weapon].isMultiHit, weaponList[playerStats.weapon].DPS, playerStats.attackPower, weaponList[playerStats.weapon].knockBack, playerStats.criticalChance, playerStats.criticalDamage, weaponList[playerStats.weapon].statusEffect);
        hitDetection.user = this.gameObject;
        instantProjectile.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);  // ���� ����
        instantProjectile.transform.localScale = new Vector3(weaponList[playerStats.weapon].attackSize, weaponList[playerStats.weapon].attackSize,1);
        bulletRigid.velocity = attackDir * 10 * weaponList[playerStats.weapon].projectileSpeed;  // �ӵ� ����
        Destroy(instantProjectile, weaponList[playerStats.weapon].projectileTime);  //��Ÿ� ����
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
