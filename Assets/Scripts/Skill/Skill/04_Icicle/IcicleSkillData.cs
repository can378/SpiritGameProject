using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/IcicleSkill")]

public class IcicleSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���� ���")] public float ratio { get; private set; }
    [field: SerializeField, Tooltip("�˹� ��ġ")] public float knockBack { get; private set; }


    [field: SerializeField, Tooltip("����ü ũ��")] public float defaultSize { get; private set; }
    [field: SerializeField, Tooltip("����ü �ӵ�")] public float projectileSpeed { get; private set; }
    [field: SerializeField, Tooltip("����ü �ð�")] public float projectileTime { get; private set; }

    [field: SerializeField, Tooltip("��ȭ �����̻�")] public BuffData[] statusEffect { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("����Ʈ ������")] public GameObject iciclePrefab { get; private set; }
    [field: SerializeField, Tooltip("����Ʈ �ù� ������")] public GameObject icicleSimulPrefab { get; private set; }




}
