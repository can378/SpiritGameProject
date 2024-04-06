using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapDamageBuff : StatusEffect
{
    [field: SerializeField] public float damagePer { get; set; }
    // �ִ� ��ø �� ���ظ� �ְ� ���ŵǴ� �����
    // ���׿� ���� ���ط��� ����
    
    public override void ApplyEffect()     //�߰�
    {
        ResetEffect();
    }
    public override void ResetEffect()     //����
    {
        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

        Stats stats = target.GetComponent<Stats>();
        duration = stats.SEResist * defaultDuration;
        if (overlap == maxOverlap)
        {
            duration = 0;
        }
    }
    public override void RemoveEffect()    //����
    {
        if(overlap == maxOverlap)
        {
            if (target.tag == "Player")
            {
                Player player = target.GetComponent<Player>();
                player.Damaged(player.stats.HPMax * player.stats.SEResist * damagePer);
            }
            else if (target.tag == "Enemy")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                List<int> attributes = new List<int>();
                attributes.Add(0);
                enemy.Damaged(enemy.stats.HPMax * enemy.stats.SEResist * damagePer);
            }
        }
        
    }
}
