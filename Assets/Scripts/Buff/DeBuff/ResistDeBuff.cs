using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistDeBuff : StatusEffect
{
    // ����
    [field: SerializeField] public float addResist { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //���ӽð� ����
    {
        Stats stats = target.GetComponent<Stats>();

        // ȿ�� ��� ����
        for(int i = 0; i <stats.resist.Length ; i++)
        {
            stats.resist[i] -= overlap * addResist;
        }
        

        // ��ø 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        // ���׿� ���� ���ӽð� ����
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