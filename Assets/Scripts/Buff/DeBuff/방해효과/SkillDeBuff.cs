using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ħ��
// 
public class SkillDeBuff : StatusEffect
{
    // ħ��
    // ��ų ��� �Ұ�
    private Coroutine skillCoolTimeCoroutine;

    public override void ApplyEffect()
    {
        ResetEffect();
        skillCoolTimeCoroutine = StartCoroutine(SkillCoolOverTime());
    }

    public override void ResetEffect()
    {
        Stats stats = target.GetComponent<Stats>();
        duration = stats.SEResist * defaultDuration;
    }

    IEnumerator SkillCoolOverTime()
    {
        

        if(target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            if (playerStats.skill != null)
                playerStats.skill.skillCoolTime += 0.1f;

            while (duration > 0)
            {
                if (playerStats.skill != null)
                    playerStats.skill.skillCoolTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
                
            }
        }
    }

    public override void RemoveEffect()
    {
        
    }
}
