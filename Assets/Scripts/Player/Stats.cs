using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [Header("최종값")]
    [field: SerializeField, ReadOnly] float _Value;
    [Header("기본값")]
    [field: SerializeField] float _DefaultValue;
    float _MaxValue;
    float _MinValue;
    [Header("수정값")]
    [field: SerializeField, ReadOnly] float _AddValue;
    [field: SerializeField, ReadOnly] float _IncreasedValue;
    [field: SerializeField, ReadOnly] float _DecreasedValue;

    public Stat(float _DefalutValue, float _MaxValue = float.MaxValue, float _MinValue = float.MinValue)
    {
        this._DefaultValue = _DefalutValue;
        this._MaxValue = _MaxValue;
        this._MinValue = _MinValue;
    }

    public void SetDefaultValue(float _DefalutValue)
    {
        this._DefaultValue = _DefalutValue;
        ResetValue();
    }

    //public void SetDefaultValue(float _DefalutValue) { this._DefalutValue = _DefalutValue; }

    // 최종 값을 수정합니다.
    void ResetValue()
    {
        _Value = Mathf.Clamp((_DefaultValue + _AddValue) * (1f + _IncreasedValue) * (1f - _DecreasedValue), _MinValue, _MaxValue);
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
    [field: SerializeField] public Stat HPMax = new Stat(100, 999999, 0);
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
    [field: SerializeField] public Stat[] SEResist { get; set; } = 
    {
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f), 
        new Stat(0.0f, 0.75f, -0.75f)
    };

    // Attack
    // 공격력
    // UI : 공격력 0
    // 기본 공격 피해량 = 공격력
    // 최소 0
    [field: SerializeField] public Stat AttackPower = new Stat(0.0f, float.MaxValue, 0);

    // 오브젝트의 공격 시 효과
    // 상태이상 효과
    [field: SerializeField] public List<SE_TYPE> SEType { get; set; } = new List<SE_TYPE>();

    // 공격 성공 시 효과
    [field: SerializeField] public List<COMMON_TYPE> CommonType { get; set; } = new List<COMMON_TYPE>();

    // 투사체 전용 효과
    [field: SerializeField] public List<PROJECTILE_TYPE> ProjectileType { get; set; } = new List<PROJECTILE_TYPE>();

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

    
    [field: SerializeField] public SerializedDictionary<int, PassiveData> activePassive = new SerializedDictionary<int, PassiveData>();         //버프 디버프
    [field: SerializeField] public SerializedDictionary<int, Buff> activeEffects = new SerializedDictionary<int, Buff>();         //버프 디버프
}
