using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �� ��ų �Ұ�
public class ASDeBuff : StatusEffect
{
    // ���� �� ��ų �Ұ�
    // ���� �� ��ų ��� �Ұ�
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

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
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

            //player���� isEnemyAttackalve false�̸� ���ؾȹް� �ϱ�
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

