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

    // 피해 감소
    public float defaultReduction = 0;
    public float increasedReduction { get; set; }
    public float addReduction { get; set; }
    public float reduction
    {
        get { return (1 + increasedReduction) * (0f + addReduction); }
    }
    
    // 속성 저항
    public float[] resist = new float[11];  //저항은 무조선 덧셈 뺄셈으로

    //Attack
    //공격력
    public float defaultPower = 0f;
    public float increasedPower { get; set; }
    public float addPower { get; set; }
    public float power
    {
        get { return (1 + increasedPower) * (defaultPower + addPower); }
    }

    //Speed
    public float defaultMoveSpeed = 5f;
    public float increasedMoveSpeed { get; set; }
    public float addMoveSpeed { get; set; }
    public float moveSpeed
    {
        get { return (1 + increasedMoveSpeed) * (defaultMoveSpeed + addMoveSpeed); }
    }

    public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
