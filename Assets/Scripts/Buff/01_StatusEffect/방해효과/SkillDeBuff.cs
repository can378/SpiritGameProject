using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ä§¹¬
// 
public class SkillDeBuff : StatusEffect
{
    // Ä§¹¬
    // ½ºÅ³ »ç¿ë ºÒ°¡
    Coroutine skillCoolTimeCoroutine;

    public override void ApplyEffect()
    {
        ResetEffect();
        skillCoolTimeCoroutine = StartCoroutine(SkillCoolOverTime());
    }

    public override void ResetEffect()
    {
        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;
    }

    IEnumerator SkillCoolOverTime()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            player.skillList[player.playerStats.skill[player.status.skillIndex]].skillCoolTime += 0.1f;

            while (duration > 0)
            {
                player.skillList[player.playerStats.skill[player.status.skillIndex]].skillCoolTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void RemoveEffect()
    {
        StopCoroutine(skillCoolTimeCoroutine);
    }
}
