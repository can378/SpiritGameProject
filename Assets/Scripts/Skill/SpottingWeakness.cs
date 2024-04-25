using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingWeakness : Skill
{
    // 피해량
    [field: SerializeField] GameObject spottingWeaknessBuff;

    public override void Enter(GameObject user)
    {
        base.Enter(user);
        Buff();
    }

    void Buff()
    {
        Debug.Log("Wave");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.stats.skillCoolTime) * skillDefalutCoolTime;

            player.ApplyBuff(spottingWeaknessBuff);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();

            // 쿨타임 적용
            skillCoolTime =skillDefalutCoolTime;

            enemy.ApplyBuff(spottingWeaknessBuff);

        }
    }

    public override void Exit()
    {

    }


}
