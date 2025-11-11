using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/SpinAttackSkill")]

public class SpinAttackSkillData : SkillData
{
    [field: SerializeField, Header("Information"),Tooltip("기본 피해량")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("공격력 계수")] public float ratio { get; private set; }                   

    [field: SerializeField, Tooltip("최대 증가 피해량")] public float maxHoldPower { get; private set; }               
    [field: SerializeField, Tooltip("기본 크기")] public float defaultSize { get; private set; }                  
    [field: SerializeField, Tooltip("이펙트 유지 시간")] public float effectTime { get; private set; }                  


    [field: SerializeField, Header("GameObject"), Tooltip("이펙트 프리팹")] public GameObject spinPrefab { get; private set; }
    [field: SerializeField, Tooltip("이펙트 시뮬 프리팹")] public GameObject spinSimulPrefab { get; private set; }

    public override string Update_NumText(Stats _Stats)
    {
        return (defaultDamage + ratio * _Stats.AttackPower.Value).ToString() + "~" + ((defaultDamage + ratio * _Stats.AttackPower.Value) * maxHoldPower).ToString();
    }
}
