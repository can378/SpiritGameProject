using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/Stun")]
// ����
public class StunDeBuff : BuffData
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����
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

            // ��ø 
            _Buff.AddStack();

            // ���׿� ���� ���ӽð� ����
            _Buff.curDuration = _Buff.duration = (1 - objectBasic.stats.SEResist[(int)buffType].Value) * defaultDuration;

            // ȿ�� ����
            objectBasic.FlinchCancle();
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

