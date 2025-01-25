using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class StunDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����
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

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - objectBasic.stats.SEResist((int)buffType)) * defaultDuration;

            // ȿ�� ����
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

