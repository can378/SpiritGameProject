using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MainWeapon
{
    [field: SerializeField] public int attackType {get; private set;}   // 0이면 휘두르기, 1이면 찌르기, 2이면 기타
    [field: SerializeField] public float weaponSize { get; private set; } // 기본 1
}
