using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기절
public class GrapDeBuff : StatusEffect
{

    ObjectBasic GrapOwner;      // 잡기를 시전한 대상
    Transform GrapPos;

    // 공격 및 스킬, 이동 불가
    // 피격 시 해제
    ObjectBasic objectBasic;
    Player player;

    bool LeftRight;     // False면 Left, True면 Right;
    int KeyDownCount;

    float Tick = 0.0f;         // 0.1초

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
            objectBasic = target.GetComponent<ObjectBasic>();

            // 중첩 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // 저항에 따른 지속시간 적용
            duration = defaultDuration;

            // 효과 적용
            objectBasic.SetFlinch(duration);
        }
        KeyDownCount = 0;
    }

    public override void Progress()
    {
        // 잡기를 시전한 대상이 있을 때 경직 상태거나 죽었다면 해제
        // 또는 KeyDownCount가 조건을 만족할 때
        if(GrapOwner == null || 0 < GrapOwner.status.isFlinch || !GrapOwner.gameObject.activeSelf || KeyDownCount > 30 )
        {
            duration = 0.0f;
            return;
        }

        // 붙잡힐 위치
        if(GrapPos!= null)
            target.transform.position = GrapPos.position;
            
        // 경직 시킴
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, duration);

        if (target.tag == "Player")
        {
            // 왼쪽 눌러야 할 때 왼쪽 누름
            if ((!LeftRight && player.hAxis < 0.0f) || (LeftRight && player.hAxis > 0.0f))
            {
                KeyDownCount += (int)(1 + objectBasic.stats.SEResist[(int)buffType].Value * 10);
                LeftRight = !LeftRight;
            }
        }
        if (target.tag == "Enemy")
        {
            Tick += Time.deltaTime;

            // 해당 Tick마다 몬스터가 KeyCount를 누름
            // 방해 저항
            if (Tick > 0.5f)
            {
                KeyDownCount += (int)(1  + objectBasic.stats.SEResist[(int)buffType].Value * 10);
                Tick -= 0.5f;
            }
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

    public void SetGrapOwner(ObjectBasic _GrapOwner, Transform _GrapPos = null)
    {
        GrapOwner = _GrapOwner;
        GrapPos = _GrapPos;
    }

}

