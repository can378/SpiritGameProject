using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff/DotDamageP")]

public class DotDamageProportionBuff : BuffData
{
    // ���� ��ġ ����
    // dotDamage�� ����̸� ����
    // �����̸� ��
    [field: SerializeField] public float damagePerTick { get; set; }       // ƽ�� ���ط�

    [field: SerializeField] public float tick { get; set; } = 0.1f;     // ƽ ����
    public override void Apply(Buff _Buff)
    {
        // �ֱ������� ���ظ� ������ �ڷ�ƾ ����
        _Buff.CustomData["tickTimer"] = 0f;
        Overlap(_Buff);
        //Debug.Log("DoT debuff applied: " + damagePerSecond + " damage per second");
    }

    public override void Remove(Buff _Buff)
    {
        // ����� ���� �� ���� �ڷ�ƾ �ߴ�
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
            _Buff.CustomData["tickTimer"] = timer;
        }
    }


    public override void Overlap(Buff _Buff)      //���ӽð� ����
    {
        // ��ø 
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
            // ���ظ� ������ �κ�
            DealDamageToTarget(damageTick);

            // �ֱ������� ���ظ� ������ ����(��: 1��)�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
        }
    }
    */

    private void DealDamageToTarget(Buff _Buff, float damage)
    {
        ObjectBasic objectBasic = _Buff.target.GetComponent<ObjectBasic>();
        objectBasic.Damaged(objectBasic.stats.HPMax.Value * damage * _Buff.stack);
    }
}
