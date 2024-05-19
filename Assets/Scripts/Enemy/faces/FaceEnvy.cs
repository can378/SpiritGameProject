using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnvy : EnemyBasic
{
    //질투=스킬모방(쥐 처럼 대신 강도는 더 강하게)

    protected override void AttackPattern()
    {
        StartCoroutine(envy());
    }

    IEnumerator envy()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
