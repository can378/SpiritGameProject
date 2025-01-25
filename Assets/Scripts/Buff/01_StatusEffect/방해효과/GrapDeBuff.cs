using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class GrapDeBuff : StatusEffect
{

    ObjectBasic GrapOwner;      // ��⸦ ������ ���
    Transform GrapPos;

    // ���� �� ��ų, �̵� �Ұ�
    // �ǰ� �� ����
    ObjectBasic objectBasic;
    Player player;
    EnemyBasic enemyBasic;

    bool LeftRight;     // False�� Left, True�� Right;
    int KeyDownCount;

    float Tick = 0.0f;         // 0.1��

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
        // ��⸦ ������ ����� ���� �� ���� ���°ų� �׾��ٸ� ����
        // �Ǵ� KeyDownCount�� ������ ������ ��
        if(GrapOwner != null && 0 < GrapOwner.status.isFlinch && !GrapOwner.gameObject.activeSelf || KeyDownCount > 25 )
        {
            duration = 0.0f;
            return;
        }

        // ������ ��ġ
        if(GrapPos!= null)
            target.transform.position = GrapPos.position;
            
        // ���� ��Ŵ
        objectBasic.status.isFlinch = Mathf.Max(objectBasic.status.isFlinch, duration);

        if (target.tag == "Player")
        {
            // ���� ������ �� �� ���� ����
            if ((!LeftRight && player.hAxis < 0.0f) || (LeftRight && player.hAxis > 0.0f))
            {
                KeyDownCount += 1;
                LeftRight = !LeftRight;
            }
        }
        if (target.tag == "Enemy")
        {
            Tick += Time.deltaTime;

            // �ش� Tick���� ���Ͱ� KeyCount�� ����
            // ���� ����
            if (Tick > 0.5f)
            {
                KeyDownCount += (int)(1  + enemyBasic.stats.SEResist(2) * 10);
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

