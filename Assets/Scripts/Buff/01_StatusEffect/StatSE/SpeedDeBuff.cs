using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDeBuff : StatusEffect
{
    // ����
    // �̵��ӵ�, ���ݼӵ� ����
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //���ӽð� ����
    {
        // �÷��̾��� ��
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - playerStats.SEResist(buffId)) * defaultDuration;

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
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - stats.SEResist(buffId)) * defaultDuration;

            stats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;

        }
    }

    public override void RemoveEffect()
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