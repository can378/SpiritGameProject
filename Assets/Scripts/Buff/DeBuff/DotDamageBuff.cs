using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageBuff : StatusEffect
{
    // �ִ� ü�� ��� %��ġ
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damagePerSecond { get; set; }

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

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - (stats.resist[resist] * 2)) * defaultDuration;
        //print(duration);
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damagePerSecond);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();
            player.Damaged(player.stats.HPMax * damage * overlap);
        }
        else if(target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();
            List<int> attributes = new List<int>();
            attributes.Add(0);
            enemy.Damaged(enemy.stats.HPMax * damage * overlap);
        }
    }
}
