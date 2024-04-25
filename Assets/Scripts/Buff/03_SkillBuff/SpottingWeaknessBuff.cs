using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingWeaknessBuff : StatusEffect
{
    // 배율
    // 치명타 확률
    [field: SerializeField] public float addCC { get; set; }

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
            playerStats.addCriticalChance -= overlap * addCC;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - playerStats.SEResist) * defaultDuration;

            playerStats.addCriticalChance += overlap * addCC;
        }
        // 그 외에
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.increasedAttackPower -= overlap * 0.5f;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - stats.SEResist) * defaultDuration;

            stats.increasedAttackPower += overlap * 0.5f;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.increasedCriticalChance -= overlap * addCC;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.increasedAttackPower -= overlap * 0.5f;
        }

    }
}