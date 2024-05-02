using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBasic
{

    public int patternIndex;//패턴 인덱스 
    public int curPatternCount;
    public int[] maxPatternCount;


    private void OnEnable()
    {
        rigid.velocity = Vector2.zero;
        StartNamedCoroutine("Think", Think());
    }


    IEnumerator Think() 
    {
        yield return null;
        ObjectPoolManager.instance.Clear(0);
        //현재 패턴 개수 넘어가면 원래대로 돌아온다.
        patternIndex = patternIndex == 2 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0: StartCoroutine(FireForward()); break;
            case 1: StartCoroutine(FireShot()); break;
            case 2: StartCoroutine(FireAround()); break;
        }
    }





    //Fire Patterns===========================================================================
    IEnumerator FireForward() 
    {
        if (!gameObject.activeSelf)
            yield break;

        //한발씩 발사

        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        //bullet.transform.position = transform.position + Vector3.right * 0.5f;
        bullet.transform.position = transform.position;


        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = enemyTarget.transform.position - transform.position;
        rigidBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FireForward());
        }
        else
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(Think());
        }
           
    }


    IEnumerator FireShot() 
    {
        if (!gameObject.activeSelf)
            yield break;

        //여러개 한번에 발사
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
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        { yield return new WaitForSeconds(1f); StartCoroutine(FireShot()); }
        else
        { yield return new WaitForSeconds(4f); StartCoroutine(Think()); }

    }



    IEnumerator FireAround() 
    {
        if (!gameObject.activeSelf) yield break;

        //원형 발사
        int roundNumA = 30;
        int roundNumB = 20;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(
                Mathf.Cos(Mathf.PI * 2 * i / roundNum), 
                Mathf.Sin(Mathf.PI * 2 * i / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 260 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        
        }




        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        { yield return new WaitForSeconds(1.5f);StartCoroutine(FireAround()); }
        else
        {
            yield return new WaitForSeconds(5f); StartCoroutine(Think());
        }
            
    }


}
