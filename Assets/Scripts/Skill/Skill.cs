using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillLimit {None, Melee, Shot}

public abstract class Skill : SelectItem
{
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public int skillID { get; private set; }
    [field: SerializeField] public SkillLimit skillLimit { get; private set; }
    [field: SerializeField] public int skillType { get; private set; }                  // 0�̸� ���, 1�̸� �غ�, 2�̸� Ȧ��

    [field: SerializeField] public float preDelay { get; private set; }                 //��ų ��� �� ��� �ð�
    [field: SerializeField] public float rate { get; private set; }                     //��ų ��� �ð�
    [field: SerializeField] public float postDelay { get; private set; }                //��ų ��� �� ��� �ð�

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //�⺻ ��� �ð�
    [field: SerializeField] public float skillCoolTime { get; set; }                    //���� ��� �ð�

    [field: SerializeField] public GameObject user { get; set; }                        //�����

    void Update() {
        CoolDown();
    }

    public abstract void Use(GameObject user);                                          //���
    public abstract void Exit(GameObject user);                                         //��ų ��� ��

    void CoolDown()
    {
        skillCoolTime -= Time.deltaTime;
    }



}
