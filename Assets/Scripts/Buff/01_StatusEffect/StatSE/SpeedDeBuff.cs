using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDeBuff : StatusEffect
{
    // ����
    // �̵��ӵ�, ���ݼӵ� ����
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()      //���ӽð� ����
    {
        // �÷��̾��� ��
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - playerStats.SEResist((int)buffType)) * defaultDuration;

            playerStats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed += overlap * decreasedAttackSpeed;

        }
        // �� �ܿ�
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - stats.SEResist((int)buffType)) * defaultDuration;

            stats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;

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

            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;

        }

    }
}