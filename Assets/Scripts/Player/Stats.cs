using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    /// <summary>
    /// default는 개발자가 정하는 기본 수치
    /// increased,decreased는 증가율, 감소율
    /// add,sub는 + 추가,감소
    /// 마지막 최종 수치
    /// </summary>

    //HP
    [field :SerializeField] public float HPMax { get; set; } = 100f;
    [field: SerializeField] public float HP { get; set; } = 100f;
    [field: SerializeField] public float tempHP { get; set; } = 0;
    // 강인도
    [field: SerializeField] public float poiseMax { get; set; } = 20f;
    [field: SerializeField] public float poise { get; set; } = 20f;

    // 방어력
    // UI : 방어력 0%
    // 받는 피해 = 피해량 * 방어력
    // 최소 -50%, 최대 50%
    [field: SerializeField] public float defaultDefensivePower { get; set; } = 0f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get { return Mathf.Clamp((defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower), -0.5f, 0.5f); }
    }

    // 상태이상 저항
    // UI : 상태이상 저항 0%
    // 상태이상 효과 = 지속시간 또는 피해량 * 상태이상 저항
    // 최소 -75%, 최대 75%
    [field: SerializeField] public float[] defaultSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] addSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] increasedSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float[] decreasedSEResist { get; set; } = new float[(int)BuffType.SPECIAL];
    public float SEResist(int index)
    {
        return Mathf.Clamp((defaultSEResist[index] + addSEResist[index]) * (1f + increasedSEResist[index]) * (1f - decreasedSEResist[index]), -1f, 1f);
    }

    // Attack
    // 공격력
    // UI : 공격력 0
    // 기본 공격 피해량 = 공격력
    // 최소 0
    [field: SerializeField] public float defaultAttackPower { get; set; } = 0f;
    public float addAttackPower { get; set; }
    public float increasedAttackPower { get; set; }
    public float decreasedAttackPower { get; set; }
    public float attackPower
    {
        get{ return Mathf.Clamp((defaultAttackPower + addAttackPower) * (1f + increasedAttackPower) * (1f - decreasedAttackPower), 0, 9999f); }
    }

    // Speed
    // 이동 속도 5
    // 이동 속도 = 이동속도
    // 최소 0
    [field: SerializeField] public float defaultMoveSpeed { get; set; } = 5f;
    public float addMoveSpeed { get; set; }
    public float increasedMoveSpeed { get; set; }
    public float decreasedMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return Mathf.Clamp((defaultMoveSpeed + addMoveSpeed) * (1f + increasedMoveSpeed) * (1f - decreasedMoveSpeed), 0, 20f); }
    }

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
