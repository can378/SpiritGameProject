using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기절
public class StunDeBuff : StatusEffect
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // 효과 적용
            player.status.isFlinch = true;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - player.stats.SEResist) * defaultDuration;
        }
        
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // 효과 적용
            player.status.isFlinch = false;
        }
    }
}

