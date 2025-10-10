using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/HealSkill")]

public class HealSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("기본 회복량")] public float defaultHeal { get; private set; }
    [field: SerializeField, Tooltip("도력 계수")] public float dotHeal { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("이펙트 프리팹")] public GameObject HealEffectPrefab { get; private set; }

    public override string Update_Num()
    {
        return "회복량";
    }

    public override string Update_NumText(Stats _Stats)
    {
        return (defaultHeal * _Stats.HPMax.Value).ToString() + " ~ " + (defaultHeal * _Stats.HPMax.Value + dotHeal * maxHoldTime * _Stats.HPMax.Value).ToString();
    }

}
