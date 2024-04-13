using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public int skillID { get; private set; }
    [field: SerializeField] public int[] skillLimit { get; private set; }               //근거리 : 0 - 베기, 1 - 찌르기, 2 - 휘두르기
                                                                                        //원거리 : 10 - 총, 11 - 활, 12 - 던지기, 13 - 범위 공격

    [field: SerializeField] public float preDelay { get; private set; }                 // 스킬 사용 전 대기 시간
    [field: SerializeField] public float maxHoldTime { get; private set; }              // 홀드 최대 유지시간
    [field: SerializeField] public float postDelay { get; private set; }                // 스킬 사용 후 대기 시간

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //기본 대기 시간
    [field: SerializeField] public float skillCoolTime { get; set; }                    //현재 대기 시간

    [field: SerializeField] public GameObject user { get; set; }                        //사용자

    void Update() {
        CoolDown();
    }

    public abstract void Enter(GameObject user);                                          //사용
    public abstract void Exit(GameObject user);                                         //스킬 사용 끝

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
