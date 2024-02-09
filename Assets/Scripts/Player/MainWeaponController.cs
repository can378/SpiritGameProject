using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponController : MonoBehaviour
{
    PlayerStatus status;

    // 얻은 무기 정보
    public MainWeapon mainWeapon;

    // 공격 정보
    [SerializeField] GameObject HitDetectionGameObject;
    [SerializeField] GameObject projectileGameObject;

    // 사용 가능 무기들
    [SerializeField] GameObject[] meleeWeaponList;

    void Awake()
    {
        status = GetComponent<PlayerStatus>();
    }

    // 무기를 획득
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
            // 플레이어 애니메이션 실행
            StartCoroutine("Swing");
        }
        else if (mainWeapon.weaponType == MainWeaponType.Shot)
        {
            // 플레이어 애니메이션 실행
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

    // 아이템 발사
    // 아이템을 투사체로 날림
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

    // 아이템 투척
    // 클릭한 위치에 아이템 생성
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
    

    // 이펙트 생성
    // 클릭한 방향으로 이펙트 생성
    IEnumerator Swing()
    {
        Debug.Log("Swing");

        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;

        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // 무기 이펙트 크기 설정
        MeleeWeapon meleeWeapon = mainWeapon.GetComponent<MeleeWeapon>();
        HitDetectionGameObject.transform.localScale = new Vector3(meleeWeapon.weaponSize, meleeWeapon.weaponSize, 1);

        // 이펙트 수치 설정
        HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(mainWeapon.weaponAttribute, mainWeapon.damage * DataManager.instance.userData.playerPower, mainWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage);

        // 무기 방향 
        HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);

        // 무기 이펙트 실행
        HitDetectionGameObject.SetActive(true);

        yield return new WaitForSeconds(mainWeapon.rate / attackSpeed);

        HitDetectionGameObject.SetActive(false);

    }

    // 무기 투사체 발사
    // 투사체를 클릭한 방향으로 발사
    IEnumerator Shot()
    {
        Debug.Log("Shot");
        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // 무기 투사체 적용
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        GameObject instantProjectile = Instantiate(projectileGameObject, transform.position, transform.rotation);

        //투사체 설정
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection projectile = instantProjectile.GetComponent<HitDetection>();


        //bulletRigid.velocity = shotPos.up * 25;
        // 투사체 설정
        projectile.SetHitDetection(shotWeapon.weaponAttribute, shotWeapon.damage * DataManager.instance.userData.playerPower, shotWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage); //기본 설정
        instantProjectile.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);  // 방향 설정
        instantProjectile.transform.localScale = new Vector3(shotWeapon.projectileSize, shotWeapon.projectileSize, 1);  // 크기 설정
        bulletRigid.velocity = Player.instance.mouseDir * 10 * shotWeapon.projectileSpeed;  // 속도 설정
        Destroy(instantProjectile, shotWeapon.projectileTime);  //사거리 설정

        yield return new WaitForSeconds(mainWeapon.postDelay / attackSpeed);

        yield return null;
    }

    // 범위 공격
    // 클릭한 위치에 공격 판정 생성
    IEnumerator Throw(Vector3 clickPos)
    {
        Debug.Log("Throw");
        float attackSpeed = mainWeapon.attackSpeed + DataManager.instance.userData.playerAttackSpeed;
        yield return new WaitForSeconds(mainWeapon.preDelay / attackSpeed);

        // 무기 폭발체 적용
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        GameObject instantExplosive = Instantiate(projectileGameObject, clickPos, transform.rotation);
        Explosive explosive = instantExplosive.GetComponent<Explosive>();

        // 범위 공격 설정
        explosive.SetExplosive(shotWeapon.weaponAttribute, shotWeapon.damage * DataManager.instance.userData.playerPower, shotWeapon.knockBack, DataManager.instance.userData.playerCritical, DataManager.instance.userData.playerCriticalDamage, shotWeapon.projectileSize, shotWeapon.projectileTime);
        // 이 시간 후 폭발
        Destroy(instantExplosive, shotWeapon.projectileSpeed);  

        yield return new WaitForSeconds(mainWeapon.postDelay / attackSpeed);

        yield return null;
    }
}
