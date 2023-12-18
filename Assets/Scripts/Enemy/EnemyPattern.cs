using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyPatterns
{ rushHit, multiShot, hitAndRun, rangeAttack, waveAttack };


public class EnemyPattern : EnemyBasic
{
    public enemyPatterns ePattern;
    private Rigidbody2D rb;
    private Vector2 dirVec;
    private float time = 0;
    private float targetDistance;
    public List<GameObject> EnemyGunMuzzle;
    public GameObject donutAttackRange;
    public GameObject roundAttackRange;
    public GameObject donutInside;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        EnemyPatternStart();

    }
    private void Update()
    {
        //targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
    }

    void EnemyPatternStart() 
    {
        switch (ePattern)
        {
            case enemyPatterns.rushHit: StartCoroutine("rushHit"); break;
            case enemyPatterns.multiShot: multiShot(); break;
            case enemyPatterns.hitAndRun: StartCoroutine("hitAndRun"); break;
            case enemyPatterns.rangeAttack: StartCoroutine("rangeAttack"); break;
            case enemyPatterns.waveAttack: StartCoroutine("waveAttack"); break;
            default: break;
        }
    }





    //rush hit와 유사하므로 아직 보류
    IEnumerator LRShot()
    {
        Vector2 leftV = new Vector2(-1f, 0f);
        Vector2 rightV = new Vector2(1f, 0f);
        //left
        for (int i = 0; i < 3; i++)
        { rb.AddForce(leftV * 10f); }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        { rb.AddForce(rightV * 10f); }

        //shot
        shot();

        //right1
        for (int i = 0; i < 3; i++)
        { rb.AddForce(rightV * 10f);}
        for (int i = 0; i < 3; i++)
        { rb.AddForce(leftV * 10f); }
        yield return new WaitForSeconds(2f);
        //shot
        shot();

        yield return new WaitForSeconds(3);
        StartCoroutine("LRShot");
    }




    //대기 제대로 안하는 문제
    IEnumerator rushHit() //돌진 후 대기 (반복)
    {
        print("rushHit");
        time = 0;
        dirVec = (enemyTarget.transform.position - transform.position).normalized;
        //Vector2 direction = enemyTarget.transform.position - transform.position;
        rb.velocity = Vector2.zero;
        while (time<1f)
        {
            time += Time.deltaTime;
            //transform.Translate(direction * 0.001f);
            rb.AddForce(dirVec*walkSpeed);
        }
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(3);
        StartCoroutine("rushHit");
    }


    private void shot() 
    {
        GameObject bullet = ObjectPoolManager.instance.Get(0);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        bullet.transform.position = transform.position;
        dirVec = (enemyTarget.transform.position - transform.position).normalized;
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
    }





    //hit and run = 때리고 도망가고
    
    IEnumerator hitAndRun() 
    {
        /*
        //initialize
        time = 0;
        dirVec = (enemyTarget.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero;

        //HIT
        while (targetDistance>=0)
        {
            Chase();
        }

        //Run
        rb.velocity = Vector2.zero;
        */
        yield return new WaitForSeconds(10);
    
        StartCoroutine("hitAndRun");

    }

    

    //multi shot=n갈래로 총을쏜다.
    void multiShot() 
    {
        int roundNum = EnemyGunMuzzle.Count;//roundNum갈래의 총알 발포
        print("multi shot");
        
        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get(0);
            bullet.transform.position = EnemyGunMuzzle[i].transform.position;
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
        Invoke("multiShot", 1.5f);
        
    }



    //range attack = 범위 공격
    IEnumerator rangeAttack() 
    {
        yield return new WaitForSeconds(3);


        float newScale = 1f;
        while (newScale <= 4f)
        {
            newScale += Time.deltaTime;
            roundAttackRange.transform.localScale 
                = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.02f);
        }
        
        

        StartCoroutine("rangeAttack");
    }

    //wave attack = 파장 범위 공격
    IEnumerator waveAttack() 
    {
        yield return new WaitForSeconds(3);


        float newScale = 1f;
        while (newScale <= 9f)
        {
            newScale += Time.deltaTime;
            donutAttackRange.transform.localScale
                = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.001f);
        }

        newScale = 0.5f;
        while (newScale <= 1f)
        {
            newScale += Time.deltaTime;
            donutInside.transform.localScale
                = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.001f);
        }

        donutInside.transform.localScale
                = new Vector3(0.5f, 0.5f, 1f);
        donutAttackRange.transform.localScale
                = new Vector3(1f, 1f, 1f);
        StartCoroutine("waveAttack");

    }
}
