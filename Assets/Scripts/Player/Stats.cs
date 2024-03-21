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
    public float HPMax = 100;
    public float HP = 100;
    public float tempHP = 0;

    // 모든 받는 피해
    // UI : 받는 피해 100%
    // 받는 피해 = 피해량 * 받는 피해
    public float defaultDefensivePower = 1f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get { return (defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower); }
    }
    
    // 속성 받는 피해
    public float[] resist = new float[11] {1,1,1,1,1,1,1,1,1,1,1};  //저항은 무조선 덧셈 뺄셈으로

    // Attack
    // 공격력
    // UI : 공격력 0
    // 기본 공격 피해량 = 공격력
    public float defaultAttackPower = 0f;
    public float addAttackPower { get; set; }
    public float increasedAttackPower { get; set; }
    public float decreasedAttackPower { get; set; }
    public float attackPower
    {
        get { return (defaultAttackPower + addAttackPower) * (1f + increasedAttackPower) * (1f - decreasedAttackPower); }
    }

    // Speed
    // 이동 속도 5
    // 이동 속도 = 이동속도
    public float defaultMoveSpeed = 5f;
    public float addMoveSpeed { get; set; }
    public float increasedMoveSpeed { get; set; }
    public float decreasedMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return (defaultMoveSpeed + addMoveSpeed) * (1f + increasedMoveSpeed) * (1f - decreasedMoveSpeed); }
    }

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
