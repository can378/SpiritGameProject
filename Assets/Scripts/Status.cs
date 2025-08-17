using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 오브젝트의 상태를 나타내는 클래스
/// 영구적이지 않은 값들을 저장
/// </summary>
public class Status : MonoBehaviour
{
    public Vector2 moveVec;                     // 이동 방향

    /// <summary>
    /// 추가 속도 (특정 상태일 때 잠시 적용 되는 속도, 우선 Status 소유 오브젝트만 사용할 것)
    /// </summary>
    public float moveSpeedMultiplier = 1f;
    public Coroutine beAttackedCoroutine;       // 피격 코루틴
    public GameObject hitTarget;                // 공격을 받은 대상

    public bool isBeAttaked;                 // 공격 받은 상태
    public float isFlinch;                   // 경직 상태, 조작 불가
    public bool isSuperArmor;               // 경직 무시
    public bool isInvincible;               // 무적
    public bool isAttack;                   // 공격
    public bool isAttackReady = true;       // 공격 준비 (true면 공격 가능, false면 공격 불가)

    public Transform fearTarget;            // 공포를 건 대상
    public Coroutine runCoroutine;          // 도망가기 코루틴

    public Coroutine watingCoroutine;       // 대기 코루틴





}
