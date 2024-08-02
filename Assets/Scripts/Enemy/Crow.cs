using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : EnemyBasic
{
    protected override void AttackPattern()
    {
        enemyStatus.attackCoroutine = StartCoroutine(LRShot());
    }

    IEnumerator LRShot()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            shot();
            yield return new WaitForSeconds(0.2f);
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
