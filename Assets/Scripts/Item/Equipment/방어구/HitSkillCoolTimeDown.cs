using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSkillCoolTimeDown : Equipment
{
    // 플레이어가 hitTarget 성공시 variation만큼 현재 스킬 쿨타임 감소
    [SerializeField] float variation;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null || !user.playerStatus.hitTarget || user.playerStats.skill[user.playerStatus.skillIndex] == 0)
            return;

        if (user.playerStatus.hitTarget.gameObject.tag == "Wall" || user.playerStatus.hitTarget.gameObject.tag == "Door" || user.playerStatus.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        user.skillList[user.playerStats.skill[user.playerStatus.skillIndex]].skillCoolTime -= variation;
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어가 기본 공격 성공 시 스킬 쿨타임 감소");
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = null;
        }
    }
}
