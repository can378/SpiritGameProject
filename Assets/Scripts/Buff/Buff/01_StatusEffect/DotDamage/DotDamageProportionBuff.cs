using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/DotDamageP")]

public class DotDamageProportionBuff : BuffData
{
    // 고정 수치 피해
    // dotDamage가 양수이면 피해
    // 음수이면 힐
    [field: SerializeField] public float damagePerTick { get; set; }       // 틱당 피해량

    [field: SerializeField] public float tick { get; set; } = 0.1f;     // 틱 간격
    public override void Apply(Buff _Buff)
    {
        // 주기적으로 피해를 입히는 코루틴 시작
        _Buff.CustomData["tickTimer"] = 0f;
        Overlap(_Buff);
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void Remove(Buff _Buff)
    {
        // 디버프 종료 시 피해 코루틴 중단
        //Debug.Log("DoT debuff removed");
    }

    public override void Update_Buff(Buff _Buff)
    {
        float timer = (float)_Buff.CustomData["tickTimer"];
        timer += Time.deltaTime;

        if (timer >= tick)
        {
            timer -= tick;

            DealDamageToTarget(_Buff, damagePerTick);
        }
        _Buff.CustomData["tickTimer"] = timer;

    }


    public override void Overlap(Buff _Buff)      //지속시간 갱신
    {
        // 중첩 
        //stack = stack < DefaultMaxOverlap ? stack + 1 : DefaultMaxOverlap;
        _Buff.AddStack();

        Stats stats = _Buff.target.GetComponent<Stats>();
        _Buff.curDuration = _Buff.duration = (1 - stats.SEResist[(int)buffType].Value) * defaultDuration;
        //print(duration);
    }

    /*
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
    */

    private void DealDamageToTarget(Buff _Buff, float damage)
    {
        ObjectBasic objectBasic = _Buff.target.GetComponent<ObjectBasic>();
        objectBasic.Damaged(objectBasic.stats.HPMax.Value * damage * (float)_Buff.stack);
    }
}
