using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : EnemyBasic
{
    //분노=머리랑 귀, 코 에서 불 나오고 랜덤으로 돌아다님
    protected override void AttackPattern()
    {
        StartCoroutine(angry());
    }

    IEnumerator angry()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
