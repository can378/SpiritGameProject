using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyAttack
{ 
    rushHit, multiShot, hitAndRun, 
    rangeAttack, waveAttack,
    pop,
    chase,jump ,None,
    wander,crow, whiteFox, snowBall,worm,frog,broomStick
};


public class EnemyAttack : EnemyPattern
{
    public enemyAttack enemyAttack;

    [Header("multiShot")]
    public List<GameObject> EnemyGunMuzzle;

    [Header("waveAttack")]
    public GameObject donutAttackRange;
    public GameObject donutInside;

    [Header("rangeAttack")] 
    public GameObject roundAttackRange;

    [Header("Beam")]
    public GameObject beam;

    private void Start()
    {
        //if (this.transform.gameObject.activeSelf != false) { EnemyPatternStart(); }
    }

    private void OnEnable()
    {
        EnemyPatternStart();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        isPARun = false;
        isHARRun = false;
    }

    void EnemyPatternStart()
    {
        switch (enemyAttack)
        {
            case enemyAttack.rushHit: StartCoroutine(rushHit(true)); break;
            case enemyAttack.multiShot:StartCoroutine(multiShot(EnemyGunMuzzle.Count, EnemyGunMuzzle,true)); break;
            case enemyAttack.hitAndRun: StartCoroutine(hitAndRun(true)); break;
            case enemyAttack.rangeAttack: StartCoroutine(rangeAttack(roundAttackRange,true)); break;
            case enemyAttack.waveAttack: StartCoroutine(waveAttack(donutAttackRange, donutInside,true)); break;
            case enemyAttack.pop: StartCoroutine(pop(true)); break;
            case enemyAttack.worm:
            case enemyAttack.chase:StartCoroutine(chasing());break;
            case enemyAttack.jump: StartCoroutine(jump(true)); break;
            case enemyAttack.wander:StartCoroutine(Wander(true));break;
            case enemyAttack.crow:StartCoroutine(LRShot(true));break;
            case enemyAttack.whiteFox: StartCoroutine(whiteFox()); break;
            case enemyAttack.snowBall: StartCoroutine(snowBall());break;
            case enemyAttack.frog:StartCoroutine(frog());break;
            case enemyAttack.broomStick:StartCoroutine(peripheralAttack(20, 2, true));break;
            default: break;
        }
    }
    IEnumerator whiteFox() 
    {

        StartCoroutine(hitAndRun(false));

        while (isHARRun == true) 
        { yield return new WaitForSeconds(0.1f); }

        StartCoroutine(peripheralAttack(10, 5, false));

        while (isPARun == true)
        { yield return new WaitForSeconds(0.1f); }

        yield return new WaitForSeconds(3f);

        StartCoroutine(whiteFox());
    }

    private bool isSnowBallAngry = false;
    IEnumerator snowBall() 
    { 
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDis > GetComponent<EnemyStats>().detectionDis && isSnowBallAngry == false)
        {
            //roll
            print("Roll");
            
            float randomDirection = Random.Range(0f, 1f) < 0.5f ? -1f : 1f;
            rigid.AddTorque(randomDirection * GetComponent<EnemyStats>().defaultSpeed*100);


            targetDirVec = (enemyTarget.position - transform.position).normalized;
            targetDirVec.y = 0;
            var rotation = Quaternion.LookRotation(targetDirVec);


            transform.Translate(Vector3.forward * Time.deltaTime * GetComponent<EnemyStats>().defaultSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);


            //stronger
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(1f);
        }
        else { isSnowBallAngry = true; } 
        
        
        if(isSnowBallAngry==true)
        {
            print("snow ball attack");
            yield return new WaitForSeconds(3f);
            //attack
            targetDirVec = (enemyTarget.position - transform.position).normalized;

            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultSpeed*3);
            yield return new WaitForSeconds(3f);

            while (targetDis > 0.1f && isSnowBallAngry == true)
            {
                targetDirVec = (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultSpeed*10);
                
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        
        StartCoroutine(snowBall());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyAttack == enemyAttack.snowBall)
        {
            //stop attacking if is hitted by something
            isSnowBallAngry = false;
            transform.localScale = new Vector3(1, 1, 1);
        
        }
        if (enemyAttack == enemyAttack.worm&& collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(worm(3));
        
        }
    }
    IEnumerator worm(float time) 
    {
        sprite.sortingOrder = enemyTarget.GetComponent<SpriteRenderer>().sortingOrder + 1;
        transform.parent = enemyTarget.transform;
        transform.position = enemyTarget.position;
        //player move slowly
        enemyTarget.GetComponent<PlayerStats>().defaultSpeed -= 3;
        yield return new WaitForSeconds(time);
        enemyTarget.GetComponent<PlayerStats>().defaultSpeed += 3;
        Destroy(this.gameObject);
    }

    IEnumerator frog()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        

        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //get closer to player
            targetDirVec = enemyTarget.position - transform.position;
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultSpeed);

        }
        else
        {

            //attack
            yield return new WaitForSeconds(2f);
            targetDirVec = enemyTarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirVec);
            transform.rotation = rotation;

            //StartCoroutine(Beam(beam, 2, 10, false,enemyTarget.gameObject));

            /*
            yield return new WaitForSeconds(2f);

            while (isBeamRun == true) { yield return new WaitForSeconds(1f); }
            
            //run away
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultSpeed * 3);
            yield return new WaitForSeconds(1f);
            */

        }

        StartCoroutine(frog());
    
    }

}
