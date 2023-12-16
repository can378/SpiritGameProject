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
        Invoke("Stop",2f);
    }


    void Think() 
    {
        ObjectPoolManager.instance.Clear(0);
        //현재 패턴 개수 넘어가면 원래대로 돌아온다.
        patternIndex = patternIndex == 2 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0: FireForward(); break;
            case 1: FireShot(); break;
            case 2: FireAround(); break;
        }
    }


    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        
        Invoke("Think", 2f);
    }




    //Fire Patterns===========================================================================
    void FireForward() 
    { 
        //한발씩 발사

        Debug.Log("FireForward");


        GameObject bullet = ObjectPoolManager.instance.Get(0);
        //bullet.transform.position = transform.position + Vector3.right * 0.5f;
        bullet.transform.position = transform.position;


        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = enemyTarget.transform.position - transform.position;
        rigidBullet.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 1f);
        else
            Invoke("Think", 2f);
    }


    void FireShot() 
    { 
        //여러개 한번에 발사

        Debug.Log("Fireshot");

        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get(0);
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
            Invoke("FireShot", 1f);
        else
            Invoke("Think", 4f);


    }



    void FireAround() 
    { 

        //원형 발사
        Debug.Log("FireAround");

        int roundNumA = 30;
        int roundNumB = 20;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get(0);
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
            Invoke("FireAround", 1.5f);
        else
        {
            Invoke("Think", 6f);
        }
            
    }


}
