using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class GrapDeBuff : StatusEffect
{
    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����
    ObjectBasic objectBasic;
    Player player;
    EnemyBasic enemyBasic;

    bool LeftRight;     // False�� Left, True�� Right;

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

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = defaultDuration;

            // ȿ�� ����
            objectBasic.SetFlinch(duration);
        }
        if (target.tag == "Enemy")
        {
            enemyBasic = target.GetComponent<EnemyBasic>();
            objectBasic = target.GetComponent<ObjectBasic>();

            // ��ø 
            overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

            // ���׿� ���� ���ӽð� ����
            duration = defaultDuration;

            // ȿ�� ����
            objectBasic.SetFlinch(duration);
        }
    }

    public override void Progress()
    {
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, duration);
        if (target.tag == "Player")
        {
            // ���� ������ �� �� ���� ����
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

