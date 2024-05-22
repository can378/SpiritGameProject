using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDeBuff : StatusEffect
{
    // 배율
    // 공격력, 도력 감소
    [field: SerializeField] public float decreasedAttackPower { get; set; }
    [field: SerializeField] public float decreasedSkillPower { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //지속시간 갱신
    {   
        // 플레이어일시
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.decreasedAttackPower -= overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower -= overlap * decreasedSkillPower;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - playerStats.SEResist(buffId)) * defaultDuration;

            playerStats.decreasedAttackPower += overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower += overlap * decreasedSkillPower;
        }
        // 그 외에
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.decreasedAttackPower -= overlap * decreasedAttackPower;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - stats.SEResist(buffId)) * defaultDuration;

            stats.decreasedAttackPower += overlap * decreasedAttackPower;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedAttackPower -= overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower -= overlap * decreasedSkillPower;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedAttackPower -= overlap * decreasedAttackPower;
        }

    }
}