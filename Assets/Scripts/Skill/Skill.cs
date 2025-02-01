using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE {DOWN, HOLD, UP}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public int skillType { get; private set; }                  // 스킬 시전 타이밍
                                                                                        //스킬 키 Down : 0
                                                                                        //스킬 키 Hold : 1
                                                                                        //스킬 키 Up   : 2
                                                                                        
    [field: SerializeField] public int[] skillLimit { get; private set; }               //근거리 : 0 - 베기, 1 - 찌르기, 2 - 휘두르기
                                                                                        //원거리 : 10 - 총, 11 - 활, 12 - 던지기, 13 - 범위 공격

    [field: SerializeField] public float maxHoldTime { get; private set; }              // 홀드 최대 유지시간
    [field: SerializeField] public float preDelay { get; private set; }                 // 스킬 발동 전 대기 시간
    [field: SerializeField] public float postDelay { get; private set; }                // 스킬 발동 후 대기 시간

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //기본 대기 시간
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

    public virtual void Cancle()
    {
        if(user.tag == "Player")
        {
            PlayerStats playerStats = this.user.GetComponent<PlayerStats>();

            // 쿨타임 적용
            skillCoolTime = (1 - playerStats.skillCoolTime) * skillDefalutCoolTime;
        }
        else if (user.tag == "Enemy")
        {
            // 쿨타임 적용
            skillCoolTime = skillDefalutCoolTime;
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
        skillCoolTime = Mathf.Clamp(skillCoolTime, 0, skillDefalutCoolTime * 2);
    }

    public void HoldCoolDown()
    {
        skillCoolTime = skillDefalutCoolTime * 2;
    }


}
