using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기절
public class GrapDeBuff : StatusEffect
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제
    ObjectBasic objectBasic;
    Player player;
    EnemyBasic enemyBasic;

    bool LeftRight;     // False면 Left, True면 Right;

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()
    {

        if (target.tag == "Player")
        {
            player = target.GetComponent<Player>();
            objectBasic = target.GetComponent<ObjectBasic>();

            // 중첩 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // 저항에 따른 지속시간 적용
            duration = defaultDuration;

            // 효과 적용
            objectBasic.SetFlinch(duration);
        }
        if (target.tag == "Enemy")
        {
            enemyBasic = target.GetComponent<EnemyBasic>();
            objectBasic = target.GetComponent<ObjectBasic>();

            // 중첩 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // 저항에 따른 지속시간 적용
            duration = defaultDuration;

            // 효과 적용
            objectBasic.SetFlinch(duration);
        }
    }

    public override void Progress()
    {
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, duration);
        if (target.tag == "Player")
        {
            // 왼쪽 눌러야 할 때 왼쪽 누름
            if ((!LeftRight && player.hAxis < 0.0f) || (LeftRight && player.hAxis > 0.0f))
            {
                duration -= 0.5f;
                LeftRight = !LeftRight;
            }
        }
        if (target.tag == "Enemy")
        {

        }
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

