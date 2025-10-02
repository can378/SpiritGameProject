using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/WaveSkill")]

public class WaveSkillData : SkillData
{
    [field: SerializeField, Header("Information"),Tooltip("기본 피해량")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("도력 계수")] public float ratio { get; private set; }                                            
    [field: SerializeField, Tooltip("이펙트 유지 시간")] public float effectTime { get; private set; }                  


    [field: SerializeField, Header("GameObject"), Tooltip("이펙트 프리팹")] public GameObject waveEffectPrefab { get; private set; }
    [field: SerializeField, Tooltip("기절 디버프")] public BuffData statusEffect { get; private set; }
}
