using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : StatusEffect
{
    // 배율
    [field: SerializeField] public float moveSpeedIncrease { get; set; }
    [field: SerializeField] public float attackSpeedIncrease { get; set; }

    public override void ApplyEffect()
    {
        ResetEffect();
        StartCoroutine(SpeedOverTime());
    }

    public override void ResetEffect()      //지속시간 갱신
    {
        if(target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            // 효과 잠시 제거
            playerStats.increasedMoveSpeed -= overlap * moveSpeedIncrease;
            playerStats.increasedAttackSpeed -= overlap * attackSpeedIncrease;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - (playerStats.resist[resist] * 2)) * defaultDuration;

            playerStats.increasedMoveSpeed += overlap * moveSpeedIncrease;
            playerStats.increasedAttackSpeed += overlap * attackSpeedIncrease;

            Debug.Log("MoveSpeed buff applied: " + overlap * moveSpeedIncrease + "MoveSpeed");
            Debug.Log("MoveSpeed : " + playerStats.moveSpeed);
            Debug.Log("AttackSpeed buff applied: " + overlap * attackSpeedIncrease + " AttackSpeed");
            Debug.Log("AttackSpeed : " + playerStats.attackSpeed);

            //print(duration);
        }
        else {
            Stats stats = target.GetComponent<Stats>();

            // 효과 잠시 제거
            stats.increasedMoveSpeed -= overlap * moveSpeedIncrease;
            //stats.increasedAttackSpeed -= overlap * attackSpeedIncrease;

            // 중첩 
            overlap = overlap < maxOverlap ? overlap + 1 : maxOverlap;

            // 저항에 따른 지속시간 적용
            duration = (1 - (stats.resist[resist] * 2)) * defaultDuration;

            stats.increasedMoveSpeed += overlap * moveSpeedIncrease;
            //stats.increasedAttackSpeed += overlap * attackSpeedIncrease;

            Debug.Log("MoveSpeed buff applied: " + overlap * moveSpeedIncrease + "MoveSpeed");
            Debug.Log("MoveSpeed : " + stats.moveSpeed);
            //Debug.Log("AttackSpeed buff applied: " + overlap * attackSpeedIncrease + " AttackSpeed");
            //Debug.Log("Playr AttackSpeed : " + stats.attackSpeed);

            //print(duration);
        }
    }

    private IEnumerator SpeedOverTime()
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.1f);

            duration -= 0.1f;
        }
    }

    public override void RemoveEffect()
    {
        if(target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();

            playerStats.increasedMoveSpeed -= overlap * moveSpeedIncrease;
            playerStats.increasedAttackSpeed -= overlap * attackSpeedIncrease;

            Debug.Log("MoveSpeed buff removed: " + overlap * moveSpeedIncrease + "MoveSpeed");
            Debug.Log("Playr MoveSpeed : " + playerStats.moveSpeed);
            Debug.Log("AttackSpeed buff removed: " + overlap * attackSpeedIncrease + " AttackSpeed");
            Debug.Log("Playr AttackSpeed : " + playerStats.attackSpeed);
        }
        else
        {
            Stats stats = target.GetComponent<Stats>();

            stats.increasedMoveSpeed -= overlap * moveSpeedIncrease;
            //playerStats.increasedAttackSpeed -= overlap * attackSpeedIncrease;

            Debug.Log("MoveSpeed buff removed: " + overlap * moveSpeedIncrease + "MoveSpeed");
            Debug.Log("Playr MoveSpeed : " + stats.moveSpeed);
            //Debug.Log("AttackSpeed buff removed: " + overlap * attackSpeedIncrease + " AttackSpeed");
            //Debug.Log("Playr AttackSpeed : " + playerStats.attackSpeed);
        }
        
    }
}
