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
        ObjectBasic objectBasic = target.GetComponent<ObjectBasic>();
        Stats stats = target.GetComponent<Stats>();

        maxOverlap = DefaultMaxOverlap + (int)(stats.SEResist[(int)buffType].Value * 10);

        overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;
        overlapText.text = overlap > 1 ? overlap.ToString() : null;
        

        duration = defaultDuration;

        if (overlap == maxOverlap)
        {
            Debug.Log(target.name +":����!");
            if (target.tag == "Player" || target.tag == "Npc")
            {
                objectBasic.Damaged(objectBasic.stats.HPMax * damagePer);

            }
            else if (target.tag == "Enemy")
            {
                
                // �ִ� ü���� 1000 �̻��̶��
                if(objectBasic.stats.HPMax >= 1000)
                {
                    objectBasic.Damaged(1000 * damagePer);
                }
                else
                {
                    objectBasic.Damaged(objectBasic.stats.HPMax * damagePer);
                }
                
            }

            GameObject BleedObject = ObjectPoolManager.instance.Get("Bleeding");
            BleedObject.transform.position = objectBasic.CenterPivot.transform.position;
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
