using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : Skill
{
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }
    [field: SerializeField] public float size { get; private set; }
    [field: SerializeField] public float time { get; private set; }
    [field: SerializeField] public GameObject spinEffect { get; private set; }
    
    [field: SerializeField] public float maxHoldPower { get; private set; }
    [field: SerializeField] public float holdPower { get; private set; }

    Coroutine HoldCoroutine;

    public override void Enter(GameObject user)
    {
        this.user = user;
        HoldCoroutine = StartCoroutine(Hold());
    }

    IEnumerator Hold()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;
            while (holdPower < maxHoldPower)
            {
                yield return new WaitForSeconds(0.1f);
                holdPower += 0.05f;
            } 
        }
    }

    public override void Exit(GameObject user)
    {
        StopCoroutine(HoldCoroutine);
        Attack();
    }

    void Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];

            player.stats.decreasedMoveSpeed -= 0.5f;

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            // 공격에 걸리는 시간 = 공격 1회당 걸리는 시간 / 플레이어 공격속도
            // 낮을 수록 빨리
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            // 사용자 위치에 생성
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            effect.transform.parent = user.transform;

            // 공격 판정 조정
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 크기 조정
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);

            /*
            투사체 = false
            관통력 = -1
            다단히트 = false
            초당 타격 횟수 = -1 
            피해량 = (기본 피해량 + 무기 피해량) * 플레이어 공격력
            넉백 = 무기 넉백
            치확 = 플레이어 치확
            치뎀 = 플레이어 치뎀
            디버프 = 없음
            */
            hitDetection.SetHitDetection(false, -1, false, -1,
             defalutDamage + player.stats.attackPower * ratio * holdPower,
             player.weaponController.weaponList[player.stats.weapon].knockBack * 10 * holdPower, 
             player.stats.criticalChance, 
             player.stats.criticalDamage,
             player.weaponController.weaponList[player.stats.weapon].statusEffect);

            // rate 동안 유지
            Destroy(effect, time * attackRate);
        }
    }

    
}
