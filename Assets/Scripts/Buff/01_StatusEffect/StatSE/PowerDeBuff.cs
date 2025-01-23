using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDeBuff : StatusEffect
{
    // ����
    // ���ݷ�, ���� ����
    [field: SerializeField] public float decreasedAttackPower { get; set; }
    [field: SerializeField] public float decreasedSkillPower { get; set; }

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
            playerStats.decreasedAttackPower -= overlap * decreasedAttackPower;
            playerStats.decreasedSkillPower -= overlap * decreasedSkillPower;

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - playerStats.SEResist((int)buffType)) * defaultDuration;

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
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - stats.SEResist((int)buffType)) * defaultDuration;

            stats.decreasedAttackPower += overlap * decreasedAttackPower;
        }
    }

    public override void Progress()
    {
        
    }

    public override void Remove()
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