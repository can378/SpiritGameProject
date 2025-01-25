using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기절
public class StunDeBuff : StatusEffect
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제
    ObjectBasic objectBasic;

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()
    {
        if (target.tag == "Player" || target.tag == "Enemy")
        {
            objectBasic = target.GetComponent<ObjectBasic>();

            // 중첩 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - objectBasic.stats.SEResist((int)buffType)) * defaultDuration;

            // 효과 적용
            objectBasic.AttackCancle();
            objectBasic.SetFlinch(duration);
        }
    }

    public override void Progress()
    {
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, duration);
    }

    public override void Remove()
    {
        if (target.tag == "Player" || target.tag == "Enemy" || target.tag == "Npc")
        {
            objectBasic = target.GetComponent<ObjectBasic>();
            objectBasic.ClearFlinch();
        }
    }
}

