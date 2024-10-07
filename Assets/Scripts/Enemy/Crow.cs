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
            GameObject bullet = ObjectPoolManager.instance.Get2("jewel");
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody2D>().AddForce(enemyStatus.targetDirVec.normalized * 7, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
