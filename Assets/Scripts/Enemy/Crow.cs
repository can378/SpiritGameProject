using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : EnemyBasic
{
    protected override void AttackPattern()
    {
        StartCoroutine(LRShot());
    }

    IEnumerator LRShot()
    {
        isAttack = true;
        isAttackReady = false;

        for (int i = 0; i < 2; i++)
        {
            shot();
            yield return new WaitForSeconds(0.2f);
        }

        isAttack = false;

        yield return new WaitForSeconds(3f);

        isAttackReady = true;

    }
}
