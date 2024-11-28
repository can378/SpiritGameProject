using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTree : EnemyBasic
{
    protected override void MovePattern()
    {
        enemyStatus.isRun = false;
    }

    protected override void AttackPattern()
    {
        if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(fakeTree());
            return;
        }
    }

    // 근처에 있으면 공격

    IEnumerator fakeTree()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        //sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        //sprite.color = new Color(255, 255, 255);


    }



}
