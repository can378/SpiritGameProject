using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            MeleeWeapon meleeWeapon = player.stats.mainWeapon.GetComponent<MeleeWeapon>();

            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime + player.stats.skillCoolTime * skillDefalutCoolTime;

            // 공속 = 플레이어 공속 * 무기 공속
            float attackRate = player.stats.attackSpeed * meleeWeapon.attackSpeed;

            // 선딜
            yield return new WaitForSeconds(preDelay / attackRate);

            // 사용자 위치에 생성
            GameObject instant = Instantiate(spinEffect, user.transform.position, user.transform.rotation);

            // 공격 판정 조정
            HitDetection hitDetection = instant.GetComponent<HitDetection>();

            // 크기 조정
            instant.transform.localScale = new Vector3(size * meleeWeapon.weaponSize, size * meleeWeapon.weaponSize, 0);

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            속성 = 무기 속성
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHitDetection(false, -1, false, -1,
             meleeWeapon.attackAttribute, 
             (meleeWeapon.damage + damage) * player.stats.power, 
             meleeWeapon.knockBack, 
             player.stats.critical, 
             player.stats.criticalDamage,
             meleeWeapon.deBuff);

            // rate 동안 유지
            Destroy(instant, rate / attackRate);

            // 후딜
            // 없애도 될 듯
            yield return new WaitForSeconds(postDelay / attackRate);
        }
    }

    public override void Exit(GameObject user)
    {
        
    }
}
