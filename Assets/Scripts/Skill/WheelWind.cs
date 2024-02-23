using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWind : Skill
{
    [field: SerializeField] public int damage { get; private set; }     // 회당 기본 피해량
    [field: SerializeField] public int DPS { get; private set; }        // 초당 공격 속도
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public GameObject WheelWindEffect { get; private set; }
    [field: SerializeField] public GameObject instant { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime + player.userData.skillCoolTime * skillDefalutCoolTime;

            // 공속 = 플레이어 공속 * 무기 공속
            float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

            // 선딜
            yield return new WaitForSeconds(preDelay / attackRate);

            // 사용자 위치에 생성
            instant = Instantiate(WheelWindEffect, user.transform.position, user.transform.rotation);
            instant.transform.parent = user.transform;


            // 공격 판정 조정
            MultiHitDetection hitDetection = instant.GetComponent<MultiHitDetection>();

            // 속성 = 무기 속성
            // 피해량 = (무기 + 기본 피해량) * 플레이어 공격력
            // 넉백 = 무기 넉백
            // 치확 = 플레이어 치확
            // 치뎀 = 플레이어 치뎀
            hitDetection.SetMultiHitDetection(meleeWeapon.weaponAttribute,
             (meleeWeapon.damage + damage) * player.userData.playerPower,
             meleeWeapon.knockBack,
             player.userData.playerCritical,
             player.userData.playerCriticalDamage,
             meleeWeapon.weaponSize * size,
             (int)(DPS * attackRate)
             );
        }
    }

    public override void Exit(GameObject user)
    {
        StartCoroutine("AttackOut");
        
    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.mainWeaponController.mainWeapon.GetComponent<MeleeWeapon>();

            float attackRate = player.userData.playerAttackSpeed * meleeWeapon.attackSpeed;

            // rate 동안 유지
            Destroy(instant);

            //후딜
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }
}
