using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    //EXP
    public int level = 1;
    public int exp = 1;
    public int point = 10;

    //Attack

    // Critical
    // UI : 치명타 확률 0%
    // 치명타 = 무작위(0 ~ 100) < 치명타 확률 * 100 ? 참 : 거짓
    // 최소 0%, 최대 100%
    public float defaultCriticalChance = 0;
    public float addCriticalChance { get; set; }
    public float increasedCriticalChance { get; set; }
    public float decreasedCriticalChance { get; set; }
    public float criticalChance
    {
        get
        {
            float CC = (defaultCriticalChance + addCriticalChance) * (1f + increasedCriticalChance) * (1f - decreasedCriticalChance);
            if (CC > 1f)
                return 1f;
            else if (CC <= 0)
                return 0;
            else
                return CC;
        }
    }

    //CriticalDamage
    // UI : 치명타 피해량 150%
    // 피해량 = 치명타 ? 치명타 피해량 * 기본 피해량 : 기본 피해량
    // 최소 100%, 최대 300%
    public float defaultCriticalDamage = 1.5f;
    public float addCriticalDamage { get; set; }
    public float increasedCriticalDamage { get; set; }
    public float decreasedCriticalDamage { get; set; }
    public float criticalDamage
    {
        get
        {
            float CD = (defaultCriticalDamage + addCriticalDamage) * (1f + increasedCriticalDamage) * (1f - decreasedCriticalDamage);
            if (CD > 3f)
                return 3f;
            else if (CD <= 1)
                return 1;
            else
                return CD;
        }
    }

    // attackSpeed
    // UI 100%
    // 높을수록 빨라짐
    // 초당 공격속도 = 무기 초당 공격 속도 * 플레이어 공격속도
    // 최소 0%, 최대 300%
    public float defaultAttackSpeed = 1;
    public float addAttackSpeed { get; set; }
    public float increasedAttackSpeed {  get; set; }
    public float decreasedAttackSpeed { get; set; }
    public float attackSpeed
    {
        get
        {
            float AS = (defaultAttackSpeed + addAttackSpeed) * (1f + increasedAttackSpeed) * (1f - decreasedAttackSpeed);
            if (AS > 3f)
                return 3f;
            else if (AS <= 0)
                return 0;
            else
                return AS;
        }
    }

    // Skill
    // SkillPower
    // UI 도력 0
    // 도술 피해량 = 도술 기본 피해량 + 도력 * 스킬 계수
    // 최소 0
    public float defaultSkillPower = 0;
    public float addSkillPower { get; set; }
    public float increasedSkillPower { get; set; }
    public float decreasedSkillPower { get; set; }
    public float skillPower
    {
        get
        {
            float SP = (defaultSkillPower + addSkillPower) * (1f + increasedSkillPower) * (1f - decreasedSkillPower);
            if (SP <= 0)
                return 0;
            return SP;
        }
    }

    // SkillCoolTime
    // 도술 재사용 대기 시간 감소
    // UI 재사용 대기 시간 감소 0%
    // 낮을수록 도술 자수 사용 가능
    // 도술 재사용 대기 시간 = 도술 기본 재사용 대기 시간 * 재사용 대기시간
    // 최소 -80% ,최대 80%
    public float defaultSkillCoolTime = 0;
    public float addSkillCoolTime { get; set; }
    public float increasedSkillCoolTime { get; set; }
    public float decreasedSkillCoolTime { get; set; }
    public float skillCoolTime
    {
        get
        {
            float SCT = (defaultSkillCoolTime + addSkillCoolTime) * (1f + increasedSkillCoolTime) * (1f - decreasedSkillCoolTime);
            if (SCT > 0.8f)
                return 0.8f;
            else if (SCT <= -0.8f)
                return -0.8f;
            else
                return SCT;
        }
    }

    // Move
    // RunSpeed
    // UI 달리기 시 이동속도 166%
    // 달리기 시 속도
    // 이동속도 = 이동속도 (달리기 ? 달리기 시 속도 : 1)
    // 최소 100%
    public float defaultRunSpeed = 1.66f;
    public float addRunSpeed { get; set; }
    public float increasedRunSpeed {  get; set; }
    public float decreasedRunSpeed { get; set; }
    public float runSpeed
    {
        get
        {
            float RP = (defaultRunSpeed + addRunSpeed) * (1f + increasedRunSpeed) * (1f - decreasedRunSpeed);
            if (RP <= 1.0f)
                return 1.0f;
            return RP;
        }
    }

    //RunCoolTime
    // 달리기 재사용 대기 시간
    // UI : 달리기 재사용 대기 시간 5초
    // 달리기 재사용 대기 시간 = 달리기 재사용 대기 시간
    // 최소 0초
    public float defaultRunCoolTime = 5f;
    public float addRunCoolTime { get; set; }
    public float increasedRunCoolTime { get; set; }
    public float decreasedRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get
        {
            float RCT = (defaultRunCoolTime + addRunCoolTime) * (1f + increasedRunCoolTime) * (1f - decreasedRunCoolTime);
            if (RCT <= 0f)
                return 0f;
            return RCT;
        }
    }

    // Dodge
    // 회피 속도
    // UI : 회피 시 이동 속도 200%
    // 최소 50%
    public float defaultDodgeSpeed = 2;
    public float addDodgeSpeed { get; set; }
    public float increasedDodgeSpeed {  get; set; }
    public float decreasedDodgeSpeed { get; set; }
    public float dodgeSpeed
    {
        get
        {
            float DS = (defaultDodgeSpeed + addDodgeSpeed) * (1f + increasedDodgeSpeed) * (1f - decreasedDodgeSpeed);
            if (DS <= 0.5f)
                return 0.5f;
            return DS;
        }
    }

    // 회피 시간
    // UI : 회피 시간 0.6초
    // 회피 시간 = 회피 시간
    // 최소 0.2초
    public float defaultDodgeTime = 0.6f;
    public float addDodgeTime { get; set; }
    public float increasedDodgeTime {  get; set; }
    public float decreasedDodgeTime { get; set; }
    public float dodgeTime
    {
        get
        {
            float DT = (defaultDodgeTime + addDodgeTime) * (1f + increasedDodgeTime) * (1f - decreasedDodgeTime);
            if (DT <= 0.2f)
                return 0.2f;
            return DT;
        }
    }

    //Item
    public int coin = 0;
    public int key = 0;
    public int dice = 0;
    public string item = "";

    //Equipments
    public int weapon = 0;

    public int maxSkillSlot = 1;
    public int[] skill = {0, 0, 0, 0, 0};

    public int maxEquipment = 3;
    public Equipment[] equipments = new Equipment[3];

    //Stat
    public int[] playerStat = new int[8];
}
