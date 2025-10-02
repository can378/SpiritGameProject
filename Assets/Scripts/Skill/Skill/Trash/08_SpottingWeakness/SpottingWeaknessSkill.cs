using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottingWeaknessSkill : SkillBase
{
    // ���ط�
    [field: SerializeField] BuffData spottingWeaknessBuff;

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

            // ��Ÿ�� ����
            skillCoolTime = (1 - player.playerStats.skillCoolTime) * 9999999999;

            player.ApplyBuff(spottingWeaknessBuff);
        }
        else if (user.tag == "Enemy")
        {
            EnemyBasic enemy = user.GetComponent<EnemyBasic>();

            // ��Ÿ�� ����
            skillCoolTime =99999999;

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
