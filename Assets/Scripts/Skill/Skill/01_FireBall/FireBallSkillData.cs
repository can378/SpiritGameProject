using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/FireBallSkill")]

public class FireBallSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("기본 데미지")] public int defaultDamage { get; private set; }              // 기본 대미지
    [field: SerializeField, Tooltip("도력 계수")] public float ratio { get; private set; }                    // 도력 비율

    [field: SerializeField, Tooltip("넉백 수치")] public float knockBack { get; private set; }               // 넉백 거리
    [field: SerializeField, Tooltip("사정거리")] public float range { get; private set; }                  // 사정거리

    [field: SerializeField, Header("GameObject"), Tooltip("화염구 이펙트")] public GameObject fireBallPrefab { get; private set; }

    [field: SerializeField, Tooltip("화상 디버프")] public BuffData BurnDeBuff { get; private set; }

    public override string Update_NumText(Stats _Stats)
    {
        return (defaultDamage + ratio * _Stats.SkillPower.Value).ToString();
    }

}
