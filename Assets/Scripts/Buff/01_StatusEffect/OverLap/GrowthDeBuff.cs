using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthDeBuff : StatusEffect
{
    [field: SerializeField] public float increasedStat { get; set; }
    [field:SerializeField] public int addDeBuff {get; set; }
    // 최대 중첩 시 피해를 주고 제거되는 디버프 
    // 저항에 따라 피해량이 감소

    public override void ApplyEffect()     //추가
    {
        ResetEffect();
    }
    public override void ResetEffect()     //갱신
    {
        Stats stats = target.GetComponent<Stats>();

        // 효과 잠시 제거
        stats.increasedAttackPower -= overlap * increasedStat;
        stats.increasedMoveSpeed -= overlap * increasedStat;
        target.transform.localScale = new Vector3(1,1,1);

        // 중첩 
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;
        if (overlap == maxOverlap)
        {
            duration = 0;
            return;
        }

        // 저항에 따른 지속시간 적용
        duration = (1 - stats.SEResist(buffId)) * defaultDuration;

        stats.increasedAttackPower += overlap * increasedStat;
        stats.increasedMoveSpeed += overlap * increasedStat;
        target.transform.localScale = new Vector3(1 + overlap * increasedStat, 1 + overlap * increasedStat, 1 + overlap * increasedStat);

    }
    
    public override void RemoveEffect()    //제거
    {
        if (overlap == maxOverlap)
        {
            if (target.tag == "Player")
            {
                Player player = target.GetComponent<Player>();
                player.ApplyBuff(addDeBuff);
            }
            else if (target.tag == "Enemy" || target.tag == "Npc")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                enemy.ApplyBuff(addDeBuff);
            }
        }

    }
}
