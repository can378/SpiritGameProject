using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalGhost : EnemyBasic
{
    //�Ϲݱͽ�. �Ѿ�(��)�� ���.

    protected override void AttackPattern()
    {
        
        // ���Ÿ� ����
        if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(Throw());
        }
    }

    

    IEnumerator Throw()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        for (int i = 0; i < 3; i++)
        {
            GameObject knife = ObjectPoolManager.instance.Get2("knife");
            knife.transform.position = transform.position;
            yield return new WaitForSeconds(1f);
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
