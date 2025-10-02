using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/WaveSkill")]

public class WaveSkillData : SkillData
{
    [field: SerializeField, Header("Information"),Tooltip("�⺻ ���ط�")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("���� ���")] public float ratio { get; private set; }                                            
    [field: SerializeField, Tooltip("����Ʈ ���� �ð�")] public float effectTime { get; private set; }                  


    [field: SerializeField, Header("GameObject"), Tooltip("����Ʈ ������")] public GameObject waveEffectPrefab { get; private set; }
    [field: SerializeField, Tooltip("���� �����")] public BuffData statusEffect { get; private set; }
}
