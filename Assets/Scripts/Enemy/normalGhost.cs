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
            //attack animation
            enemyAnim.animator.Play("NomalGhost_Attack", -1, 0f);
            enemyAnim.ChangeDirection(enemyStatus.targetDirVec);
            yield return new WaitForSeconds(0.5f);

            //throw knife
            GameObject knife = ObjectPoolManager.instance.Get("knife");
            HitDetection HD = knife.GetComponent<HitDetection>();
            HD.SetGuiding(true, enemyStatus.EnemyTarget.CenterPivot, 20, 0.3f, true);
            Debug.Log(knife.name+" throwing");


            knife.transform.position = CenterPivot.transform.position;
            knife.transform.rotation = Quaternion.LookRotation(Vector3.forward, enemyStatus.targetDirVec);
            //Vector3 distTest= knife.transform.position - CenterPivot.transform.position;
            //Debug.Log("distance from knife to enmey" +distTest.x+" "+distTest.y+" "+distTest.z);
            yield return new WaitForSeconds(1f);
        }
        enemyAnim.animator.SetBool("isAttack", false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(5f);
        enemyStatus.isAttackReady = true;

    }
}
