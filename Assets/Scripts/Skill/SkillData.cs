using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE { DOWN, HOLD, UP }


public class SkillData : ItemData
{
    [field: SerializeField] public SKILL_TYPE skillType { get; private set; }                  // ��ų ���� Ÿ�̹�
                                                                                               //��ų Ű Down : 0
                                                                                               //��ų Ű Hold : 1
                                                                                               //��ų Ű Up   : 2

    [field: SerializeField] public WEAPON_TYPE[] skillLimit { get; private set; }       // �ش� ���⸦ ������ ���� ���� ��ų ����� �����ϴ�.
                                                                                        // ���ٸ� ������ ���� ��ų�̴�.                                                                            

    [field: SerializeField] public float maxHoldTime { get; private set; }              // Ȧ�� �ִ� �����ð�
    [field: SerializeField] public float preDelay { get; private set; }                 // ��ų �ߵ� �� ��� �ð�
    [field: SerializeField] public float postDelay { get; private set; }                // ��ų �ߵ� �� ��� �ð�

    [field: SerializeField] public float skillDefalutCoolTime { get; private set; }     //�⺻ ��� �ð�
    [field: SerializeField] public Dictionary<string, object> CustomData { get; private set; }

    public string TypeToKorean()
    {
        {
            switch (skillType)
            {
                case SKILL_TYPE.UP:
                    return "���";
                case SKILL_TYPE.HOLD:
                    return "����";
                case SKILL_TYPE.DOWN:
                    return "����";
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

