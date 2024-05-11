using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격 및 스킬 불가
public class ASDeBuff : StatusEffect
{
    // 공격 및 스킬 불가
    // 공격 및 스킬 사용 불가
    Coroutine attackDelayTimeCoroutine;
    float curSkillCoolTIme;
    float curAttackDelay;

    public override void ApplyEffect()
    {
        ResetEffect();
        attackDelayTimeCoroutine = StartCoroutine(AttackDelayOverTime());
    }

    public override void ResetEffect()
    {
        if (target.tag == "Player")
        {
            Stats stats = target.GetComponent<Stats>();

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - stats.SEResist) * defaultDuration;

        }
        else if (target.tag == "Enemy")
        {

            duration = 5;
        
        }
        
    }

    IEnumerator AttackDelayOverTime()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while (duration > 0)
            {
                if (player.playerStats.weapon != 0)
                    player.status.attackDelay = 99f;

                if (player.playerStats.skill[player.status.skillIndex] != 0)
                    player.skillController.skillList[player.playerStats.skill[player.status.skillIndex]].skillCoolTime = 99f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();

            //player에서 isEnemyAttackalve false이면 피해안받게 하기
            print("enemy fear");
            stats.isEnemyFear = true;

            target.GetComponent<EnemyBasic>().StopAllCoroutines();
            StartCoroutine(target.GetComponent<EnemyBasic>().runAway());
            yield return new WaitForSeconds(duration);


        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.playerStats.skill[player.status.skillIndex] != 0)
            {
                player.skillController.skillList[player.playerStats.skill[player.status.skillIndex]].skillCoolTime = curSkillCoolTIme;
            }

            if (player.playerStats.weapon != 0)
            {
                player.status.attackDelay = curAttackDelay;
            }
            StopCoroutine(attackDelayTimeCoroutine);
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();

            StopCoroutine(attackDelayTimeCoroutine);
            StopCoroutine(target.GetComponent<EnemyBasic>().runAway());
            
            target.GetComponent<EnemyBasic>().RestartAllCoroutines();
           
            stats.isEnemyFear=false;
            
        }
    }
}

