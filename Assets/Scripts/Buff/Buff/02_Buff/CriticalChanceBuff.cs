using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class CriticalChanceBuff : StatusEffect
{
    // ����
    // ġ��Ÿ Ȯ��
    [field: SerializeField] public float addCC { get; set; }

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()      //���ӽð� ����
    {   
        // �÷��̾��Ͻ�
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.addCriticalChance -= overlap * addCC;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            duration = defaultDuration;

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

            duration = defaultDuration;

            stats.increasedAttackPower += overlap * 0.5f;
        }
    }

    public override void Remove()
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
*/