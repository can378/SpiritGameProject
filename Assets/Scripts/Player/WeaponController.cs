using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerStatus status;
    PlayerStats stats;

    // 공격 정보
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // 근거리 공격 이펙트 
    [SerializeField] GameObject[] meleeEffectList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
    }

    // 무기를 획득
    public void EquipWeapon(Weapon gainWeapon)
    {
        // 무기 소유
        stats.weapon = gainWeapon;
        // 장비 능력치 적용
        stats.weapon.Equip();

        if (stats.weapon.weaponType < 10)
        {
            HitDetectionGameObject = meleeEffectList[stats.weapon.weaponType];
        }
        else if (10 <= stats.weapon.weaponType)
        {
            projectileGameObject = stats.weapon.projectile;
        }

        
        // 장비 안보이게 하기
        stats.weapon.gameObject.SetActive(false);
        MapUIManager.instance.UpdateWeaponUI();
    }

    public void UnEquipWeapon()
    {
        HitDetectionGameObject = null;
        projectileGameObject = null;

        // 현재 위치에 장비를 놓는다.
        stats.weapon.gameObject.transform.position = gameObject.transform.position;
        stats.weapon.gameObject.SetActive(true);

        // 무기 능력치 해제
        stats.weapon.UnEquip();

        // 무기 해제
        stats.weapon = null;
        MapUIManager.instance.UpdateWeaponUI();
    }

    public void Use(Vector3 clickPos)
    {
        stats.weapon.ConsumeAmmo();
        if (stats.weapon.weaponType < 10 )
        {
            // 플레이어 애니메이션 실행
            StartCoroutine("Swing");
        }
        else if ( 10 <= stats.weapon.weaponType)
        {
            // 플레이어 애니메이션 실행
            if(stats.weapon.weaponType == 13)
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

    // 아이템 발사
    // 아이템을 투사체로 날림
    IEnumerator ShotItem(GameObject explosive)
    {
        Debug.Log("ShotItem");

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
        Debug.Log("Swing");

        float attackSpeed = stats.weapon.attackSpeed * stats.attackSpeed;

        //선딜
        yield return new WaitForSeconds(stats.weapon.preDelay / attackSpeed);

        // 무기 이펙트 크기 설정
        HitDetectionGameObject.transform.localScale = new Vector3(stats.weapon.attackSize, stats.weapon.attackSize, 1);

        // 이펙트 수치 설정
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(false,-1, stats.weapon.isMultiHit, stats.weapon.DPS, stats.attackPower, stats.weapon.knockBack, stats.criticalChance, stats.criticalDamage,stats.weapon.statusEffect);

        // 무기 방향 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(status.mouseAngle - 90, Vector3.forward);

        // 무기 이펙트 실행
        HitDetectionGameObject.SetActive(true);

        // 공격 시간
        yield return new WaitForSeconds(stats.weapon.rate / attackSpeed);

        // 무기 이펙트 해제
        HitDetectionGameObject.SetActive(false);

    }

    // 무기 투사체 발사
    // 투사체를 클릭한 방향으로 발사
    IEnumerator Shot()
    {
        Debug.Log("Shot");
        float attackSpeed = stats.weapon.attackSpeed * stats.attackSpeed;
        // 선딜
        yield return new WaitForSeconds(stats.weapon.preDelay / attackSpeed);

        // 무기 투사체 적용
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);

        //투사체 설정
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();


        //bulletRigid.velocity = shotPos.up * 25;
        // 투사체 설정
        hitDetection.SetHitDetection(true, stats.weapon.penetrations, stats.weapon.isMultiHit, stats.weapon.DPS, stats.attackPower, stats.weapon.knockBack, stats.criticalChance, stats.criticalDamage, stats.weapon.statusEffect);
        instantProjectile.transform.rotation = Quaternion.AngleAxis(status.mouseAngle - 90, Vector3.forward);  // 방향 설정
        instantProjectile.transform.localScale = new Vector3(stats.weapon.attackSize, stats.weapon.attackSize,1);
        bulletRigid.velocity = status.mouseDir * 10 * stats.weapon.projectileSpeed;  // 속도 설정
        Destroy(instantProjectile, stats.weapon.projectileTime);  //사거리 설정

        // 후딜
        // 없애도 될듯
        yield return new WaitForSeconds(stats.weapon.postDelay / attackSpeed);

        yield return null;
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
