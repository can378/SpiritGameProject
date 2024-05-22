using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDeBuff : StatusEffect
{
    // ����
    // ���ݷ�, ���� ����
    [field: SerializeField] public float decreasedAttackPower { get; set; }
    [field: SerializeField] public float decreasedSkillPower { get; set; }

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
            playerStats.decreasedAttackPower -= overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower -= overlap * decreasedSkillPower;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - playerStats.SEResist(buffId)) * defaultDuration;

            playerStats.decreasedAttackPower += overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower += overlap * decreasedSkillPower;
        }
        // �� �ܿ�
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.decreasedAttackPower -= overlap * decreasedAttackPower;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - stats.SEResist(buffId)) * defaultDuration;

            stats.decreasedAttackPower += overlap * decreasedAttackPower;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedAttackPower -= overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower -= overlap * decreasedSkillPower;

        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedAttackPower -= overlap * decreasedAttackPower;
        }

    }
}