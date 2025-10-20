using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/AS")]
// ���� �� ��ų �Ұ�
public class ASDeBuff : BuffData
{
    // ���� �� ��ų �Ұ�
    // ���� �� ��ų ��� �Ұ�
    float curSkillCoolTIme;     // ���� ��ų ��Ÿ�� ���� ��
    float curAttackDelay;       // ���� ���� ��Ÿ�� ���� ��

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)
    {
        Stats stats = _Buff.target.stats;

        // ��ø 
        _Buff.AddStack();

        // ���׿� ���� ���ӽð� ����
        float Resist = (int)buffType < (int)BuffType.SPECIAL ? stats.SEResist[(int)buffType].Value : 0;
        _Buff.curDuration = _Buff.duration = (1 - Resist) * defaultDuration;

    }

/*
    IEnumerator AttackDelayOverTime()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while (duration > 0)
            {
                if (player.playerStats.weapon.weaponData == null)
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
*/
    public override void Update_Buff(Buff _Buff)
    {
        if (_Buff.target.tag == "Player")
        {
            Player player = _Buff.target.GetComponent<Player>();

            if (player.playerStats.weapon.weaponInstance == null)
                player.playerStatus.attackDelay = 99f;

            if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime = 99f;
        }
        else if (_Buff.target.tag == "Enemy")
        {
            EnemyBasic enemy = _Buff.target.GetComponent<EnemyBasic>();

            enemy.enemyStatus.isAttackReady = false;
            enemy.enemyStatus.isRun = true;
        }
    }

    public override void Remove(Buff _Buff)
    {
        if (_Buff.target.tag == "Player")
        {
            Player player = _Buff.target.GetComponent<Player>();

            if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
            {
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime = curSkillCoolTIme;
            }

            if (player.playerStats.weapon.weaponInstance == null)
            {
                player.playerStatus.attackDelay = curAttackDelay;
            }
        }
        else if (_Buff.target.tag == "Enemy")
        {
            EnemyBasic enemy = _Buff.target.GetComponent<EnemyBasic>();

            enemy.enemyStatus.isAttackReady = true;
            enemy.enemyStatus.isRun = false;
            

        }
    }
}

