using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE { DOWN, HOLD, UP }


public class SkillData : ItemData
{
    [field: SerializeField] public SKILL_TYPE skillType { get; private set; }                  // 스킬 시전 타이밍
                                                                                               //스킬 키 Down : 0
                                                                                               //스킬 키 Hold : 1
                                                                                               //스킬 키 Up   : 2

    [field: SerializeField] public WEAPON_TYPE[] skillLimit { get; private set; }       // 해당 무기를 가지고 있을 때만 스킬 사용이 가능하다.
                                                                                        // 없다면 제한이 없는 스킬이다.                                                                            

    [field: SerializeField] public float maxHoldTime { get; private set; }              // 홀드 최대 유지시간
    [field: SerializeField] public float preDelay { get; private set; }                 // 스킬 발동 전 대기 시간
    [field: SerializeField] public float postDelay { get; private set; }                // 스킬 발동 후 대기 시간

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //기본 대기 시간
    [field: SerializeField] public Dictionary<string, object> CustomData { get; private set; }

    public string TypeToKorean()
    {
        {
            switch (skillType)
            {
                case SKILL_TYPE.UP:
                    return "즉시";
                case SKILL_TYPE.HOLD:
                    return "지속";
                case SKILL_TYPE.DOWN:
                    return "집중";
                default:
                    return "???";
            }
        }
    }

    public virtual float DamageText(Stats _Stats)
    {
        return 0;
    }
}

