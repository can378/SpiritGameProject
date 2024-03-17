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

    //Critical
    public float defaultCritical = 0;
    public float increasedCritical { get; set; }
    public float addCritical { get; set; }
    public float critical {
        get { return (1 + increasedCritical) * (defaultCritical + addCritical); }
    }

    //CriticalDamage
    public float defaultCriticalDamage = 0.5f;
    public float increasedCriticalDamage { get; set; }
    public float addCriticalDamage { get; set; }
    public float criticalDamage
    {
        get { return (1 + increasedCriticalDamage) * (0.5f + addCriticalDamage); }
    }

    //Drain
    public float defaultDrain = 0;
    public float increasedDrain {  get; set; }
    public float addDrain {  get; set; }
    public float drain
    {
        get { return (1 + increasedDrain) * (0 + addDrain); }
    }

    // attackSpeed
    // 높을수록 빨라짐
    public float defaultAttackSpeed = 1;
    public float increasedAttackSpeed {  get; set; }
    public float addAttackSpeed {  get; set; }
    public float attackSpeed
    {
        get { return (1 + increasedAttackSpeed) * (defaultAttackSpeed + addAttackSpeed); }
    }

    //Skill
    //SkillPower
    public float defaultSkillPower = 0;
    public float increasedSkillPower {  get; set; }
    public float addSkillPower {  get; set; }
    public float skillPower
    {
        get { return (1 + increasedSkillPower) * (defaultSkillPower + addSkillPower); }
    }

    //SkillCoolTime
    // 높을수록 스킬 자주 사용 가능
    public float defaultSkillCoolTime = 0;
    public float increasedSkillCoolTime {  get; set; }
    public float addSkillCoolTime {  get; set; }
    public float skillCoolTime
    {
        get { return (1 + increasedSkillCoolTime) * (0 + addSkillCoolTime); }
    }

    //Move
    //RunSpeed
    public float defaultRunSpeed = 1.66f;
    public float increasedRunSpeed {  get; set; }
    public float addRunSpeed {  get; set; }
    public float runSpeed
    {
        get { return (1 + increasedRunSpeed) * (defaultRunSpeed + addRunSpeed); }
    }
    //RunCoolTime
    public float defaultRunCoolTime = 5;
    public float decreasedRunCoolTime {  get; set; }
    public float subRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get { return (1 - decreasedRunCoolTime) * (defaultRunCoolTime - subRunCoolTime); }
    }

    //Dodge
    // 회피 속도
    public float defaultDodgeSpeed = 2;
    public float increasedDodgeSpeed {  get; set; }
    public float addDodgeSpeed {  get; set; }
    public float dodgeSpeed
    {
        get { return (1 + increasedDodgeSpeed) * (defaultDodgeSpeed + addDodgeSpeed); }
    }

    //회피 시간
    public float defaultDodgeTime = 0.6f;
    public float increasedDodgeTime {  get; set; }
    public float addDodgeTime {  get; set; }
    public float dodgeTime
    {
        get { return (1 + increasedDodgeTime) * (defaultDodgeTime + addDodgeTime); }
    }

    //Item
    public int coin = 0;
    public int key = 0;
    public int dice = 0;
    public string item = "";

    //Equipments
    public Weapon weapon = null;
    public Skill skill = null;
    public int maxEquipment = 3;
    public Equipment[] equipments = new Equipment[3];

    //Stat
    public int[] playerStat = new int[8];
}
