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
    private int roundNum;//multishot몇갈래 발포하는지 변수

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
            case enemyPatterns.multiShot: 
                roundNum = EnemyGunMuzzle.Count; 
                multiShot(); 
                break;
            case enemyPatterns.hitAndRun: StartCoroutine("hitAndRun"); break;
            case enemyPatterns.rangeAttack: StartCoroutine("rangeAttack"); break;
            case enemyPatterns.waveAttack: StartCoroutine("waveAttack"); break;
            default: break;
        }
    }

    

    //Patterns===============================================================================

    IEnumerator LRShot()
    {
        dirVec = (enemyTarget.transform.position - transform.position).normalized;

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        { rb.AddForce(dirVec * walkSpeed); }

        yield return new WaitForSeconds(0.1f);

        rb.velocity = Vector2.zero;
        shot();
        yield return new WaitForSeconds(3);
        StartCoroutine("LRShot");
    }

    private void shot()
    {
        GameObject bullet = ObjectPoolManager.instance.Get(0);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        bullet.transform.position = transform.position;
        dirVec = (enemyTarget.transform.position - transform.position).normalized;
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
    }



    IEnumerator rushHit() //돌진 후 대기 (반복)
    {
        dirVec = (enemyTarget.transform.position - transform.position).normalized;
  
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        { rb.AddForce(dirVec * walkSpeed); }
        
        yield return new WaitForSeconds(0.1f);
       
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(3);
        StartCoroutine("rushHit");
    }





    IEnumerator hitAndRun()
    {
        //getting closer
        do
        {
            Chase();
            targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
            yield return new WaitForSeconds(0.01f);
        } while (targetDistance > 1.2f);


        //getting farther
        do 
        {
            rb.AddForce(-dirVec * walkSpeed,ForceMode2D.Impulse);
            targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
            dirVec = (enemyTarget.transform.position - transform.position).normalized;
            yield return new WaitForSeconds(0.01f);
        } while (targetDistance < 10f);


        yield return new WaitForSeconds(0.01f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);

        StartCoroutine("hitAndRun");
    }


    //필요할 경우 활용? 아직 안씀
    IEnumerator moveEllipse() 
    {
        //dirVec = (enemyTarget.transform.position - transform.position).normalized;
        int r = 10;

        for (int theta = 0; theta < 360; theta++)
        {
            transform.position = new Vector3(r * Mathf.Cos(theta * Mathf.Deg2Rad) * 0.5f, r * Mathf.Sin(theta * Mathf.Deg2Rad));
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine("moveEllipse");
    }

    

    //multi shot=n갈래로 총을쏜다.
    void multiShot() 
    {

        if (!isActiveAndEnabled) { return; }//죽었으면 종료
        
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
