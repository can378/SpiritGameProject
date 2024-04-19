using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWind : Skill
{
    [field: SerializeField] int defaultDamage;// 회당 기본 피해량
    [field: SerializeField] float ratio;
    [field: SerializeField] int DPS;       // 초당 공격 속도
    [field: SerializeField] float size;     // 이펙트 크기
    [field: SerializeField] GameObject WheelWindEffect;     //휠윈드 prefep 이펙트
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
            
            // 먼저 속도 감소
            player.stats.decreasedMoveSpeed += 0.5f;

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // 공격에 걸리는 시간 = 공격 1회당 걸리는 시간 / 플레이어 공격속도
            // 낮을 수록 빨리 공격
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            // 시간이 조금 지난 후 회전 시작
            yield return new WaitForSeconds(preDelay * attackRate);

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


    public override void Exit()
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

            // 회전 멈춤
            Destroy(effect);

            // 조금 시간이 지난 후 속도 감소 해제
            yield return new WaitForSeconds(postDelay * attackRate);

            player.stats.decreasedMoveSpeed -= 0.5f;

            

        }
    }
}
