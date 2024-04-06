using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWind : Skill
{
    [field: SerializeField] public int defaultDamage { get; private set; }     // 회당 기본 피해량
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public int DPS { get; private set; }        // 초당 공격 속도
    [field: SerializeField] public float size { get; private set; }     // 이펙트 크기
    [field: SerializeField] public GameObject WheelWindEffect { get; private set; }     //휠윈드 prefep 이펙트
    GameObject effect;      // 이펙트

    public override void Use(GameObject user)
    {
        this.user = user;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log("WheelWind");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // 쿨타임 적용
            skillCoolTime = player.stats.skillCoolTime * skillDefalutCoolTime;

            // 공속 = 플레이어 공속 * 무기 공속
            float attackSpeed = player.stats.attackSpeed * player.stats.weapon.attackSpeed;

            // 선딜
            yield return new WaitForSeconds(preDelay / attackSpeed);

            // 사용자 위치에 생성
            if (effect != null)
                Destroy(effect);
            effect = Instantiate(WheelWindEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;


            // 공격 판정 조정
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            effect.transform.localScale = new Vector3(size * player.stats.weapon.attackSize, size * player.stats.weapon.attackSize, 0);
            /*
            투사체 = false
            관통력 = -1
            다단히트 = true
            초당 타격 횟수 = DPS * attackRate 
            속성 = 무기 속성
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHitDetection(false, -1, true, (int)(DPS * attackSpeed),
             defaultDamage + player.stats.attackPower * ratio,
             player.stats.weapon.knockBack,
             player.stats.criticalChance,
             player.stats.criticalDamage,
             player.stats.weapon.statusEffect);
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

            float attackRate = (1 + player.stats.attackSpeed) * player.stats.weapon.attackSpeed;

            yield return new WaitForSeconds(postDelay / attackRate);

            Destroy(effect);

        }
    }
}
