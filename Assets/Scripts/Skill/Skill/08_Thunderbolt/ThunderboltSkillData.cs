using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/ThunderboltSkill")]

public class ThunderboltSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("기본 피해량")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("도력 계수")] public float ratio { get; private set; }

    [field: SerializeField, Tooltip("기본 크기")] public float defaultSize { get; private set; }
    [field: SerializeField, Tooltip("벼락 시간")] public float effectTime { get; private set; }
    [field: SerializeField, Tooltip("벼락 소원 범위")] public float summonAreaSize { get; private set; }
    [field: SerializeField, Tooltip("초당 소환 횟수")] public float DPS { get; private set; }
    [field: SerializeField, Tooltip("넉백 수치")] public float knockBack { get; private set; }

    [field: SerializeField, Tooltip("벼락 약화 상태이상")] public BuffData[] statusEffect { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("벼락 프리팹")] public GameObject thunderboltEffectSimul { get; private set; }
    [field: SerializeField, Header("GameObject"), Tooltip("벼략 소원 범위 시뮬 프리팹")] public GameObject summonAreaSimul { get; private set; }

    public override string Update_NumText(Stats _Stats)
    {
        return (defaultDamage + _Stats.SkillPower.Value * ratio).ToString();
    }

}
