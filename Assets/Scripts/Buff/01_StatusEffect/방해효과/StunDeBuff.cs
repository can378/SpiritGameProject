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
        if (target.tag == "Player" || target.tag == "Enemy")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();

            // 효과 적용
            objectBasic.status.isFlinch = true;
            objectBasic.AttackCancle();

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
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

