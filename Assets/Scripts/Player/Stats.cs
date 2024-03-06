using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //HP
    public float HPMax = 100;
    public float HP = 100;
    public float tempHP = 0;

    //피해 감소
    public float reductionRatio = 0;
    public float[] resist = new float[11];

    //Attack
    public float power = 1;

    //Move Speed
    public float defaultSpeed = 5;

    public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
