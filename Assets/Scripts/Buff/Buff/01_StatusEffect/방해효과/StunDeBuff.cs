using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/Stun")]
// 기절
public class StunDeBuff : BuffData
{
    // 공격 및 스킬, 이동 불가
    // 피격 시 해제
    ObjectBasic objectBasic;

    public override void Apply(Buff _Buff)
    {
        Overlap(_Buff);
    }

    public override void Overlap(Buff _Buff)
    {
        if (_Buff.target.tag == "Player" || _Buff.target.tag == "Enemy")
        {
            objectBasic = _Buff.target.GetComponent<ObjectBasic>();

            // 중첩 
            _Buff.AddStack();

            // 저항에 따른 지속시간 적용
            _Buff.curDuration = _Buff.duration = (1 - objectBasic.stats.SEResist[(int)buffType].Value) * defaultDuration;

            // 효과 적용
            objectBasic.CancleAction();
            objectBasic.SetFlinch(_Buff.curDuration);
        }
    }

    public override void Update_Buff(Buff _Buff)
    {
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, _Buff.curDuration);
    }

    public override void Remove(Buff _Buff)
    {
        if (_Buff.target.tag == "Player" || _Buff.target.tag == "Enemy" || _Buff.target.tag == "Npc")
        {
            objectBasic = _Buff.target.GetComponent<ObjectBasic>();
            objectBasic.ClearFlinch();
        }
    }
}

