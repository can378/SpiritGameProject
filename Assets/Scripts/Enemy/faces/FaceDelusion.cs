using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDelusion : EnemyBasic
{
    //착각=무작위로 총 발사

    protected override void AttackPattern()
    {
        StartCoroutine(delusion());
    }


    protected override void Move()
    {
        // 경직 중에는 직접 이동 불가
        if (enemyStatus.isFlinch)
        {
            return;
        }
        else if (enemyStatus.isRun)
        {
            if (enemyStatus.enemyTarget)
            {
                rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
            }
            return;
        }

        MovePattern();

        rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    }

    protected override void MovePattern()
    {
        RandomMove();
    }

    IEnumerator delusion()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //START
        //여러개 한번에 발사
        for (int i = 0; i < 7; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();


            Vector2 dirVec = enemyStatus.enemyTarget.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-360f, 360f), Random.Range(0f, 100f));
            dirVec += ranVec;
            bulletRigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }
        rigid.AddForce(enemyStatus.targetDirVec * 20);


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

}
