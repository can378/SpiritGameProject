using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class GrowthDeBuff : Buff
{
    [field: SerializeField] public float increasedStat { get; set; }
    [field: SerializeField] public int addDeBuff { get; set; }
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
        stats.AttackPower.IncreasedValue -= stack * increasedStat;
        stats.MoveSpeed.IncreasedValue -= stack * increasedStat;
        target.transform.localScale = new Vector3(1, 1, 1);

        // ��ø 
        stack = stack < DefaultMaxOverlap ? stack + 1 : DefaultMaxOverlap;
        if (stack == DefaultMaxOverlap)
        {
            duration = 0;
            return;
        }

        // ���׿� ���� ���ӽð� ����
        duration = (1 - stats.SEResist[buffID].Value) * defaultDuration;

        stats.AttackPower.IncreasedValue += stack * increasedStat;
        stats.MoveSpeed.IncreasedValue += stack * increasedStat;
        target.transform.localScale = new Vector3(1 + stack * increasedStat, 1 + stack * increasedStat, 1 + stack * increasedStat);

    }

    public override void Progress()
    {

    }

    public override void Remove()    //����
    {
        if (stack == DefaultMaxOverlap)
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
*/