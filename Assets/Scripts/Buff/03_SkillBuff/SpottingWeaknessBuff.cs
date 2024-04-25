using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingWeaknessBuff : StatusEffect
{
    // ����
    // ġ��Ÿ Ȯ��
    [field: SerializeField] public float addCC { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //���ӽð� ����
    {   
        // �÷��̾��Ͻ�
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.addCriticalChance -= overlap * addCC;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - playerStats.SEResist) * defaultDuration;

            playerStats.addCriticalChance += overlap * addCC;
        }
        // �� �ܿ�
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.increasedAttackPower -= overlap * 0.5f;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - stats.SEResist) * defaultDuration;

            stats.increasedAttackPower += overlap * 0.5f;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.increasedCriticalChance -= overlap * addCC;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.increasedAttackPower -= overlap * 0.5f;
        }

    }
}