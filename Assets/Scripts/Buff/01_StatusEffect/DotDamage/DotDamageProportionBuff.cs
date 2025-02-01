using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageProportionBuff : StatusEffect
{
    // �ִ� ü�� ��� %��ġ
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damagePerTick { get; set; }
    float tick = 0.1f;

    public override void Apply()
    {
        // �ֱ������� ���ظ� ������ �ڷ�ƾ ����
        Overlap();
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void Remove()
    {
        // ����� ���� �� ���� �ڷ�ƾ �ߴ�
        //Debug.Log("DoT debuff removed");
    }

    public override void Overlap()      //���ӽð� ����
    {
        // ��ø 
        overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;
        //print(duration);
    }

    public override void Progress()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {
            DealDamageToTarget(damagePerTick);
            tick += 0.1f;
        }
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damagePerTick);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        if(target.tag == "Player" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
            objectBasic.Damaged(objectBasic.stats.HPMax * damage * overlap);
        }
        else if(target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();
            enemy.Damaged(enemy.stats.HPMax * damage * overlap);
        }
    }
}
