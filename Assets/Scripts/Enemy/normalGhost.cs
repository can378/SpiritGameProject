using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGhost : EnemyBasic
{
    //ÀÏ¹Ý±Í½Å. ÃÑ¾Ë(ÇÑ)À» ½ð´Ù.

    protected override void AttackPattern()
    {
        // ¿ø°Å¸® °ø°Ý
        if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
            enemyStatus.attackCoroutine = StartCoroutine(Throw());
        }
    }

    

    IEnumerator Throw()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        enemyAnim.animator.SetBool("isAttack", true);
        for (int i = 0; i < 3; i++)
        {
            enemyAnim.animator.Play("NomalGhost_Attack", -1, 0f);
            enemyAnim.ChangeDirection(enemyStatus.targetDirVec);
            yield return new WaitForSeconds(0.5f);

            GameObject knife = ObjectPoolManager.instance.Get2("knife");
            knife.transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }
        enemyAnim.animator.SetBool("isAttack", false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
