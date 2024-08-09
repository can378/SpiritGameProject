using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : EnemyBasic
{
    public List<GameObject> faces;
    //분노=머리랑 귀, 코 에서 불 나오고 랜덤으로 돌아다님
    protected override void AttackPattern()
    {
        print("angry");
        StartCoroutine(angry());
    }
    // protected override void Move()
    // {
    //     // 경직 중에는 직접 이동 불가
    //     if (enemyStatus.isFlinch)
    //     {
    //         return;
    //     }
    //     else if (enemyStatus.isRun)
    //     {
    //         if (enemyStatus.enemyTarget)
    //         {
    //             rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
    //         }
    //         return;
    //     }

    //     MovePattern();

    //     rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    // }

    protected override void MovePattern()
    {
        RandomMove();
    }
    IEnumerator angry()
    {
        //enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        foreach(GameObject f in faces) 
        {
            f.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        foreach (GameObject f in faces)
        {
            f.SetActive(false);
        }



        //enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;

    }
}
