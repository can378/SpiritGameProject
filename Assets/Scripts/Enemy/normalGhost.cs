using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGhost : EnemyBasic
{
    //�Ϲݱͽ�. �Ѿ�(��)�� ���.

    protected override void AttackPattern()
    {
        // ���Ÿ� ����
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
            //attack animation
            enemyAnim.animator.Play("NomalGhost_Attack", -1, 0f);
            enemyAnim.ChangeDirection(enemyStatus.targetDirVec);
            yield return new WaitForSeconds(0.5f);

            //throw knife
            GameObject knife = ObjectPoolManager.instance.Get("knife");
            Debug.Log(knife.name+" throwing");


            knife.transform.position = transform.position;
            Vector3 distTest= knife.transform.position - gameObject.transform.position;
            Debug.Log("distance from knife to enmey" +distTest.x+" "+distTest.y+" "+distTest.z);
            yield return new WaitForSeconds(1f);
        }
        enemyAnim.animator.SetBool("isAttack", false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
