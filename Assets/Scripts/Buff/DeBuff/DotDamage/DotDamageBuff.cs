using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageBuff : StatusEffect
{
    // 고정 수치 피해
    // dotDamage가 양수이면 피해
    // 음수이면 힐
    [field: SerializeField] public float damageSecond { get; set; }

    public override void ApplyEffect()
    {
        // 주기적으로 피해를 입히는 코루틴 시작
        ResetEffect();
        StartCoroutine(InflictDamageOverTime());
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void RemoveEffect()
    {
        // 디버프 종료 시 피해 코루틴 중단
        //Debug.Log("DoT debuff removed");
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        // 중첩 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - stats.SEResist) * defaultDuration;
        //print(duration);
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (duration > 0)
        {
            // 피해를 입히는 부분
            DealDamageToTarget(damageSecond);

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        if(target.tag == "Player")
        {
            Player player = target.GetComponent<Player>();
            player.Damaged(damage * overlap);
        }
        else if(target.tag == "Enemy")
        {
            EnemyBasic enemy = target.GetComponent<EnemyBasic>();
            List<int> attributes = new List<int>();
            attributes.Add(0);
            enemy.Damaged(damage * overlap);
        }
    }
}
