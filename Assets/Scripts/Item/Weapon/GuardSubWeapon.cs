using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSubWeapon : SubWeapon
{
    [field: SerializeField] public float reduction { get; private set; }   // ������ ���ҷ�
    [field: SerializeField] public float speedReduction { get; private set; } // �ӵ� ���ҷ�
}
