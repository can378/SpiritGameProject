using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 플레이어 상태
    [field: SerializeField] public float runCurrentCoolTime { get; set; }       // 달리기 대기시간
    [field: SerializeField] public float attackDelay { get; set; }              // 공격 대기시간
    [field: SerializeField] public float subWeaponDelay {get; set; }            // subWeaponDelay 준비 시간
    [field: SerializeField] public bool isPlayerMove { get; set; }

    [field: SerializeField] public bool isReload {get; set;}               //장전
    [field: SerializeField] public bool isSprint { get; set; }                 //달리기
    [field: SerializeField] public bool isDodge { get; set; }                //회피
    [field: SerializeField] public bool isAttack { get; set; }                //공격
    [field: SerializeField] public bool isAttackReady { get; set; }           //공격 준비 완료
    [field: SerializeField] public bool isEquip { get; set; }                 //무기 장비
    [field: SerializeField] public bool isInvincible { get; set; }            //무적 상태
    [field: SerializeField] public bool isAttackable { get; set; }             //공격가능 상태

    [field: SerializeField] public bool isSubWeapon { get; set; }               // 보조무기 사용중
    [field: SerializeField] public bool isGuard { get; set; }                   // 막기
    [field: SerializeField] public bool isParry { get; set; }                   // 반격
    [field: SerializeField] public bool isSubWeaponReady { get; set; }          // 보조무기 준비 완료

    [field: SerializeField] public List<StatusEffect> activeEffects = new List<StatusEffect>();

}
