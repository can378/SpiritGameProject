using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ݺҰ�
public class AttackDeBuff : StatusEffect
{
    // ���� �Ұ�
    // ���� ��� �Ұ�
    private Coroutine ASTimeCoroutine;

    public override void ApplyEffect()
    {
        ResetEffect();
        ASTimeCoroutine = StartCoroutine(ASOverTime());
    }

    public override void ResetEffect()
    {
        Stats stats = target.GetComponent<Stats>();
        duration = stats.SEResist * defaultDuration;
    }

    IEnumerator ASOverTime()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.stats.weapon != null)
                player.status.attackDelay += 0.1f;

            while (duration > 0)
            {
                if (player.stats.weapon != null)
                    player.status.attackDelay += 0.1f;
                yield return new WaitForSeconds(0.1f);

            }
        }
    }

    public override void RemoveEffect()
    {

    }
}
