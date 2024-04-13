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

    public override void Enter(GameObject user)
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
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];

            player.stats.decreasedMoveSpeed += 0.5f;

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // 공격에 걸리는 시간 = 공격 1회당 걸리는 시간 / 플레이어 공격속도
            // 낮을 수록 빨리 공격
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            yield return new WaitForSeconds(0.8f * attackRate);

            // 사용자 위치에 생성
            if (effect != null)
                Destroy(effect);
            effect = Instantiate(WheelWindEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            // 공격 판정 조정
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            effect.transform.localScale = new Vector3(size * player.weaponController.weaponList[player.stats.weapon].attackSize, size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
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
            hitDetection.SetHitDetection(false, -1, true, (int)(DPS / attackRate),
             defaultDamage + player.stats.attackPower * ratio,
             player.weaponController.weaponList[player.stats.weapon].knockBack,
             player.stats.criticalChance,
             player.stats.criticalDamage,
             player.weaponController.weaponList[player.stats.weapon].statusEffect);
        }
    }

    public override void Exit(GameObject user)
    {
        StartCoroutine(AttackOut());

    }

    IEnumerator AttackOut()
    {
        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            Destroy(effect);

            yield return new WaitForSeconds(0.5f * attackRate);

            player.stats.decreasedMoveSpeed -= 0.8f;

            

        }
    }
}
