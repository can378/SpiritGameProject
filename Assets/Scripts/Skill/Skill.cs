using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillLimit {None, Melee, Shot}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public int skillID { get; private set; }
    [field: SerializeField] public SkillLimit skillLimit { get; private set; }
    [field: SerializeField] public int skillType { get; private set; }                  // 0이면 즉발, 1이면 준비, 2이면 홀드

    [field: SerializeField] public float preDelay { get; private set; }                 //스킬 사용 전 대기 시간
    [field: SerializeField] public float rate { get; private set; }                     //스킬 사용 시간
    [field: SerializeField] public float postDelay { get; private set; }                //스킬 사용 후 대기 시간

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //기본 대기 시간
    [field: SerializeField] public float skillCoolTime { get; set; }                    //현재 대기 시간

    [field: SerializeField] public GameObject user { get; set; }                        //사용자

    void Update() {
        CoolDown();
    }

    public abstract void Use(GameObject user);                                          //사용
    public abstract void Exit(GameObject user);                                         //스킬 사용 끝

    void CoolDown()
    {
        skillCoolTime -= Time.deltaTime;
    }



}
