using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �� ��ų �Ұ�
public class ASDeBuff : StatusEffect
{
    // ���� �� ��ų �Ұ�
    // ���� �� ��ų ��� �Ұ�
    private Coroutine attackDelayTimeCoroutine;
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
        duration = stats.SEResist * defaultDuration;
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
                if (player.stats.skill != 0)
                    player.skillController.skillList[player.stats.skill].skillCoolTime = 99f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.stats.skill != 0)
            {
                player.skillController.skillList[player.stats.skill].skillCoolTime = curSkillCoolTIme;
            }

            if (player.stats.weapon != 0)
            {
                player.status.attackDelay = curAttackDelay;
            }
        }
    }
}

