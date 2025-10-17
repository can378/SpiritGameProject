using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/ThunderboltSkill")]

public class ThunderboltSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���� ���")] public float ratio { get; private set; }

    [field: SerializeField, Tooltip("�⺻ ũ��")] public float defaultSize { get; private set; }
    [field: SerializeField, Tooltip("���� �ð�")] public float effectTime { get; private set; }
    [field: SerializeField, Tooltip("���� �ҿ� ����")] public float summonAreaSize { get; private set; }
    [field: SerializeField, Tooltip("�ʴ� ��ȯ Ƚ��")] public float DPS { get; private set; }
    [field: SerializeField, Tooltip("�˹� ��ġ")] public float knockBack { get; private set; }

    [field: SerializeField, Tooltip("���� ��ȭ �����̻�")] public BuffData[] statusEffect { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("���� ������")] public GameObject thunderboltEffectSimul { get; private set; }
    [field: SerializeField, Header("GameObject"), Tooltip("���� �ҿ� ���� �ù� ������")] public GameObject summonAreaSimul { get; private set; }

    public override string Update_NumText(Stats _Stats)
    {
        return (defaultDamage + _Stats.SkillPower.Value * ratio).ToString();
    }

}
