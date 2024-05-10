using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기절
public class StunDeBuff : StatusEffect
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // 효과 적용
            player.isFlinch = true;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - player.stats.SEResist) * defaultDuration;

            player.StopCoroutine(player.flinchCoroutine);
            player.flinchCoroutine = StartCoroutine(player.Flinch(duration));

            StartCoroutine(Stun());
        }
        else if (target.tag == "Enemy")
        {
            print("enemy stun - reset effect");
            EnemyStats enemy = target.GetComponent<EnemyStats>();

        
        }
        
    }

    IEnumerator Stun()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            while(player.isFlinch)
            {
                yield return null;
            }

            duration = 0;
        }
        else if (target.tag == "Enemy")
        {
            print("enemy stun");
            EnemyStats stats = target.GetComponent<EnemyStats>();
            stats.isEnemyStun = true;
            target.GetComponent<EnemyBasic>().StopAllCoroutines();

            while (duration > 0)
            {
                target.GetComponent<EnemyBasic>().runAway();
                //player에서 isEnemyAttackalve false이면 피해안받게 하기
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            if (player.flinchCoroutine != null) StopCoroutine(player.flinchCoroutine);
        }
        else if (target.tag == "Enemy")
        {
            EnemyStats stats = target.GetComponent<EnemyStats>();
            target.GetComponent<EnemyBasic>().RestartAllCoroutines();

            stats.isEnemyStun = false;
        }
    }
}

