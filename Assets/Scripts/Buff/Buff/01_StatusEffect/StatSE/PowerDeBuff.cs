using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/PowerDe")]

public class PowerDeBuff : BuffData
{
    // ����
    // ���ݷ�, ���� ����
    [field: SerializeField] public float decreasedAttackPower { get; set; }
    [field: SerializeField] public float decreasedSkillPower { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //���ӽð� ����
    {
        // �÷��̾��Ͻ�
        Stats stats = _Buff.target.GetComponent<Stats>();

        // ȿ�� ��� ����
        stats.AttackPower.DecreasedValue -= _Buff.stack * decreasedAttackPower;
        stats.SkillPower.DecreasedValue -= _Buff.stack * decreasedSkillPower;

        // ��ø 
        _Buff.AddStack();

        // ���׿� ���� ���ӽð� ����
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