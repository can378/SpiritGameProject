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


    protected override void Move()
    {
        // ���� �߿��� ���� �̵� �Ұ�
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

    IEnumerator delusion()
    {
        isAttack = true;
        isAttackReady = false;

        //START
        //������ �ѹ��� �߻�
        for (int i = 0; i < 7; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();


            Vector2 dirVec = enemyTarget.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-360f, 360f), Random.Range(0f, 100f));
            dirVec += ranVec;
            bulletRigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }
        rigid.AddForce(targetDirVec * 20);


        //END
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }

}
