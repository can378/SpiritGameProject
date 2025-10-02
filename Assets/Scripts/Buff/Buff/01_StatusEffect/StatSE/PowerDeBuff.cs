using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/PowerDe")]

public class PowerDeBuff : BuffData
{
    // 배율
    // 공격력, 도력 감소
    [field: SerializeField] public float decreasedAttackPower { get; set; }
    [field: SerializeField] public float decreasedSkillPower { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //지속시간 갱신
    {
        // 플레이어일시
        Stats stats = _Buff.target.GetComponent<Stats>();

        // 효과 잠시 제거
        stats.AttackPower.DecreasedValue -= _Buff.stack * decreasedAttackPower;
        stats.SkillPower.DecreasedValue -= _Buff.stack * decreasedSkillPower;

        // 중첩 
        _Buff.AddStack();

        // 저항에 따른 지속시간 적용
        _Buff.curDuration = _Buff.duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;

        stats.AttackPower.DecreasedValue += _Buff.stack * decreasedAttackPower;
        stats.SkillPower.DecreasedValue += _Buff.stack * decreasedSkillPower;
    }

    public override void Update_Buff(Buff _Buff)
    {

    }

    public override void Remove(Buff _Buff)
    {
        Stats stats = _Buff.target.GetComponent<Stats>();

        stats.AttackPower.DecreasedValue -= _Buff.stack * decreasedAttackPower;
        stats.SkillPower.DecreasedValue -= _Buff.stack * decreasedSkillPower;

    }
}