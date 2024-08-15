using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class StunDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()
    {
        if (target.tag == "Player" || target.tag == "Enemy")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            // ȿ�� ����
            objectBasic.status.isFlinch = true;
            objectBasic.AttackCancle();

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - objectBasic.stats.SEResist(buffId)) * defaultDuration;

            if (objectBasic.status.flinchCoroutine != null) objectBasic.StopCoroutine(objectBasic.status.flinchCoroutine);
            objectBasic.SetFlinch(duration);

            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player" || target.tag == "Enemy" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            while (objectBasic.status.isFlinch)
            {
                objectBasic.stats.poise = 0;
                yield return null;
            }

            duration = 0;
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player" || target.tag == "Enemy" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            if (objectBasic.status.flinchCoroutine != null) StopCoroutine(objectBasic.status.flinchCoroutine);
        }
    }
}

