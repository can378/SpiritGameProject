using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerWoman : EnemyBasic
{
    protected override void AttackPattern()
    {
        if (targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(deerWoman());
            return;
        }
    }


    IEnumerator deerWoman()
    {
        isAttack = true;
        isAttackReady = false;

        //shot arrow
        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;


        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        targetDirVec = (enemyTarget.position - transform.position).normalized;
        rigidBullet.AddForce(targetDirVec.normalized * 3, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(2f);

        isAttack = false;
        isAttackReady = true;

    }
}
