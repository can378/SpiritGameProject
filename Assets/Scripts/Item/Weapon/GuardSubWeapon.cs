using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSubWeapon : SubWeapon
{
    [field: SerializeField] public float reduction { get; private set; }   // 데미지 감소량
    [field: SerializeField] public float speedReduction { get; private set; } // 속도 감소량
}
