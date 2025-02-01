using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingWeaknessSkill : Skill
{
    // 피해량
    [field: SerializeField] int spottingWeaknessBuff;

    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        Buff();
    }

    void Buff()
    {
        Debug.Log("SpottingWeakness");

        if(user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // 쿨타임 적용
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * skillDefalutCoolTime;

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

    public override void Cancle()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {

    }


}
