using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossShadowDummy : EnemyBasic
{
    protected override void MovePattern()
    {
        enemyStatus.moveVec = (enemyStatus.EnemyTarget.CenterPivot.position - transform.position).normalized;
    }
    protected override void AttackPattern()
    {
        enemyStatus.attackCoroutine = StartCoroutine(shadowShot());
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
        print(this.name + " Dead");

        RemoveAllEffects();
        AttackCancle();
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
