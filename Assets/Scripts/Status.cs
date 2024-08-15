using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public Vector2 moveVec;                 // 이동 방향 벡터
    public Coroutine flinchCoroutine;       // 피격 코루틴
    public GameObject hitTarget;            // 공격 성공

    public bool isBeAttaked;                  // 피격 : 피격됨
    public bool isFlinch;                   // 경직 : 스스로 움직일 수 없으며 공격할 수 없음
    public bool isInvincible;               // 무적 : 피해와 적의 공격 무시
    public bool isAttack;                   // 공격 : 스스로 움직일 수 없으며 추가로 공격 불가
    public bool isAttackReady = true;       // 공격 준비 : false일 시 공격은 할 수 없으나 스스로 이동은 가능







}
