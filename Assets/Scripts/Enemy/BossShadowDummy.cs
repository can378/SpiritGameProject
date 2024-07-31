using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadowDummy : EnemyBasic
{
    

    protected override void MovePattern()
    {
        enemyStatus.moveVec = (enemyStatus.enemyTarget.position - transform.position).normalized;
    }
    protected override void AttackPattern()
    {
        StartCoroutine(shadowShot());
    }

    IEnumerator shadowShot() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        shot();

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

    public override void Dead()
    {
        gameObject.SetActive(false);
    }
}
