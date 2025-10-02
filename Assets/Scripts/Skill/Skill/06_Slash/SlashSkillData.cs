using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/SlashSkill")]

public class SlashSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���ݷ� ���")] public float ratio { get; private set; }

    [field: SerializeField, Tooltip("�ִ� ���� ���ط�")] public float maxHoldPower { get; private set; }
    [field: SerializeField, Tooltip("�ʴ� Ÿ�� Ƚ��")]public  int DPS { get; private set; }
    [field: SerializeField, Tooltip("�⺻ ũ��")] public float effectSize { get; private set; }
    [field: SerializeField, Tooltip("����ü �ӵ�")] public float projectileSpeed { get; private set; }
    [field: SerializeField, Tooltip("����ü �����Ÿ�")] public float projectileTime { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("���� ������")] public GameObject slashPrefab { get; private set; }
    [field: SerializeField, Tooltip("���� �ù� ������")] public GameObject slashSimulPrefab { get; private set; }


}
