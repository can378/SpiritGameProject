using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격 및 스킬 불가
public class ASDeBuff : StatusEffect
{
    // 공격 및 스킬 불가
    // 공격 및 스킬 사용 불가
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

            // 중첩 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // 저항에 따른 지속시간 적용

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

            //player에서 isEnemyAttackalve false이면 피해안받게 하기
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

