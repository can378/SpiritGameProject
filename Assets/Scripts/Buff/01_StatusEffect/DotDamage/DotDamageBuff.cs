using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotDamageBuff : StatusEffect
{
    // 고정 수치 피해
    // dotDamage가 양수이면 피해
    // 음수이면 힐
    [field: SerializeField] public float damageTick { get; set; }
    [field: SerializeField] public TMP_Text overlapText { get; private set; }

    [field: SerializeField] public float tick { get; set; } = 0.1f;
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

    public override void Progress()
    {
        tick -= Time.deltaTime;
        if(tick < 0)
        {
            DealDamageToTarget(damageTick);
            tick += 0.1f;
        }
    }

    public override void Overlap()      //지속시간 갱신
    {
        // 중첩 
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
            // 피해를 입히는 부분
            DealDamageToTarget(damageTick);

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
            yield return new WaitForSeconds(1f);
        }
    }

    private void DealDamageToTarget(float damage)
    {
        ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
        objectBasic.Damaged(damage * overlap);
    }
}
