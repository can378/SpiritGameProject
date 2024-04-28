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
        Stats stats = target.GetComponent<Stats>();
        
        // ��ø 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        // ���׿� ���� ���ӽð� ����
        duration = (1 - stats.SEResist) * defaultDuration;
    }

    IEnumerator AttackDelayOverTime()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while (duration > 0)
            {
                if (player.stats.weapon != 0)
                    player.status.attackDelay = 99f;

                if (player.stats.skill[player.status.skillIndex] != 0)
                    player.skillController.skillList[player.stats.skill[player.status.skillIndex]].skillCoolTime = 99f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();
            while (duration > 0)
            {
                stats.isEnemyAttackable = false;
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.stats.skill[player.status.skillIndex] != 0)
            {
                player.skillController.skillList[player.stats.skill[player.status.skillIndex]].skillCoolTime = curSkillCoolTIme;
            }

            if (player.stats.weapon != 0)
            {
                player.status.attackDelay = curAttackDelay;
            }
            StopCoroutine(attackDelayTimeCoroutine);
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();
            stats.isEnemyAttackable = true;
            StopCoroutine(attackDelayTimeCoroutine);
        }
    }
}

