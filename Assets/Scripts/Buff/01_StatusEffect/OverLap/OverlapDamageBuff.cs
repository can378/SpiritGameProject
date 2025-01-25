using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class OverlapDamageBuff : StatusEffect
{
    [field: SerializeField] public int maxOverlap { get; set; }
    [field: SerializeField] public float damagePer { get; set; }
    public TMP_Text overlapText;
    // 최대 중첩 시 피해를 주고 제거되는 디버프
    // 저항에 따라 피해량이 감소
    
    public override void Apply()     //추가
    {
        Overlap();
    }
    public override void Overlap()     //갱신
    {
        Stats stats = target.GetComponent<Stats>();

        maxOverlap = DefaultMaxOverlap + (int)(stats.SEResist((int)buffType) * 10);

        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;
        overlapText.text = overlap > 1 ? overlap.ToString() : null;
        

        duration = defaultDuration;

        if (overlap == maxOverlap)
        {
            Debug.Log(target.name +":출혈!");
            if (target.tag == "Player" || target.tag == "Npc")
            {
                ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
                objectBasic.Damaged(objectBasic.stats.HPMax * damagePer);

            }
            else if (target.tag == "Enemy")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                
                // 최대 체력이 1000 이상이라면
                if(enemy.stats.HPMax >= 1000)
                {
                    enemy.Damaged(1000 * damagePer);
                }
                else
                {
                    enemy.Damaged(enemy.stats.HPMax * damagePer);
                }
                
            }

            GameObject BleedObject = ObjectPoolManager.instance.Get2("Bleeding");
            BleedObject.transform.position = target.transform.position;
            BleedObject.transform.localScale = Vector3.one * 3;

            duration = 0;
        }
    }

    public override void Progress()
    {
        
    }
    public override void Remove()    //제거
    {
           
    }
}
