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
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.status.isFlinch = true;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - player.stats.SEResist) * defaultDuration;

            if (player.FlinchCoroutine != null) player.StopCoroutine(player.FlinchCoroutine);
            player.FlinchCoroutine = StartCoroutine(player.Flinch(duration));

            StartCoroutine(Stun());
        }
        else if (target.tag == "Enemy")
        { }
        
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while(player.status.isFlinch)
            {
                yield return null;
            }

            duration = 0;
        }
        else if (target.tag == "Enemy")
        { }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.FlinchCoroutine != null) StopCoroutine(player.FlinchCoroutine);
        }
        else if (target.tag == "Enemy")
        { }
    }
}

