using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBasic
{
    private BossRoom bossRoom;

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public override void Dead()
    {
        bossRoom=GameManager.instance.nowRoomScript.map.GetComponent<BossRoom>();
        if (bossRoom != null) { bossRoom.bossDead = true; }
        base.Dead();

    }
}
