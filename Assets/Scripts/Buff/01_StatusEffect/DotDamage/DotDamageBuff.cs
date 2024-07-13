using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotDamageBuff : StatusEffect
{
    // ���� ��ġ ����
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damageSecond { get; set; }
    [field: SerializeField] public TMP_Text overlapText { get; private set; }
    public override void ApplyEffect()
    {
        // �ֱ������� ���ظ� ������ �ڷ�ƾ ����
        ResetEffect();
        StartCoroutine(InflictDamageOverTime());
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // ����� ���� �� ���� �ڷ�ƾ �ߴ�
        //Debug.Log("DoT debuff removed");
    }

    public override void ResetEffect()      //���ӽð� ����
    {
        // ��ø 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;
        overlapText.text = overlap > 1 ? overlap.ToString() : null;

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;
        //print(duration);
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damageSecond);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        if (target.tag == "Player" || target.tag == "Npc")
        {
            ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
            objectBasic.Damaged(damage * overlap);
        }
        else if(target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();
            enemy.Damaged(damage * overlap);
        }
    }
}
