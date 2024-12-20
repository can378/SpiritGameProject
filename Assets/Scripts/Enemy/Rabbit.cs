using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : EnemyBasic
{
    protected override void AttackPattern()
    {
        StartCoroutine(rabbit());
    }

    IEnumerator rabbit()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

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
