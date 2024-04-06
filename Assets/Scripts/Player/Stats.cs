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

    // 피해감소
    // UI : 피해감소 0%
    // 받는 피해 = 피해량 * 피해감소
    // 최대, 최소 75%
    public float defaultDefensivePower = 0f;
    public float addDefensivePower { get; set; }
    public float increasedDefensivePower { get; set; }
    public float decreasedDefensivePower { get; set; }
    public float defensivePower
    {
        get {
            float DP = (defaultDefensivePower + addDefensivePower) * (1f + increasedDefensivePower) * (1f - decreasedDefensivePower);
            if (DP > 0.75f)
                return 1 - 0.75f;
            else if (DP < -0.75f)
                return 1 + 0.75f;
            else
                return 1 - DP;
        }
    }

    // 상태이상 저항
    // UI : 상태이상 저항 0%
    // 상태이상 효과 = 지속시간 또는 피해량 * 상태이상 저항
    // 최대,최소 75%
    public float defaultSEResist = 0f;
    public float addSEResist { get; set; }
    public float increasedSEResist { get; set; }
    public float decreasedSEResist { get; set; }
    public float SEResist
    {
        get {
            float SER = (defaultSEResist + addSEResist) * (1f + increasedSEResist) * (1f - decreasedSEResist);
            if(SER > 0.75f)
                return 1 - 0.75f;
            else if(SER < -0.75f)
                return 1 + 0.75f;
            else 
                return 1 - SER;
        }
    }

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
