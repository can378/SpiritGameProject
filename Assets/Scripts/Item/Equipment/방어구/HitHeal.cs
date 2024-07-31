using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHeal : Equipment
{
    // 플레이어가 공격 성공 시 variation 만큼 회복
    [SerializeField] float variation;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null || !user.playerStatus.hitTarget)
            return;

        if (user.playerStatus.hitTarget.gameObject.tag == "Wall" || user.playerStatus.hitTarget.gameObject.tag == "Door" || user.playerStatus.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        user.Damaged(-variation);
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어가 기본 공격 성공 시 회복");
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
