using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/SpinAttackSkill")]

public class SpinAttackSkillData : SkillData
{
    [field: SerializeField, Header("Information"),Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���ݷ� ���")] public float ratio { get; private set; }                   

    [field: SerializeField, Tooltip("�ִ� ���� ���ط�")] public float maxHoldPower { get; private set; }               
    [field: SerializeField, Tooltip("�⺻ ũ��")] public float defaultSize { get; private set; }                  
    [field: SerializeField, Tooltip("����Ʈ ���� �ð�")] public float effectTime { get; private set; }                  


    [field: SerializeField, Header("GameObject"), Tooltip("����Ʈ ������")] public GameObject spinPrefab { get; private set; }
    [field: SerializeField, Tooltip("����Ʈ �ù� ������")] public GameObject spinSimulPrefab { get; private set; }
}
