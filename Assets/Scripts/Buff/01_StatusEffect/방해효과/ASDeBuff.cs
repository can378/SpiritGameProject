using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �� ��ų �Ұ�
public class ASDeBuff : StatusEffect
{
    // ���� �� ��ų �Ұ�
    // ���� �� ��ų ��� �Ұ�
    float curSkillCoolTIme;
    float curAttackDelay;

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()
    {
        if (target.tag == "Player")
        {
            Stats stats = target.GetComponent<Stats>();

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����

            float Resist = (int)buffType < (int)BuffType.SPECIAL ? stats.SEResist[(int)buffType].Value : 0;
            duration = (1 - Resist) * defaultDuration;

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

    public override void Progress()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.playerStats.weapon != 0)
                player.playerStatus.attackDelay = 99f;

            if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime = 99f;
        }
        else if (target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();

            enemy.enemyStatus.isAttackReady = false;
            enemy.enemyStatus.isRun = true;
        }
    }

    public override void Remove()
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
        }
        else if (target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();

            enemy.enemyStatus.isAttackReady = true;
            enemy.enemyStatus.isRun = false;
            

        }
    }
}

