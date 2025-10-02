using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/DefensiveDe")]

public class DefensiveDeBuff : BuffData
{
    // 배율
    // 받는 피해 저항 감소, 디버프 저항 감소
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //지속시간 갱신
    {
        Stats stats = _Buff.target.GetComponent<Stats>();

        // 효과 잠시 제거
        stats.DefensivePower.DecreasedValue -= _Buff.stack * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue -= _Buff.stack * decreasedSEResist;
        }

        // 중첩 
        _Buff.AddStack();

        // 저항에 따른 지속시간 적용
        _Buff.curDuration = _Buff.duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;

        stats.DefensivePower.DecreasedValue += _Buff.stack * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue += _Buff.stack * decreasedSEResist;
        }
    }

    public override void Update_Buff(Buff _Buff)
    {

    }

    public override void Remove(Buff _Buff)
    {
        Stats stats = _Buff.target.GetComponent<Stats>();

        stats.DefensivePower.DecreasedValue -= _Buff.stack * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue -= _Buff.stack * decreasedSEResist;
        }
    }
}