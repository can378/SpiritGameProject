using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Crow : EnemyBasic
{
    public Transform[] ThrowPos;
    string[] Param = {"isAttack1","isAttack2"};

    protected override void AttackPattern()
    {
        enemyStatus.attackCoroutine = StartCoroutine(LRShot());
    }

    IEnumerator LRShot()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            // 무작위 공격 애니메이션 실행
            int RandomParam = Random.Range(0, Param.Length);
            enemyAnim.animator.SetBool(Param[RandomParam], true);
            enemyAnim.ChangeDirection(enemyStatus.targetDirVec);
            yield return new WaitForSeconds(0.5f);         // 던지기 모션 

            // 보석 던지기
            GameObject bullet = ObjectPoolManager.instance.Get2("jewel");
            bullet.transform.position = ThrowPos[RandomParam].position;
            Vector3 TargetDirVec = (enemyStatus.enemyTarget.position - ThrowPos[RandomParam].position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(TargetDirVec * 7, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.4f);

            // 공격 애니메이션 종료
            enemyAnim.animator.SetBool(Param[RandomParam], false);

            yield return null;
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
