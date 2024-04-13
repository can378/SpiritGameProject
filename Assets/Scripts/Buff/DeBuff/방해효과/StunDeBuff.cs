using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class StunDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����

    Coroutine StunCoroutine;

    public override void ApplyEffect()
    {
        ResetEffect();
        StunCoroutine = StartCoroutine(Stun());
    }

    public override void ResetEffect()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.status.isFlinch = true;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - player.stats.SEResist) * defaultDuration;
        }
        
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while (duration > 0)
            {
                if(!player.status.isFlinch)
                {
                    duration = 0;
                    break;
                }
                yield return null;
            }
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.status.isFlinch = false;

            StopCoroutine(Stun());
        }
    }
}

