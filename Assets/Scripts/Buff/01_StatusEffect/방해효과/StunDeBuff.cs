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
        if (target.tag == "Player" || target.tag == "Enemy" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            // ȿ�� ����
            objectBasic.isFlinch = true;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - objectBasic.stats.SEResist(buffId)) * defaultDuration;

            objectBasic.StopCoroutine(objectBasic.flinchCoroutine);
            objectBasic.flinchCoroutine = StartCoroutine(objectBasic.Flinch(duration));

            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player" || target.tag == "Enemy" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            while (objectBasic.isFlinch)
            {
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

            if (objectBasic.flinchCoroutine != null) StopCoroutine(objectBasic.flinchCoroutine);
        }
    }
}

