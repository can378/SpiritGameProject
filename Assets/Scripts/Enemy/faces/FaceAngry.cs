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
    protected override void Move()
    {
        // 경직 중에는 직접 이동 불가
        if (isFlinch)
        {
            return;
        }
        else if (isRun)
        {
            if (enemyTarget)
            {
                rigid.velocity = -(enemyTarget.position - transform.position).normalized * stats.moveSpeed;
            }
            return;
        }

        MovePattern();

        rigid.velocity = moveVec * stats.moveSpeed;

    }

    protected override void MovePattern()
    {
        RandomMove();
    }
    IEnumerator angry()
    {
        isAttack = true;
        isAttackReady = false;

        foreach(GameObject f in faces) 
        {
            f.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        foreach (GameObject f in faces)
        {
            f.SetActive(false);
        }



        isAttack = false;
        yield return new WaitForSeconds(1f);
        isAttackReady = true;

    }
}
