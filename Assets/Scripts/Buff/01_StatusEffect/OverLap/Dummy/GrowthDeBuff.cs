using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthDeBuff : StatusEffect
{
    [field: SerializeField] public float increasedStat { get; set; }
    [field:SerializeField] public int addDeBuff {get; set; }
    // �ִ� ��ø �� ���ظ� �ְ� ���ŵǴ� ����� 
    // ���׿� ���� ���ط��� ����

    public override void Apply()     //�߰�
    {
        Overlap();
    }
    public override void Overlap()     //����
    {
        Stats stats = target.GetComponent<Stats>();

        // ȿ�� ��� ����
        stats.increasedAttackPower -= overlap * increasedStat;
        stats.increasedMoveSpeed -= overlap * increasedStat;
        target.transform.localScale = new Vector3(1,1,1);

        // ��ø 
        overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;
        if (overlap == DefaultMaxOverlap)
        {
            duration = 0;
            return;
        }

        // ���׿� ���� ���ӽð� ����
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;

        stats.increasedAttackPower += overlap * increasedStat;
        stats.increasedMoveSpeed += overlap * increasedStat;
        target.transform.localScale = new Vector3(1 + overlap * increasedStat, 1 + overlap * increasedStat, 1 + overlap * increasedStat);

    }

    public override void Progress()
    {
        
    }

    public override void Remove()    //����
    {
        if (overlap == DefaultMaxOverlap)
        {
            if (target.tag == "Player")
            {
                Player player = target.GetComponent<Player>();
                player.ApplyBuff(addDeBuff);
            }
            else if (target.tag == "Enemy" || target.tag == "Npc")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                enemy.ApplyBuff(addDeBuff);
            }
        }

    }
}
