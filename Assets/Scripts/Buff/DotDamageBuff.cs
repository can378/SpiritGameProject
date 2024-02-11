using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageBuff : StatusEffect
{
    // ���� %��ġ
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damagePerSecond { get; set; }
    private Coroutine dotDamageCoroutine;

    public override void ApplyEffect()
    {
        // �ֱ������� ���ظ� ������ �ڷ�ƾ ����
        dotDamageCoroutine = StartCoroutine(InflictDamageOverTime());
        Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // ����� ���� �� ���� �ڷ�ƾ �ߴ�
        if (dotDamageCoroutine != null)
        {
            StopCoroutine(dotDamageCoroutine);
        }
        Debug.Log("DoT debuff removed");
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // ���ظ� ������ �κ�
            DealDamageToTarget(damagePerSecond);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);

            duration -= 1f;
        }
    }

    private void DealDamageToTarget(float damage)
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();
            player.Damaged(player.userData.playerHPMax * damage);
        }
        else if(target.tag == "Enemy")
        {
            EnemyStatus enemyStatus = target.GetComponent<EnemyStatus>();
            enemyStatus.health -= enemyStatus.maxHealth * damage;
        }
    }
}
