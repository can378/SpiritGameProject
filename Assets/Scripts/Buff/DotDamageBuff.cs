using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageBuff : StatusEffect
{
    // 고정 %수치
    // dotDamage가 양수이면 피해
    // 음수이면 힐
    [field: SerializeField] public float damagePerSecond { get; set; }
    private Coroutine dotDamageCoroutine;

    public override void ApplyEffect()
    {
        // 주기적으로 피해를 입히는 코루틴 시작
        dotDamageCoroutine = StartCoroutine(InflictDamageOverTime());
        Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // 디버프 종료 시 피해 코루틴 중단
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
            // 피해를 입히는 부분
            DealDamageToTarget(damagePerSecond);

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
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
