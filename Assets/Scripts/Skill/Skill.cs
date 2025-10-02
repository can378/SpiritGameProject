using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase : MonoBehaviour
{
    [HideInInspector] public SkillData skillData { get; protected set; }

    [field: SerializeField] public float skillCoolTime { get; set; }                    //현재 대기 시간

    [field: SerializeField] public ObjectBasic user { get; set; }                        //사용자

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

            // 쿨타임 적용
            skillCoolTime = (1 - playerStats.skillCoolTime) * skillData.skillDefalutCoolTime;
        }
        else if (user.tag == "Enemy")
        {
            // 쿨타임 적용
            skillCoolTime = skillData.skillDefalutCoolTime;
        }
    }

    public abstract void Exit();                                                            //스킬 사용 끝

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
