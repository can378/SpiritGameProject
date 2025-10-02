using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/WheelWindSkill")]

public class WheelWindSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���ݷ� ���")] public float ratio { get; private set; }

    [field: SerializeField, Tooltip("�ʴ� Ÿ�� Ƚ��")] public int DPS { get; private set; }
    [field: SerializeField, Tooltip("�⺻ ũ��")] public float defaultSize { get; private set; }


    [field: SerializeField, Header("GameObject"), Tooltip("����Ʈ ������")] public GameObject WheelWindPrefab { get; private set; }



}
