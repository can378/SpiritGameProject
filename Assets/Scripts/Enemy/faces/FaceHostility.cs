using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHostility : EnemyBasic
{

    //���밨=�� ��ä�� ������� �߻�
    protected override void AttackPattern()
    {
        StartCoroutine(hostility());
    }

    IEnumerator hostility()
    {
        isAttack = true;
        isAttackReady = false;

        if (!gameObject.activeSelf)
            yield break;

        //������ �ѹ��� �߻�
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();


            Vector2 dirVec = enemyTarget.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }



        //Repeat
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
