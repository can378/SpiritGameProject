using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : EnemyBasic
{
    public List<GameObject> faces;
    //�г�=�Ӹ��� ��, �� ���� �� ������ �������� ���ƴٴ�
    protected override void AttackPattern()
    {
        print("angry");
        StartCoroutine(angry());
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
