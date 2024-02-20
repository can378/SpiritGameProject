using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 외부에서 접근 할 수도 있는 플레이어 정보들
    // 유저데이터에 넣기 애매한 것들은 우선 여기로

    [field: SerializeField] public Vector2 mousePos { get; set; }
    [field: SerializeField] public Vector2 mouseDir { get; set; }
    [field: SerializeField] public float mouseAngle { get; set; }

    [field: SerializeField] public float runCurrentCoolTime { get; set; }           // 달리기 대기시간
         
    [field: SerializeField] public bool isPlayerMove { get; set; }

    [field: SerializeField] public bool isReload {get; set;}                        //장전
    [field: SerializeField] public bool isSprint { get; set; }                      //달리기
    [field: SerializeField] public bool isDodge { get; set; }                       //회피

    [field: SerializeField] public bool isInvincible { get; set; }                  //무적 상태
    [field: SerializeField] public bool isAttackable { get; set; }                  //공격가능 상태

    [field: SerializeField] public bool isAttack { get; set; }                      // 공격 중
    [field: SerializeField] public float attackDelay { get; set; }                  // 다음 공격까지 대기시간
    [field: SerializeField] public bool isAttackReady { get; set; }                 // 공격 준비 완료

    [field: SerializeField] public bool isSkill { get; set; }                       // 스킬 사용 중
    [field: SerializeField] public bool isSkillReady { get; set; }                  // 스킬 준비 중
    [field: SerializeField] public bool isSkillHold { get; set; }                   // 스킬 홀드 중

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();         //버프 디버프
}
