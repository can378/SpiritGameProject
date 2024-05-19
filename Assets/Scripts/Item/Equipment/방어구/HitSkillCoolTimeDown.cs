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
        if (user == null || !user.hitTarget)
            return;

        if (user.hitTarget.gameObject.tag == "Wall" || user.hitTarget.gameObject.tag == "Door" || user.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        user.skillController.skillList[user.playerStats.skill[user.status.skillIndex]].skillCoolTime -= variation;
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
