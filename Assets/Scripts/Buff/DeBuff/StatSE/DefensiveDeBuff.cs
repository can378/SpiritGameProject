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
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist -= overlap * decreasedSEResist;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = playerStats.SEResist * defaultDuration;

            playerStats.decreasedDefensivePower += overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist += overlap * decreasedSEResist;
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = stats.SEResist * defaultDuration;

            stats.decreasedDefensivePower += overlap * decreasedDefensivePower;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
            playerStats.decreasedSEResist -= overlap * decreasedSEResist;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedDefensivePower -= overlap * decreasedDefensivePower;
        }

    }
}