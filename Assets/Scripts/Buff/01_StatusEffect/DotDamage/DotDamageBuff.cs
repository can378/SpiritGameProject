using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotDamageBuff : StatusEffect
{
    // ���� ��ġ ����
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damageTick { get; set; }
    [field: SerializeField] public TMP_Text overlapText { get; private set; }

    [field: SerializeField] public float tick { get; set; } = 0.1f;
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

    public override void Progress()
    {
        tick -= Time.deltaTime;
        if(tick < 0)
        {
            DealDamageToTarget(damageTick);
            tick += 0.1f;
        }
    }

    public override void Overlap()      //���ӽð� ����
    {
        // ��ø 
        overlap = overlap < DefaultMaxOverlap ? overlap + 1 : DefaultMaxOverlap;
        overlapText.text = overlap > 1 ? overlap.ToString() : null;

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;
        //print(duration);
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damageTick);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
        objectBasic.Damaged(damage * overlap);
    }
}
