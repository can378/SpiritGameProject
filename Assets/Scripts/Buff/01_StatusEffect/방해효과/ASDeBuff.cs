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
            duration = (1 - stats.SEResist(buffId)) * defaultDuration;

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
                    player.playerStatus.attackDelay = 99f;

                if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
                    player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime = 99f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();

            //player���� isEnemyAttackalve false�̸� ���ؾȹް� �ϱ�
            print("enemy fear");
            enemy.enemyStatus.isAttackReady = false;

            while (duration > 0)
            {
                enemy.enemyStatus.isAttackReady = false;
                enemy.enemyStatus.isRun = true;
                yield return new WaitForSeconds(0.1f);
            }


        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
            {
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime = curSkillCoolTIme;
            }

            if (player.playerStats.weapon != 0)
            {
                player.playerStatus.attackDelay = curAttackDelay;
            }
            StopCoroutine(attackDelayTimeCoroutine);
        }
        else if (target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();

            enemy.enemyStatus.isAttackReady = true;
            enemy.enemyStatus.isRun = false;
            
            StopCoroutine(attackDelayTimeCoroutine);

        }
    }
}

