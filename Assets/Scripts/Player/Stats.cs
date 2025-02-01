using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [Header("최종값")]
    [field: SerializeField, ReadOnly] float _Value;
    [Header("기본값")]
    [field: SerializeField] float _DefalutValue;
    float _MaxValue;
    float _MinValue;
    [Header("수정값")]
    [field: SerializeField, ReadOnly] float _AddValue;
    [field: SerializeField, ReadOnly] float _IncreasedValue;
    [field: SerializeField, ReadOnly] float _DecreasedValue;

    public Stat(float _DefalutValue, float _MaxValue = float.MaxValue, float _MinValue = float.MinValue)
    {
        this._DefalutValue = _DefalutValue;
        this._MaxValue = _MaxValue;
        this._MinValue = _MinValue;
    }

    public void SetDefaultValue(float _DefalutValue) { this._DefalutValue = _DefalutValue; }

    public void ResetValue()
    {
        _Value = Mathf.Clamp((_DefalutValue + _AddValue) * (1f + _IncreasedValue) * (1f - _DecreasedValue), _MinValue, _MaxValue);
    }

    public float Value
    {
        get
        {
            ResetValue();
            return _Value;
        }
    }

    public float AddValue
    {
        get {return _AddValue;}
        set
        {
            _AddValue = value;
            ResetValue();
        }
    }

    public float IncreasedValue
    {
        get { return _IncreasedValue; }
        set
        {
            _IncreasedValue = value;
            ResetValue();
        }
    }

    public float DecreasedValue
    {
        get { return _DecreasedValue; }
        set
        {
            _DecreasedValue = value;
            ResetValue();
        }
    }

}

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
    [field: SerializeField] public Stat DefensivePower = new Stat(0.0f,0.5f,-0.5f);

    // 상태이상 저항
    // UI : 상태이상 저항 0%
    // 상태이상 효과 = 지속시간 또는 피해량 * 상태이상 저항
    // 최소 -75%, 최대 75%
    [field: SerializeField] public Stat[] SEResist { get; set; } = new Stat[(int)BuffType.SPECIAL];

    // Attack
    // 공격력
    // UI : 공격력 0
    // 기본 공격 피해량 = 공격력
    // 최소 0
    [field: SerializeField] public Stat AttackPower = new Stat(0.0f, float.MaxValue, 0);

    // Skill
    // SkillPower
    // UI 도력 0
    // 도술 피해량 = 도술 기본 피해량 + 도력 * 스킬 계수
    // 최소 0
    [field: SerializeField] public Stat SkillPower = new Stat(0.0f, float.MaxValue, 0);

    // Speed
    // 이동 속도 5
    // 이동 속도 = 이동속도
    // 최소 0
    [field: SerializeField] public Stat MoveSpeed = new Stat(5f, float.MaxValue, 0);

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
