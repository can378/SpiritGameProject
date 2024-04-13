using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public int skillID { get; private set; }
    [field: SerializeField] public int[] skillLimit { get; private set; }               //�ٰŸ� : 0 - ����, 1 - ���, 2 - �ֵθ���
                                                                                        //���Ÿ� : 10 - ��, 11 - Ȱ, 12 - ������, 13 - ���� ����

    [field: SerializeField] public float preDelay { get; private set; }                 // ��ų ��� �� ��� �ð�
    [field: SerializeField] public float maxHoldTime { get; private set; }              // Ȧ�� �ִ� �����ð�
    [field: SerializeField] public float postDelay { get; private set; }                // ��ų ��� �� ��� �ð�

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //�⺻ ��� �ð�
    [field: SerializeField] public float skillCoolTime { get; set; }                    //���� ��� �ð�

    [field: SerializeField] public GameObject user { get; set; }                        //�����

    void Update() {
        CoolDown();
    }

    public abstract void Enter(GameObject user);                                          //���
    public abstract void Exit(GameObject user);                                         //��ų ��� ��

    void CoolDown()
    {
        if(skillCoolTime == 0)
        {
            return;
        }

        skillCoolTime -= Time.deltaTime;
        skillCoolTime = Mathf.Clamp(skillCoolTime,0,skillDefalutCoolTime * 2);
    }



}
