using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE {DOWN, HOLD, UP}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public int skillType { get; private set; }                  // ��ų ���� Ÿ�̹�
                                                                                        //��ų Ű Down : 0
                                                                                        //��ų Ű Hold : 1
                                                                                        //��ų Ű Up   : 2
                                                                                        
    [field: SerializeField] public int[] skillLimit { get; private set; }               //�ٰŸ� : 0 - ����, 1 - ���, 2 - �ֵθ���
                                                                                        //���Ÿ� : 10 - ��, 11 - Ȱ, 12 - ������, 13 - ���� ����

    [field: SerializeField] public float maxHoldTime { get; private set; }              // Ȧ�� �ִ� �����ð�
    [field: SerializeField] public float preDelay { get; private set; }                 // ��ų �ߵ� �� ��� �ð�
    [field: SerializeField] public float postDelay { get; private set; }                // ��ų �ߵ� �� ��� �ð�

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //�⺻ ��� �ð�
    [field: SerializeField] public float skillCoolTime { get; set; }                    //���� ��� �ð�

    [field: SerializeField] public ObjectBasic user { get; set; }                        //�����

    void Update() 
    {
        CoolDown();
    }

    public virtual void Enter(ObjectBasic user)
    {
        this.user = user;
        print(user.name + " : " + this.name);
    }

    public virtual void Cancle()
    {
        if(user.tag == "Player")
        {
            PlayerStats playerStats = this.user.GetComponent<PlayerStats>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - playerStats.skillCoolTime) * skillDefalutCoolTime;
        }
        else if (user.tag == "Enemy")
        {
            // ��Ÿ�� ����
            skillCoolTime = skillDefalutCoolTime;
        }
    }

    public abstract void Exit();                                                            //��ų ��� ��

    void CoolDown()
    {
        if(skillCoolTime == 0)
        {
            return;
        }

        skillCoolTime -= Time.deltaTime;
        skillCoolTime = Mathf.Clamp(skillCoolTime, 0, skillDefalutCoolTime * 2);
    }

    public void HoldCoolDown()
    {
        skillCoolTime = skillDefalutCoolTime * 2;
    }


}
