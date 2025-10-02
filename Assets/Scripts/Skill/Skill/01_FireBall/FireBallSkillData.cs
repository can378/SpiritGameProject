using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/FireBallSkill")]

public class FireBallSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("�⺻ ������")] public int defaultDamage { get; private set; }              // �⺻ �����
    [field: SerializeField, Tooltip("���� ���")] public float ratio { get; private set; }                    // ���� ����

    [field: SerializeField, Tooltip("�˹� ��ġ")] public float knockBack { get; private set; }               // �˹� �Ÿ�
    [field: SerializeField, Tooltip("�����Ÿ�")] public float range { get; private set; }                  // �����Ÿ�

    [field: SerializeField, Header("GameObject"), Tooltip("ȭ���� ����Ʈ")] public GameObject fireBallPrefab { get; private set; }

    [field: SerializeField, Tooltip("ȭ�� �����")] public BuffData BurnDeBuff { get; private set; }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(description, defaultDamage + _Stats.SkillPower.Value * ratio);
    }

    public override float DamageText(Stats _Stats)
    {
        return defaultDamage + ratio * _Stats.SkillPower.Value;
    }

}
