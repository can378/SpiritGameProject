using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격불가
/*
public class AttackDeBuff : StatusEffect
{
    // 공격 불가
    // 공격 사용 불가
    private Coroutine ASTimeCoroutine;

    public override void Apply()
    {
        Overlap();
        ASTimeCoroutine = StartCoroutine(ASOverTime());
    }

    public override void Overlap()
    {
        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;
    }

    IEnumerator ASOverTime()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            player.playerStatus.attackDelay += 0.1f;

            while (duration > 0)
            {
                player.playerStatus.attackDelay += 0.1f;
                yield return new WaitForSeconds(0.1f);

            }
        }
    }

    public override void Remove()
    {
        StopCoroutine(ASTimeCoroutine);
    }
}
*/