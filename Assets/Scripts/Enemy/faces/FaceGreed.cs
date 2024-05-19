using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGreed : EnemyBasic
{

    //탐욕=입을 크게 벌린다. 이와중에 혓바닥이 길어지는데 혓바닥에 닿으면 안된다. 혓바닥은 지워지지않고 이 페이즈 끝날때까지 남아있어서 무빙을 잘 생각하고 쳐야한다.

    protected override void AttackPattern()
    {
        StartCoroutine(greed());
    }

    IEnumerator greed()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
