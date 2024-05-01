using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : Skill
{
    // 피해량
    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    // 최대 강화량
    [field: SerializeField] public float maxHoldPower { get; private set; }

    // 기본 크기, 이펙트 유지시간, 이펙트
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject spinEffect;
    [field: SerializeField] GameObject spinEffectSimul;

    // 강화 수치
    float holdPower;
    GameObject simul;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        StartCoroutine(Simulation());
    }

    IEnumerator Simulation()
    {
        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            player.stats.decreasedMoveSpeed += 0.5f;
            holdPower = 1f;

            simul = Instantiate(spinEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.status.isSkillHold)
            {
                holdPower += 0.05f;
                simul.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.decreasedMoveSpeed += 0.5f;

            simul = Instantiate(spinEffectSimul, user.gameObject.transform.position, Quaternion.identity);
            simul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < maxHoldPower && timer <= maxHoldTime / 2)
            {
                holdPower += 0.05f;
                simul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Exit()
    {
        StopCoroutine(Simulation());
        Attack();
    }

    void Attack()
    {
        Debug.Log("SpinAttack");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();
            Weapon weapon = player.weaponController.weaponList[player.stats.weapon];
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float attackRate = weapon.SPA / player.stats.attackSpeed;

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(simul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, holdPower * size * player.weaponController.weaponList[player.stats.weapon].attackSize, 0);
            effect.tag = "PlayerAttack";

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
            player.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(spinEffect, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime =skillDefalutCoolTime;

            Destroy(simul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
            effect.tag = "EnemyAttack";

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
             defalutDamage + enemy.stats.attackPower * ratio * holdPower,
             10 * holdPower,
             0,
             0,
             null);

            // rate 동안 유지
            enemy.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time);
        }
    }

    
}
