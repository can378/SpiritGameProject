using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    // 외부에서 접근 할 수도 있는 플레이어 정보들
    // 유저데이터에 넣기 애매한 것들은 우선 여기로
   [field: SerializeField] public GameObject nearObject { get; set; }

    [field: SerializeField] public Vector3 mousePos { get; set; }
    [field: SerializeField] public Vector3 mouseDir { get; set; }
    [field: SerializeField] public float mouseAngle { get; set; }

    [field: SerializeField] public float runCurrentCoolTime { get; set; }           // 달리기 대기시간
         
    [field: SerializeField] public bool isPlayerMove { get; set; }

    [field: SerializeField] public bool isReload {get; set;}                        // 장전
    [field: SerializeField] public float reloadDelay { get; set; }
    [field: SerializeField] public bool isDodge { get; set; }                       // 회피

    [field: SerializeField] public bool isAttackable { get; set; }                  // 공격가능 상태

    [field: SerializeField] public float attackDelay { get; set; }                  // 다음 공격까지 대기시간

    [field: SerializeField] public int skillIndex { get; set; } = 0;                // 현재 스킬 인덱스
    [field: SerializeField] public float skillChangeDelay { get; set; }
    [field: SerializeField] public bool isSkill { get; set; }                       // 스킬 사용 중
    [field: SerializeField] public bool isSkillHold { get; set; }                   // 스킬 홀드 중
    

}
