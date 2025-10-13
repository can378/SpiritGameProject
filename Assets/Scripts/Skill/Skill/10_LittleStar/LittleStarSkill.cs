using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleStarSkill : SkillBase
{
    [field: SerializeField] LittleStarSkillData LSSData;

    protected void Awake()
    {
        skillData = LSSData;
    }
    public override void Enter(ObjectBasic user)
    {
        base.Enter(user);
        Summon();
    }

    public override void Cancle()
    {
        
    }

    public override void Exit()
    {

    }

    void Summon()
    {
        Debug.Log("LittleStar");

        if (user.tag == "Player")
        {
            Player player = this.user.GetComponent<Player>();

            // ��Ÿ�� ����
            skillCoolTime = (1 + player.playerStats.SkillCoolTime.Value) * LSSData.skillDefalutCoolTime;

            player.ApplyBuff(LSSData.LSBuff);

        }
        else if (user.tag == "Enemy")
        {

            ObjectBasic objectBasic = this.user.GetComponent<ObjectBasic>();

            // ��Ÿ�� ����
            skillCoolTime = LSSData.skillDefalutCoolTime;

            objectBasic.ApplyBuff(LSSData.LSBuff);


        }
    }

    
}
