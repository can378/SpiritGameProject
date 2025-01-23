using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveDeBuff : StatusEffect
{
    // 배율
    // 받는 피해 저항 감소, 디버프 저항 감소
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()      //지속시간 갱신
    {
        Stats stats = target.GetComponent<Stats>();

        // 효과 잠시 제거
        stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        for(int i = 0; i < stats.defaultSEResist.Length ; i++ )
        {
            stats.decreasedSEResist[i] -= overlap * decreasedSEResist;
        }

        // 중첩 
        overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

        // 저항에 따른 지속시간 적용
        duration = (1 - stats.SEResist((int)buffType)) * defaultDuration;

        stats.decreasedDefensivePower += overlap * decreasedDefensivePower;
        for (int i = 0; i < stats.defaultSEResist.Length; i++)
        {
            stats.decreasedSEResist[i] += overlap * decreasedSEResist;
        }
    }

    public override void Progress()
    {
        
    }

    public override void Remove()
    {
        Stats stats = target.GetComponent<Stats>();

        stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        for (int i = 0; i < stats.defaultSEResist.Length; i++)
        {
            stats.decreasedSEResist[i] -= overlap * decreasedSEResist;
        }
    }
}