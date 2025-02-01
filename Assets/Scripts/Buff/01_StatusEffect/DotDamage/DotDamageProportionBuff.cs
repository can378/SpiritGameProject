using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageProportionBuff : StatusEffect
{
    // 최대 체력 비례 %수치
    // dotDamage가 양수이면 피해
    // 음수이면 힐
    [field: SerializeField] public float damagePerTick { get; set; }
    float tick = 0.1f;

    public override void Apply()
    {
        // 주기적으로 피해를 입히는 코루틴 시작
        Overlap();
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void Remove()
    {
        // 디버프 종료 시 피해 코루틴 중단
        //Debug.Log("DoT debuff removed");
    }

    public override void Overlap()      //지속시간 갱신
    {
        // 중첩 
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
            // 피해를 입히는 부분
            DealDamageToTarget(damagePerTick);

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
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
