using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveDeBuff : StatusEffect
{
    // ����
    // �޴� ���� ���� ����, ����� ���� ����
    [field: SerializeField] public float decreasedDefensivePower { get; set; }
    [field: SerializeField] public float decreasedSEResist { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //���ӽð� ����
    {
        Stats stats = target.GetComponent<Stats>();

        // ȿ�� ��� ����
        stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        stats.decreasedSEResist -= overlap * decreasedSEResist;

        // ��ø 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        // ���׿� ���� ���ӽð� ����
        duration = (1 - stats.SEResist) * defaultDuration;

        stats.decreasedDefensivePower += overlap * decreasedDefensivePower;
        stats.decreasedSEResist += overlap * decreasedSEResist;
    }

    public override void RemoveEffect()
    {
        Stats stats = target.GetComponent<Stats>();

        stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        stats.decreasedSEResist -= overlap * decreasedDefensivePower;
    }
}