using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats playerStats;

    // 공격 정보
    public int enchant;
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // 근거리 공격 이펙트 
    [SerializeField] GameObject[] meleeEffectList;

    // 무기 정보
    [field:SerializeField] public Weapon[] weaponList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        playerStats = GetComponent<PlayerStats>();
    }

    // 무기를 획득
    public bool EquipWeapon(int weaponID)
    {
        // 무기 소유
        playerStats.weapon = weaponID;
        // 장비 능력치 적용
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

        // 장비 UI 적용
        MapUIManager.instance.UpdateWeaponUI();
        return true;
    }

    public void UnEquipWeapon()
    {
        HitDetectionGameObject = null;
        projectileGameObject = null;

        // 현재 위치에 장비를 놓는다.
        Instantiate(GameData.instance.weaponList[playerStats.weapon],gameObject.transform.position,gameObject.transform.localRotation);

        // 무기 능력치 해제
        weaponList[playerStats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
        weaponList[playerStats.weapon].gameObject.SetActive(false);
        
        // 무기 해제
        playerStats.weapon = 0;
        MapUIManager.instance.UpdateWeaponUI();
    }

    public void Use(Vector3 clickPos)
    {
        weaponList[playerStats.weapon].ConsumeAmmo();
        if (weaponList[playerStats.weapon].weaponType < 10 )
        {
            // 플레이어 애니메이션 실행
            StartCoroutine("Swing");
        }
        else if ( 10 <= weaponList[playerStats.weapon].weaponType)
        {
            // 플레이어 애니메이션 실행
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

    // 아이템 발사
    // 아이템을 투사체로 날림
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

    // 아이템 투척
    // 클릭한 위치에 아이템 생성
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

    // 이펙트 생성
    // 클릭한 방향으로 이펙트 생성
    IEnumerator Swing()
    {
        //Debug.Log("Swing");

        float attackAngle = status.mouseAngle;
        //Vector2 attackDir = status.mouseDir;

        //선딜
        yield return new WaitForSeconds(weaponList[playerStats.weapon].preDelay / playerStats.attackSpeed);

        // 무기 이펙트 크기 설정
        HitDetectionGameObject.transform.localScale = new Vector3(weaponList[playerStats.weapon].attackSize, weaponList[playerStats.weapon].attackSize, 1);
        HitDetectionGameObject.GetComponentInChildren<Enchant>().index = enchant;

        // 이펙트 수치 설정
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(false,-1, weaponList[playerStats.weapon].isMultiHit, weaponList[playerStats.weapon].DPS, playerStats.attackPower, weaponList[playerStats.weapon].knockBack, playerStats.criticalChance, playerStats.criticalDamage,weaponList[playerStats.weapon].statusEffect);
        hitDetection.user = this.gameObject;

        // 무기 방향 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);

        // 무기 이펙트 실행
        HitDetectionGameObject.SetActive(true);

        // 공격 시간
        yield return new WaitForSeconds(weaponList[playerStats.weapon].rate / playerStats.attackSpeed);

        // 무기 이펙트 해제
        HitDetectionGameObject.SetActive(false);
        HitDetectionGameObject.GetComponentInChildren<Enchant>().index = 0;

    }

    // 무기 투사체 발사
    // 투사체를 클릭한 방향으로 발사
    IEnumerator Shot()
    {
        //Debug.Log("Shot");

        float attackAngle = status.mouseAngle;
        Vector2 attackDir = status.mouseDir;

        // 선딜
        yield return new WaitForSeconds(weaponList[playerStats.weapon].preDelay / playerStats.attackSpeed);

        // 무기 투사체 적용
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);
        instantProjectile.GetComponent<Enchant>().index = enchant;

        //투사체 설정
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

        //bulletRigid.velocity = shotPos.up * 25;
        // 투사체 설정
        hitDetection.SetHitDetection(true, weaponList[playerStats.weapon].penetrations, weaponList[playerStats.weapon].isMultiHit, weaponList[playerStats.weapon].DPS, playerStats.attackPower, weaponList[playerStats.weapon].knockBack, playerStats.criticalChance, playerStats.criticalDamage, weaponList[playerStats.weapon].statusEffect);
        hitDetection.user = this.gameObject;
        instantProjectile.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);  // 방향 설정
        instantProjectile.transform.localScale = new Vector3(weaponList[playerStats.weapon].attackSize, weaponList[playerStats.weapon].attackSize,1);
        bulletRigid.velocity = attackDir * 10 * weaponList[playerStats.weapon].projectileSpeed;  // 속도 설정
        Destroy(instantProjectile, weaponList[playerStats.weapon].projectileTime);  //사거리 설정
    }

    // 범위 공격
    // 클릭한 위치에 공격 판정 생성
    /*
    IEnumerator Throw(Vector3 clickPos)
    {
        Debug.Log("Throw");
        float attackSpeed = weapon.attackSpeed * Player.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(weapon.preDelay / attackSpeed);

        // 무기 폭발체 적용
        ShotWeapon shotWeapon = weapon.GetComponent<ShotWeapon>();
        GameObject instantExplosive = Instantiate(projectileGameObject, clickPos, transform.rotation);
        Explosive explosive = instantExplosive.GetComponent<Explosive>();

        // 범위 공격 설정
        explosive.SetExplosive(shotWeapon.weaponAttribute, shotWeapon.damage * Player.instance.userData.playerPower, shotWeapon.knockBack, Player.instance.userData.playerCritical, Player.instance.userData.playerCriticalDamage, shotWeapon.projectileSize, shotWeapon.projectileTime);
        // 이 시간 후 폭발
        Destroy(instantExplosive, shotWeapon.projectileSpeed);  

        yield return new WaitForSeconds(weapon.postDelay / attackSpeed);

        yield return null;
    }
    */
}
