using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDelusion : EnemyBasic
{
    //����=�������� �� �߻�

    protected override void AttackPattern()
    {
        StartCoroutine(delusion());
    }

    IEnumerator delusion()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
