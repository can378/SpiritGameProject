using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class AttackSpeedBuff : StatusEffect
{
    // 배율
    // 공격속도 증가
    [field: SerializeField] public float increasedAttackSpeed { get; set; }

    public override void Apply()
    {
        Overlap();
    }

    public override void Overlap()      //지속시간 갱신
    {
        // 플레이어일 시
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.increasedAttackSpeed -= overlap * increasedAttackSpeed;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = defaultDuration;

            playerStats.increasedAttackSpeed += overlap * increasedAttackSpeed;

        }
        // 그 외에
        else
        {

        }
    }

    public override void Remove()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.increasedAttackSpeed -= overlap * increasedAttackSpeed;
        }
        else
        {

        }
    }
}
*/