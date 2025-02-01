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
    [field: SerializeField] public Stat CriticalChance = new Stat(0.0f, 1.0f, 0.0f);

    //CriticalDamage
    // UI : ġ��Ÿ ���ط� 150%
    // ���ط� = ġ��Ÿ ? ġ��Ÿ ���ط� * �⺻ ���ط� : �⺻ ���ط�
    // �ּ� 100%, �ִ� 300%
    [field: SerializeField] public Stat CriticalDamage { get; set; } = new Stat(1.5f, 1.0f, 3.0f);

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


    // SkillCoolTime
    // ���� ���� ��� �ð�
    // UI ���� ��� �ð� 0%
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

    // �޸��� ���
    /*
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
    */

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

    //n�ʰ� �Ǹ�
    //-->cameraManager���� ȿ������
    public float blind;

    //Item
    public int coin;
    public int key;
    public int dice;
    public int item;

    //Equipments
    public int weapon;

    public int maxSkillSlot;
    public int[] skill = new int[5];

    public int maxEquipment;
    public int[] equipments = new int[5];

    //Stat
    public int[] playerStat = new int[8];
}
