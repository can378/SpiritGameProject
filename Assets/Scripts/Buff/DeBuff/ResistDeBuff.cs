using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistDeBuff : StatusEffect
{
    // 배율
    [field: SerializeField] public float addResist { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        Stats stats = target.GetComponent<Stats>();

        // 효과 잠시 제거
        for(int i = 0; i <stats.resist.Length ; i++)
        {
            stats.resist[i] -= overlap * addResist;
        }
        

        // 중첩 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        // 저항에 따른 지속시간 적용
        duration = (1 - (stats.resist[resist] * 2)) * defaultDuration;

        for (int i = 0; i < stats.resist.Length; i++)
        {
            stats.resist[i] += overlap * addResist;
        }
        
    }

    public override void RemoveEffect()
    {
        Stats stats = target.GetComponent<Stats>();

        for (int i = 0; i < stats.resist.Length; i++)
        {
            stats.resist[i] -= overlap * addResist;
        }

    }
}