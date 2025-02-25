using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerWoman : EnemyBasic
{
    protected override void AttackPattern()
    {
        if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(deerWoman());
            return;
        }
    }


    IEnumerator deerWoman()
    {
        Vector3 shotDir = enemyStatus.targetDirVec;

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        //shot arrow
        GameObject bullet = ObjectPoolManager.instance.Get("Bullet");
        bullet.transform.position = transform.position;

        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        rigidBullet.AddForce(shotDir.normalized * 3, ForceMode2D.Impulse);
    
        yield return new WaitForSeconds(2f);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

    }
}
