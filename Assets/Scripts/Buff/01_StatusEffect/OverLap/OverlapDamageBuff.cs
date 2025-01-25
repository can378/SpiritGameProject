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
    // �ִ� ��ø �� ���ظ� �ְ� ���ŵǴ� �����
    // ���׿� ���� ���ط��� ����
    
    public override void Apply()     //�߰�
    {
        Overlap();
    }
    public override void Overlap()     //����
    {
        Stats stats = target.GetComponent<Stats>();

        maxOverlap = DefaultMaxOverlap + (int)(stats.SEResist((int)buffType) * 10);

        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;
        overlapText.text = overlap > 1 ? overlap.ToString() : null;
        

        duration = defaultDuration;

        if (overlap == maxOverlap)
        {
            Debug.Log(target.name +":����!");
            if (target.tag == "Player" || target.tag == "Npc")
            {
                ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
                objectBasic.Damaged(objectBasic.stats.HPMax * damagePer);

            }
            else if (target.tag == "Enemy")
            {
                EnemyBasic enemy = target.GetComponent<EnemyBasic>();
                
                // �ִ� ü���� 1000 �̻��̶��
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
    public override void Remove()    //����
    {
           
    }
}
