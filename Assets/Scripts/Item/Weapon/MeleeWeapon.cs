using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MainWeapon
{
    [field: SerializeField] public int attackType {get; private set;}   // 0�̸� �ֵθ���, 1�̸� ���, 2�̸� ��Ÿ
    [field: SerializeField] public float weaponSize { get; private set; } // �⺻ 1
}
