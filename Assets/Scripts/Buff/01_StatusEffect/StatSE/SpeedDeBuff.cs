using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDeBuff : StatusEffect
{
    // 배율
    // 이동속도, 공격속도 감소
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        // 플레이어일 시
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - playerStats.SEResist(buffId)) * defaultDuration;

            playerStats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed += overlap * decreasedAttackSpeed;

        }
        // 그 외에
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - stats.SEResist(buffId)) * defaultDuration;

            stats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;

        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;

        }

    }
}