using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyAttack
{ 
    rushHit, multiShot, hitAndRun, 
    rangeAttack, waveAttack,
    pop,
    chase,jump ,None,
    wander,crow, whiteFox, snowBall,worm,frog,broomStick,pox
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

    [Header("BroomStick")]
    public GameObject colObj;

    public GameObject buff;

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
            case enemyAttack.snowBall: StartCoroutine(snowBall(new Vector2(-1,0)));break;
            case enemyAttack.frog:StartCoroutine(frog());break;
            case enemyAttack.broomStick:
                StartCoroutine(peripheralAttack(colObj,20, 2, true));
                //두통 디버프 5초?
                break;
            case enemyAttack.pox:break;
            default: break;
        }
    }
    IEnumerator whiteFox() 
    {

        StartCoroutine(hitAndRun(false));

        while (isHARRun == true) 
        { yield return new WaitForSeconds(0.1f); }

        StartCoroutine(peripheralAttack(this.gameObject,10, 5, false));

        while (isPARun == true)
        { yield return new WaitForSeconds(0.1f); }

        yield return new WaitForSeconds(3f);

        StartCoroutine(whiteFox());
    }

    private bool isSnowBallAngry = false;
    private bool isHit = false;
    //private bool isSnowBallPause = false;
    IEnumerator snowBall(Vector2 dir) 
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);

        
        //ROLLING
        float rightOrLeft = Vector2.Dot(rigid.velocity.normalized, Vector2.right);
        if (rightOrLeft > 0)
        {
            //move right
            transform.Rotate(new Vector3(0, 0, -GetComponent<EnemyStats>().defaultMoveSpeed));
        }
        else if (rightOrLeft < 0)
        {
            //move left
            transform.Rotate(new Vector3(0, 0, GetComponent<EnemyStats>().defaultMoveSpeed));
        }




        


        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //MOVE
            //setting random dirVec
            if (isHit == true)
            {
                rigid.velocity = new Vector2(0,0);
                yield return new WaitForSeconds(0.1f);
                //float randomAngle = Random.Range(0, 360);
                dir *= -1;
                //dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;//random direction
                dir += new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                isHit = false;
            }
            rigid.AddForce(dir * GetComponent<EnemyStats>().defaultMoveSpeed * 20);
            yield return new WaitForSeconds(0.1f);

            //STRONGER
            if (transform.localScale.x <= 4)
            {
                transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
        //ATTACK
        else
        {
            print("snow ball attack");

            isSnowBallAngry = true;

            rigid.velocity = new Vector3(0,0,0);
            yield return new WaitForSeconds(1f);

            //move Backward
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 20);

            //rush to target
            while (targetDis > 0&&isSnowBallAngry==true)
            {
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                //targetDirVec = (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 50);
                yield return new WaitForSeconds(0.1f);
                print("rush snowball");
            }

            //pause
            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(4f);
            isSnowBallAngry = false;

        } 
        
        

        //NEXT
        StartCoroutine(snowBall(dir));
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyAttack == enemyAttack.snowBall)
        {
            //stop attacking if is hitted by something
            isSnowBallAngry = false;
            isHit = true;
            transform.localScale = new Vector3(1, 1, 1);
        
        }
        if (enemyAttack == enemyAttack.worm&& collision.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(worm(3));
        
        }
        if (enemyAttack==enemyAttack.broomStick&&collision.tag=="Player") 
        {
            //두통 디버프 5초
            //ApplyBuff(buff);
            enemyTarget.GetComponent<Player>().ApplyBuff(buff);
        }
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }
    }
    IEnumerator worm(float time) 
    {
        //sprite.sortingOrder = enemyTarget.GetComponent<SpriteRenderer>().sortingOrder + 1;
        //transform.parent = enemyTarget.transform;
        //transform.position = enemyTarget.position;

        //player move slowly
        enemyTarget.GetComponent<PlayerStats>().defaultMoveSpeed -= 3;
        yield return new WaitForSeconds(time);
        enemyTarget.GetComponent<PlayerStats>().defaultMoveSpeed += 3;
        Destroy(this.gameObject);
    }

    IEnumerator frog()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        

        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //get closer to player
            targetDirVec = enemyTarget.position - transform.position;
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);

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
