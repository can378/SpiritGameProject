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
    // �ּ� 0%, �ִ� 100%
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
    // UI : ġ��Ÿ ���ط� 150%
    // ���ط� = ġ��Ÿ ? ġ��Ÿ ���ط� * �⺻ ���ط� : �⺻ ���ط�
    // �ּ� 100%, �ִ� 300%
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
    // �������� ������
    // �ʴ� ���ݼӵ� = ���� �ʴ� ���� �ӵ� * �÷��̾� ���ݼӵ�
    // �ּ� 0%, �ִ� 300%
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
    // UI ���� 0
    // ���� ���ط� = ���� �⺻ ���ط� + ���� * ��ų ���
    // �ּ� 0
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
    // ���� ���� ��� �ð� ����
    // UI ���� ��� �ð� ���� 0%
    // �������� ���� �ڼ� ��� ����
    // ���� ���� ��� �ð� = ���� �⺻ ���� ��� �ð� * ���� ���ð�
    // �ּ� -80% ,�ִ� 80%
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
    // UI �޸��� �� �̵��ӵ� 166%
    // �޸��� �� �ӵ�
    // �̵��ӵ� = �̵��ӵ� (�޸��� ? �޸��� �� �ӵ� : 1)
    // �ּ� 100%
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
    // �޸��� ���� ��� �ð�
    // UI : �޸��� ���� ��� �ð� 5��
    // �޸��� ���� ��� �ð� = �޸��� ���� ��� �ð�
    // �ּ� 0��
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
    // ȸ�� �ӵ�
    // UI : ȸ�� �� �̵� �ӵ� 200%
    // �ּ� 50%
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

    // ȸ�� �ð�
    // UI : ȸ�� �ð� 0.6��
    // ȸ�� �ð� = ȸ�� �ð�
    // �ּ� 0.2��
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
