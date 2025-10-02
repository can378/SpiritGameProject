using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector] public SkillData skillData { get; protected set; }

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

    public bool IsSkillUse()
    {
        if (skillCoolTime <= 0)
            return true;
        return false;
    }

    public virtual void Cancle()
    {
        if (user.tag == "Player")
        {
            PlayerStats playerStats = this.user.GetComponent<PlayerStats>();

            // ��Ÿ�� ����
            skillCoolTime = (1 - playerStats.skillCoolTime) * skillData.skillDefalutCoolTime;
        }
        else if (user.tag == "Enemy")
        {
            // ��Ÿ�� ����
            skillCoolTime = skillData.skillDefalutCoolTime;
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
        skillCoolTime = Mathf.Clamp(skillCoolTime, 0, skillData.skillDefalutCoolTime * 2);
    }

    public void HoldCoolDown()
    {
        skillCoolTime = skillData.skillDefalutCoolTime * 2;
    }


}
