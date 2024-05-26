using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    //EXP
    //public int level = 1;
    //public int exp = 1;
    //public int point = 10;

    //Attack

    // Critical
    // UI : ġ��Ÿ Ȯ�� 0%
    // ġ��Ÿ = ������(0 ~ 100) < ġ��Ÿ Ȯ�� * 100 ? �� : ����
    // �ּ� 0%, �ִ� 100%
    [field: SerializeField] public float defaultCriticalChance { get; set; } = 0;
    public float addCriticalChance { get; set; }
    public float increasedCriticalChance { get; set; }
    public float decreasedCriticalChance { get; set; }
    public float criticalChance
    {
        get { return Mathf.Clamp((defaultCriticalChance + addCriticalChance) * (1f + increasedCriticalChance) * (1f - decreasedCriticalChance), 0, 1f); }
    }

    //CriticalDamage
    // UI : ġ��Ÿ ���ط� 150%
    // ���ط� = ġ��Ÿ ? ġ��Ÿ ���ط� * �⺻ ���ط� : �⺻ ���ط�
    // �ּ� 100%, �ִ� 300%
    [field: SerializeField] public float defaultCriticalDamage { get; set; } = 1.5f;
    public float addCriticalDamage { get; set; }
    public float increasedCriticalDamage { get; set; }
    public float decreasedCriticalDamage { get; set; }
    public float criticalDamage
    {
        get { return Mathf.Clamp((defaultCriticalDamage + addCriticalDamage) * (1f + increasedCriticalDamage) * (1f - decreasedCriticalDamage), 1f, 3f); }
    }

    // attackSpeed
    // UI 0%
    // �������� ������
    // �ʴ� ���ݼӵ� = ���� �ʴ� ���� �ӵ� * �÷��̾� ���ݼӵ�
    // �ּ� 0%, �ִ� 300%
    [field: SerializeField] public float defaultAttackSpeed { get; set; } = 1;
    public float addAttackSpeed { get; set; }
    public float increasedAttackSpeed {  get; set; }
    public float decreasedAttackSpeed { get; set; }
    public float attackSpeed
    {
        get { return Mathf.Clamp((defaultAttackSpeed + addAttackSpeed) * (1f + increasedAttackSpeed) * (1f - decreasedAttackSpeed), 0.0f, 3f); }
    }

    // Skill
    // SkillPower
    // UI ���� 0
    // ���� ���ط� = ���� �⺻ ���ط� + ���� * ��ų ���
    // �ּ� 0
    [field: SerializeField] public float defaultSkillPower { get; set; } = 0;
    public float addSkillPower { get; set; }
    public float increasedSkillPower { get; set; }
    public float decreasedSkillPower { get; set; }
    public float skillPower
    {
        get { return Mathf.Clamp((defaultSkillPower + addSkillPower) * (1f + increasedSkillPower) * (1f - decreasedSkillPower), 0f, 9999f); }
    }

    // SkillCoolTime
    // ���� ���� ��� �ð� ����
    // UI ���� ��� �ð� ���� 0%
    // �������� ���� �ڼ� ��� ����
    // ���� ���� ��� �ð� = ���� �⺻ ���� ��� �ð� * ���� ���ð�
    // �ּ� -80% ,�ִ� 80%
    [field: SerializeField] public float defaultSkillCoolTime { get; set; } = 0;
    public float addSkillCoolTime { get; set; }
    public float increasedSkillCoolTime { get; set; }
    public float decreasedSkillCoolTime { get; set; }
    public float skillCoolTime
    {
        get { return Mathf.Clamp((defaultSkillCoolTime + addSkillCoolTime) * (1f + increasedSkillCoolTime) * (1f - decreasedSkillCoolTime), -0.8f, 0.8f); }
    }

    // Move
    // RunSpeed
    // UI �޸��� �� �̵��ӵ� 50%
    // �޸��� �� �ӵ�
    // �̵��ӵ� = �̵��ӵ� (�޸��� ? �޸��� �� �ӵ� : 1)
    // �ּ� 100%
    [field: SerializeField] public float defaultRunSpeed { get; set; } = 1f;
    public float addRunSpeed { get; set; }
    public float increasedRunSpeed {  get; set; }
    public float decreasedRunSpeed { get; set; }
    public float runSpeed
    {
        get { return Mathf.Clamp((defaultRunSpeed + addRunSpeed) * (1f + increasedRunSpeed) * (1f - decreasedRunSpeed), 0f, 3f); }
    }

    //RunCoolTime
    // �޸��� ���� ��� �ð�
    // UI : �޸��� ���� ��� �ð� 5��
    // �޸��� ���� ��� �ð� = �޸��� ���� ��� �ð�
    // �ּ� 0��
    [field: SerializeField] public float defaultRunCoolTime { get; set; } = 5f;
    public float addRunCoolTime { get; set; }
    public float increasedRunCoolTime { get; set; }
    public float decreasedRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get { return Mathf.Clamp((defaultRunCoolTime + addRunCoolTime) * (1f + increasedRunCoolTime) * (1f - decreasedRunCoolTime), 0f, 10f); }
    }

    // Dodge
    // ȸ�� �ӵ�
    // UI : ȸ�� �� �߰� �̵� �ӵ� 66%
    // �ּ� 50%
    [field: SerializeField] public float defaultDodgeSpeed { get; set; } = 0.66f;
    public float addDodgeSpeed { get; set; }
    public float increasedDodgeSpeed {  get; set; }
    public float decreasedDodgeSpeed { get; set; }
    public float dodgeSpeed
    {
        get { return Mathf.Clamp((defaultDodgeSpeed + addDodgeSpeed) * (1f + increasedDodgeSpeed) * (1f - decreasedDodgeSpeed), 0f, 2f); }
    }

    // ȸ�� �ð�
    // UI : ȸ�� �ð� 0.8��
    // ȸ�� �ð� = ȸ�� �ð�
    // �ּ� 0.1��
    [field: SerializeField] public float defaultDodgeTime { get; set; }  = 0.4f;
    public float addDodgeTime { get; set; }
    public float increasedDodgeTime {  get; set; }
    public float decreasedDodgeTime { get; set; }
    public float dodgeTime
    {
        get { return Mathf.Clamp((defaultDodgeTime + addDodgeTime) * (1f + increasedDodgeTime) * (1f - decreasedDodgeTime), 0.1f, 0.8f); }
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
    public int[] equipments = {0,0,0};

    //Stat
    public int[] playerStat = new int[8];
}
