using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ħ��
//
/*
public class SkillDeBuff : StatusEffect
{
    // ħ��
    // ��ų ��� �Ұ�
    Coroutine skillCoolTimeCoroutine;

    public override void Apply()
    {
        Overlap();
        skillCoolTimeCoroutine = StartCoroutine(SkillCoolOverTime());
    }

    public override void Overlap()
    {
        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;
    }

    IEnumerator SkillCoolOverTime()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime += 0.1f;

            while (duration > 0)
            {
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillCoolTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void Remove()
    {
        StopCoroutine(skillCoolTimeCoroutine);
    }
}
*/