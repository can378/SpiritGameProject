using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class StunDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.status.isFlinch = true;

            // ��ø 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = (1 - player.stats.SEResist) * defaultDuration;
        }
        
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();

            // ȿ�� ����
            player.status.isFlinch = false;
        }
    }
}

