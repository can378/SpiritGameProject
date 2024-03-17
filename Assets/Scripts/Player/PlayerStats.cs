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
    public float increasedCritical { get; set; }
    public float addCritical { get; set; }
    public float critical {
        get { return (1 + increasedCritical) * (0 + addCritical); }
    }

    //CriticalDamage
    public float increasedCriticalDamage { get; set; }
    public float addCriticalDamage { get; set; }
    public float criticalDamage
    {
        get { return (1 + increasedCriticalDamage) * (0.5f + addCriticalDamage); }
    }

    //Drain
    public float increasedDrain {  get; set; }
    public float addDrain {  get; set; }
    public float drain
    {
        get { return (1 + increasedDrain) * (0 + addDrain); }
    }

    // attackSpeed
    public float increasedAttackSpeed {  get; set; }
    public float addAttackSpeed {  get; set; }
    public float attackSpeed
    {
        get { return (1 + increasedAttackSpeed) * (1 + addAttackSpeed); }
    }

    //Skill
    //SkillPower
    public float increasedSkillPower {  get; set; }
    public float addSkillPower {  get; set; }
    public float skillPower
    {
        get { return (1 + increasedSkillPower) * (1 + addSkillPower); }
    }

    //SkillCoolTime 스킬 쿨타임 감소량
    public float increasedSkillCoolTime {  get; set; }
    public float addSkillCoolTime {  get; set; }
    public float skillCoolTime
    {
        get { return (1 + increasedSkillCoolTime) * (0 + addSkillCoolTime); }
    }

    //Move
    //RunSpeed
    public float increasedRunSpeed {  get; set; }
    public float addRunSpeed {  get; set; }
    public float runSpeed
    {
        get { return (1 + increasedRunSpeed) * (1.66f + addRunSpeed); }
    }
    //RunCoolTime
    public float decreasedRunCoolTime {  get; set; }
    public float subRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get { return (1 - increasedRunSpeed) * (5 - addRunSpeed); }
    }

    //Dodge
    // 회피 속도
    public float increasedDodgeSpeed {  get; set; }
    public float addDodgeSpeed {  get; set; }
    public float dodgeSpeed
    {
        get { return (1 + increasedRunSpeed) * (1 + addRunSpeed); }
    }

    //회피 시간
    public float increasedDodgeTime {  get; set; }
    public float addDodgeTime {  get; set; }
    public float dodgeTime
    {
        get { return (1 + increasedRunSpeed) * (0.5f + addRunSpeed); }
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
