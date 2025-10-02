using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/DefensiveDe")]

public class DefensiveDeBuff : BuffData
{
    // ����
    // �޴� ���� ���� ����, ����� ���� ����
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)      //���ӽð� ����
    {
        Stats stats = _Buff.target.GetComponent<Stats>();

        // ȿ�� ��� ����
        stats.DefensivePower.DecreasedValue -= _Buff.stack * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue -= _Buff.stack * decreasedSEResist;
        }

        // ��ø 
        _Buff.AddStack();

        // ���׿� ���� ���ӽð� ����
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