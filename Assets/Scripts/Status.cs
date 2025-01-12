using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public Vector2 moveVec;                     // ?��?�� 방향 벡터
    public Coroutine flinchCoroutine;           // 경직 코루?��
    public Coroutine beAttackedCoroutine;       // ?���? ?��?�� �?�? 코루?��
    public GameObject hitTarget;                // 공격 ?���?

    public bool isBeAttaked;                 // ?���? : ?��격됨
    public bool isFlinch;                   // 경직 : ?��?���? ???직일 ?�� ?��?���? 공격?�� ?�� ?��?��
    public bool isSuperArmor;               // 강인(?��?��?���?) : 경직 ?���? ?��?��?��.
    public bool isInvincible;               // 무적 : ?��?��??? ?��?�� 공격 무시
    public bool isAttack;                   // 공격 : ?��?���? ???직일 ?�� ?��?���? 추�??�? 공격 불�??
    public bool isAttackReady = true;       // 공격 �?�? : false?�� ?�� 공격??? ?�� ?�� ?��?��?�� ?��?���? ?��?��??? �??��

    public Transform fearTarget;            // ���� ���
    public Coroutine runCoroutine;          // �������� �ڷ�ƾ





}
