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
        duration = (1 - stats.SEResist) * defaultDuration;
        if (overlap == maxOverlap)
        {
            duration = 0;
        }
    }
    public override void RemoveEffect()    //����
    {
        if(overlap == maxOverlap)
        {
            Debug.Log(target.name +":����!");
            if (target.tag == "Player" || target.tag == "Npc")
            {
                ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
                objectBasic.Damaged(objectBasic.stats.HPMax * (1 - objectBasic.stats.SEResist) * damagePer);
            }
            else if (target.tag == "Enemy")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                enemy.Damaged(enemy.stats.HPMax * (1 - enemy.stats.SEResist) * damagePer);
            }
        }
        
    }
}
