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
        Vector3 shotDir = targetDirVec;

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        //shot arrow
        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;

        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        rigidBullet.AddForce(shotDir.normalized * 3, ForceMode2D.Impulse);
    
        yield return new WaitForSeconds(2f);

        isAttack = false;
        isAttackReady = true;

    }
}
