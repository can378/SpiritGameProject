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
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // ȿ�� ��� ����
            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = playerStats.SEResist * defaultDuration;

            playerStats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed += overlap * decreasedAttackSpeed;

            /*
            Debug.Log("MoveSpeed buff applied: " + overlap * decreasedMoveSpeed + "MoveSpeed");
            Debug.Log("MoveSpeed : " + playerStats.moveSpeed);
            Debug.Log("AttackSpeed buff applied: " + overlap * decreasedAttackSpeed + " AttackSpeed");
            Debug.Log("AttackSpeed : " + playerStats.attackSpeed);
            */
            //print(duration);
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // ȿ�� ��� ����
            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            //stats.decreaseddAttackSpeed -= overlap * decreasedAttackSpeed;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = stats.SEResist * defaultDuration;

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