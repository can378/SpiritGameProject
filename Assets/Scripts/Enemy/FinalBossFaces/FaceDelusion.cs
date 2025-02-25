using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDelusion : BossFace
{
    //착각=무작위로 총 발사
    private bool isReady = true;
    protected override void faceAttack()
    {
        if (isReady) { StartCoroutine(delusion()); }
    }


    IEnumerator delusion()
    {
        isReady = false;
        //여러개 한번에 발사
        for (int i = 0; i < 7; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get("Bullet");
            bullet.transform.position = transform.position;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();


            Vector2 dirVec = enemyStatus.enemyTarget.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-360f, 360f), Random.Range(0f, 100f));
            dirVec += ranVec;
            bulletRigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }
        rigid.AddForce(enemyStatus.targetDirVec * 20);

        yield return new WaitForSeconds(4f);
        isReady = true;
    }
    protected override void MovePattern()
    {
        RandomMove();
    }

}
