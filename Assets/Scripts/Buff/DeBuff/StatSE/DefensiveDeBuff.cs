using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveDeBuff : StatusEffect
{
    // 배율
    // 받는 피해 저항 감소, 디버프 저항 감소
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist -= overlap * decreasedSEResist;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = playerStats.SEResist * defaultDuration;

            playerStats.decreasedDefensivePower += overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist += overlap * decreasedSEResist;
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = stats.SEResist * defaultDuration;

            stats.decreasedDefensivePower += overlap * decreasedDefensivePower;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist -= overlap * decreasedSEResist;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        }

    }
}