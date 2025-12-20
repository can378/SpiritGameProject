using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBasic
{
    private BossRoom bossRoom;

    public virtual void BossCutScene()
    {
        
    }

    // 보스는 플레이어 감지 로직이 다름
    protected override void Detect()
    {
        // BossRoom이 타겟을 설정해준다.
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public override void Dead()
    {
        bossRoom=GameManager.instance.nowRoomScript.map.GetComponent<BossRoom>();

        //if (bossRoom != null) { bossRoom.bossDead = true; }
        
        base.Dead();

    }
}
