using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class AttackSpeedBuff : StatusEffect
{
    // ����
    // ���ݼӵ� ����
    [field: SerializeField] public float increasedAttackSpeed { get; set; }

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
            playerStats.increasedAttackSpeed -= overlap * increasedAttackSpeed;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = defaultDuration;

            playerStats.increasedAttackSpeed += overlap * increasedAttackSpeed;

        }
        // �� �ܿ�
        else
        {

        }
    }

    public override void Remove()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.increasedAttackSpeed -= overlap * increasedAttackSpeed;
        }
        else
        {

        }
    }
}
*/