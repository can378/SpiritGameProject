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
    // UI : ġ��Ÿ Ȯ�� 0%
    // ġ��Ÿ = ������(0 ~ 100) < ġ��Ÿ Ȯ�� * 100 ? �� : ����
    public float defaultCriticalChance = 0;
    public float addCriticalChance { get; set; }
    public float increasedCriticalChance { get; set; }
    public float decreasedCriticalChance { get; set; }
    public float criticalChance
    {
        get { return (defaultCriticalChance + addCriticalChance) * (1f + increasedCriticalChance) * (1f - decreasedCriticalChance); }
    }

    //CriticalDamage
    // UI : ġ��Ÿ ���ط� 150%
    // ���ط� = ġ��Ÿ ? ġ��Ÿ ���ط� * �⺻ ���ط� : �⺻ ���ط�
    public float defaultCriticalDamage = 0.5f;
    public float addCriticalDamage { get; set; }
    public float increasedCriticalDamage { get; set; }
    public float decreasedCriticalDamage { get; set; }
    public float criticalDamage
    {
        get { return (defaultCriticalDamage + addCriticalDamage) * (1f + increasedCriticalDamage) * (1f - decreasedCriticalDamage); }
    }

    //Drain
    public float defaultDrain = 0;
    public float addDrain { get; set; }
    public float increasedDrain {  get; set; }
    public float decreasedDrain { get; set; }
    public float drain
    {
        get { return (defaultDrain + addDrain) * (1f + increasedDrain) * (1f - decreasedDrain); }
    }

    // attackSpeed
    // UI 100%
    // �������� ������
    // �ʴ� ���ݼӵ� = ���� �ʴ� ���� �ӵ� * �÷��̾� ���ݼӵ�
    public float defaultAttackSpeed = 1;
    public float addAttackSpeed { get; set; }
    public float increasedAttackSpeed {  get; set; }
    public float decreasedAttackSpeed { get; set; }
    public float attackSpeed
    {
        get { return (defaultAttackSpeed + addAttackSpeed) * (1f + increasedAttackSpeed) * (1f - decreasedAttackSpeed); }
    }

    // Skill
    // SkillPower
    // UI ���� 0
    // ���� ���ط� = ���� �⺻ ���ط� + ���� * ��ų ���
    public float defaultSkillPower = 0;
    public float addSkillPower { get; set; }
    public float increasedSkillPower { get; set; }
    public float decreasedSkillPower { get; set; }
    public float skillPower
    {
        get { return (defaultSkillPower + addSkillPower) * (1f + increasedSkillPower) * (1f - decreasedSkillPower); }
    }

    // SkillCoolTime
    // ���� ���� ��� �ð�
    // UI ���� ��� �ð� 100%
    // �������� ���� �ڼ� ��� ����
    // ���� ���� ��� �ð� = ���� �⺻ ���� ��� �ð� * ���� ���ð�
    public float defaultSkillCoolTime = 1;
    public float addSkillCoolTime { get; set; }
    public float increasedSkillCoolTime { get; set; }
    public float decreasedSkillCoolTime { get; set; }
    public float skillCoolTime
    {
        get { return (defaultSkillCoolTime + addSkillCoolTime) * (1f + increasedSkillCoolTime) * (1f - decreasedSkillCoolTime); }
    }

    // Move
    // RunSpeed
    // UI �޸��� �� �̵��ӵ� 166%
    // �޸��� �� �ӵ�
    // �̵��ӵ� = �̵��ӵ� (�޸��� ? �޸��� �� �ӵ� : 1)
    public float defaultRunSpeed = 1.66f;
    public float addRunSpeed { get; set; }
    public float increasedRunSpeed {  get; set; }
    public float decreasedRunSpeed { get; set; }
    public float runSpeed
    {
        get { return (defaultRunSpeed + addRunSpeed) * (1f + increasedRunSpeed) * (1f - decreasedRunSpeed); }
    }

    //RunCoolTime
    // �޸��� ���� ��� �ð�
    // UI : �޸��� ���� ��� �ð� 5��
    // �޸��� ���� ��� �ð� = �޸��� ���� ��� �ð�
    public float defaultRunCoolTime = 5;
    public float addRunCoolTime { get; set; }
    public float increasedRunCoolTime { get; set; }
    public float decreasedRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get { return (defaultRunCoolTime + addRunCoolTime) * (1f + increasedRunCoolTime) * (1f - decreasedRunCoolTime); }
    }

    // Dodge
    // ȸ�� �ӵ�
    // UI : ȸ�� �� �̵� �ӵ� 200%
    public float defaultDodgeSpeed = 2;
    public float addDodgeSpeed { get; set; }
    public float increasedDodgeSpeed {  get; set; }
    public float decreasedDodgeSpeed { get; set; }
    public float dodgeSpeed
    {
        get { return (defaultDodgeSpeed + addDodgeSpeed) * (1f + increasedDodgeSpeed) * (1f - decreasedDodgeSpeed); }
    }

    // ȸ�� �ð�
    // UI : ȸ�� �ð� 0.6��
    // ȸ�� �ð� = ȸ�� �ð�
    public float defaultDodgeTime = 0.6f;
    public float addDodgeTime { get; set; }
    public float increasedDodgeTime {  get; set; }
    public float decreasedDodgeTime { get; set; }
    public float dodgeTime
    {
        get { return (defaultDodgeTime + addDodgeTime) * (1f + increasedDodgeTime) * (1f - decreasedDodgeTime); }
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
