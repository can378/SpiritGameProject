using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDeBuff : StatusEffect
{
    // 배율
    [field: SerializeField] public float decreasedMoveSpeed { get; set; }
    [field: SerializeField] public float decreasedAttackSpeed { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - (playerStats.resist[resist] * 2)) * defaultDuration;

            playerStats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed += overlap * decreasedAttackSpeed;

            Debug.Log("MoveSpeed buff applied: " + overlap * decreasedMoveSpeed + "MoveSpeed");
            Debug.Log("MoveSpeed : " + playerStats.moveSpeed);
            Debug.Log("AttackSpeed buff applied: " + overlap * decreasedAttackSpeed + " AttackSpeed");
            Debug.Log("AttackSpeed : " + playerStats.attackSpeed);

            //print(duration);
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            //stats.decreaseddAttackSpeed -= overlap * decreasedAttackSpeed;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - (stats.resist[resist] * 2)) * defaultDuration;

            stats.decreasedMoveSpeed += overlap * decreasedMoveSpeed;
            //stats.decreaseddAttackSpeed += overlap * decreasedAttackSpeed;

            Debug.Log("MoveSpeed buff applied: " + overlap * decreasedMoveSpeed + "MoveSpeed");
            Debug.Log("MoveSpeed : " + stats.moveSpeed);
            //Debug.Log("AttackSpeed buff applied: " + overlap * decreasedAttackSpeed + " AttackSpeed");
            //Debug.Log("Playr AttackSpeed : " + stats.attackSpeed);

            //print(duration);
        }
    }

    public override void RemoveEffect()
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            playerStats.decreasedAttackSpeed -= overlap * decreasedAttackSpeed;

            Debug.Log("MoveSpeed buff removed: " + overlap * decreasedMoveSpeed + "MoveSpeed");
            Debug.Log("Playr MoveSpeed : " + playerStats.moveSpeed);
            Debug.Log("AttackSpeed buff removed: " + overlap * decreasedAttackSpeed + " AttackSpeed");
            Debug.Log("Playr AttackSpeed : " + playerStats.attackSpeed);
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.decreasedMoveSpeed -= overlap * decreasedMoveSpeed;
            //playerStats.decreaseddAttackSpeed -= overlap * decreasedAttackSpeed;

            Debug.Log("MoveSpeed buff removed: " + overlap * decreasedMoveSpeed + "MoveSpeed");
            Debug.Log("Playr MoveSpeed : " + stats.moveSpeed);
            //Debug.Log("AttackSpeed buff removed: " + overlap * decreasedAttackSpeed + " AttackSpeed");
            //Debug.Log("Playr AttackSpeed : " + playerStats.attackSpeed);
        }

    }
}