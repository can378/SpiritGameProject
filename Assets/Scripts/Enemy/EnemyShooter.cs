using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    int health;
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    public GameObject bullet2;
    public GameObject player;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnEnable()
    {
        health = 300;
        Invoke("Stop",2f);
    }
    void Think() 
    {

        //현재 패턴 개수 넘어가면 원래대로 돌아온다.
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0: FireForward(); break;
            case 1: FireShot(); break;
            case 2: FireArc(); break;
            case 3: FireAround(); break;
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


    void FireForward() 
    { 
        
        //FireForward
        Debug.Log("FireForward");

        bullet2.transform.position = transform.position + Vector3.right * 0.5f;

        Rigidbody2D rigidBullet = bullet2.GetComponent<Rigidbody2D>();
        rigidBullet.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        


        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 1f);
        else
            Invoke("Think", 2f);
    }


    void FireShot() 
    { 
        
        Debug.Log("Fireshot");
        for (int i = 0; i < 5; i++)
        {
            bullet2.transform.position = transform.position;

            Rigidbody2D rigid = bullet2.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        }



        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 1f);
        else
            Invoke("Think", 2f);


    }


    void FireArc() 
    { 
        Debug.Log("FireArc");
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.5f);
        else
            Invoke("Think", 2f);

    }


    void FireAround() 
    { 
        
        Debug.Log("FireAround");

        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet2 = ObjectPoolManager.instance.Get(0);
            bullet2.transform.position = transform.position;
            bullet2.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet2.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum)
                , Mathf.Sin(Mathf.PI * 2 * i / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 260 * i / roundNum + Vector3.forward * 90;
            bullet2.transform.Rotate(rotVec);
        
        }

        //Repeat
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 1.5f);
        else
        {
            Invoke("Think", 2f);
            ObjectPoolManager.instance.Clear(0);
        }
            
    }
}
