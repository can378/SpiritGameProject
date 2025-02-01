using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveDeBuff : StatusEffect
{
    // ����
    // �޴� ���� ���� ����, ����� ���� ����
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()      //���ӽð� ����
    {
        Stats stats = target.GetComponent<Stats>();

        // ȿ�� ��� ����
        stats.DefensivePower.DecreasedValue -= overlap * decreasedDefensivePower;
        for(int i = 0; i < stats.SEResist.Length ; i++ )
        {
            stats.SEResist[i].DecreasedValue -= overlap * decreasedSEResist;
        }

        // ��ø 
        overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

        // ���׿� ���� ���ӽð� ����
        duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;

        stats.DefensivePower.DecreasedValue += overlap * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue += overlap * decreasedSEResist;
        }
    }

    public override void Progress()
    {
        
    }

    public override void Remove()
    {
        Stats stats = target.GetComponent<Stats>();

        stats.DefensivePower.DecreasedValue -= overlap * decreasedDefensivePower;
        for (int i = 0; i < stats.SEResist.Length; i++)
        {
            stats.SEResist[i].DecreasedValue -= overlap * decreasedSEResist;
        }
    }
}