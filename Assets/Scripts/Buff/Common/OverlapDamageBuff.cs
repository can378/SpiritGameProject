using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapDamageBuff : StatusEffect
{
    [field: SerializeField] public float damagePer { get; set; }
    //최대 중첩 시 피해를 주고 제거되는 디버프
    
    public override void ApplyEffect()     //추가
    {
        ResetEffect();
    }
    public override void ResetEffect()     //갱신
    {
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        Stats stats = target.GetComponent<Stats>();
        duration = (1 - (stats.resist[resist] * 2)) * defaultDuration;

        if (overlap == maxOverlap)
        {
            duration = 0;
        }
    }
    public override void RemoveEffect()    //제거
    {
        if(overlap == maxOverlap)
        {
            if (target.tag == "Player")
            {
                Player player = target.GetComponent<Player>();
                player.Damaged(player.stats.HPMax * damagePer);
            }
            else if (target.tag == "Enemy")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                List<int> attributes = new List<int>();
                attributes.Add(0);
                enemy.Damaged(enemy.stats.HPMax * damagePer);
            }
        }
        
    }
}
