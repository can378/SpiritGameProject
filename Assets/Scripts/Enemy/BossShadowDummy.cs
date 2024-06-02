using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadowDummy : EnemyBasic
{
    

    protected override void MovePattern()
    {
        moveVec = (GameObject.FindWithTag("Player").gameObject.transform.position - transform.position).normalized;
    }
    protected override void AttackPattern()
    {
        StartCoroutine(shadowShot());
    }

    IEnumerator shadowShot() 
    {
        isAttack = true;
        isAttackReady = false;

        shot();

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
