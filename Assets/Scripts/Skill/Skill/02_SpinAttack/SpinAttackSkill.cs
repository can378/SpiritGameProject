using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackSkill : Skill
{
    // 피해량
    [field: SerializeField] int defalutDamage;
    [field: SerializeField] float ratio;

    // 최대 강화량
    [field: SerializeField] float maxHoldPower;

    // 기본 크기, 이펙트 유지시간, 이펙트
    [field: SerializeField] float size;
    [field: SerializeField] float time;
    [field: SerializeField] GameObject spinPrefab;
    [field: SerializeField] GameObject spinSimulPrefab;

    // 강화 수치
    float holdPower;
    GameObject spinSimul;

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

            spinSimul = Instantiate(spinSimulPrefab, user.gameObject.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            while (holdPower < maxHoldPower && player.playerStatus.isSkillHold)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if(user.tag == "Enemy")
        {
            EnemyBasic enemy = this.user.GetComponent<EnemyBasic>();
            float timer;

            enemy.stats.decreasedMoveSpeed += 0.5f;

            spinSimul = Instantiate(spinSimulPrefab, user.gameObject.transform.position, Quaternion.identity);
            spinSimul.transform.parent = user.transform;

            holdPower = 1f;
            timer = 0;

            while (holdPower < maxHoldPower && timer <= maxHoldTime / 2 && enemy.enemyStatus.isAttack)
            {
                holdPower += 0.05f;
                spinSimul.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Cancle()
    {
        base.Cancle();
        StopCoroutine(Simulation());
        user.GetComponent<Stats>().decreasedMoveSpeed -= 0.5f;
        Destroy(spinSimul);
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
            Weapon weapon = player.weaponList[player.playerStats.weapon];
            GameObject effect = Instantiate(spinPrefab, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();
            float attackRate = weapon.SPA / player.playerStats.attackSpeed;

            // 쿨타임 적용
            skillCoolTime = (1 + player.playerStats.skillCoolTime) * skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, holdPower * size * player.weaponList[player.playerStats.weapon].attackSize, 0);
            effect.tag = "PlayerAttack";
            effect.layer = LayerMask.NameToLayer("PlayerAttack");
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
             player.weaponList[player.playerStats.weapon].knockBack * 10 * holdPower, 
             player.playerStats.criticalChance, 
             player.playerStats.criticalDamage);
            hitDetection.SetSEs(player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = user;

            // rate 동안 유지
            player.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time * attackRate);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();
            GameObject effect = Instantiate(spinPrefab, user.transform.position, user.transform.rotation);
            HitDetection hitDetection = effect.GetComponent<HitDetection>();

            // 쿨타임 적용
            skillCoolTime =skillDefalutCoolTime;

            Destroy(spinSimul);

            effect.transform.parent = user.transform;
            effect.transform.localScale = new Vector3(holdPower * size, holdPower * size, 0);
            effect.tag = "EnemyAttack";
            effect.layer = LayerMask.NameToLayer("EnemyAttack");

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
             10 * holdPower);
            hitDetection.user = user;

            // rate 동안 유지
            enemy.stats.decreasedMoveSpeed -= 0.5f;
            Destroy(effect, time);
        }
    }

    
}
