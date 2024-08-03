using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaby : EnemyBasic
{
    
    public GameObject tear;
    public GameObject SafeArea;
    public GameObject DamageArea;
    public GameObject ScreamArea;

    private GameObject floor;
    private int patternIndex = 0;
    private bool isHitWall;
    Bounds bounds;
    float randomX, randomY;
    Vector2 madRushVec;
    Vector3 corner;

    protected override void Start()
    {
        base.Start();
        floor = GameManager.instance.nowRoom;

        enemyStatus.isAttackReady = true;
        bounds = floor.GetComponent<Collider2D>().bounds;
    }

    protected override void Move()
    {
        // 경직과 공격 중에는 직접 이동 불가
        if (enemyStatus.isFlinch)
        {
            return;
        }
        else if (enemyStatus.isAttack)
        {
            rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;
            return;
        }
        else if (enemyStatus.isRun)
        {
            if (enemyStatus.enemyTarget)
            {
                rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
            }
            return;
        }
        rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    }

    protected override void Update()
    {
        base.Update();
        CheckMovement();
    }

    protected override void AttackPattern()
    {
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
       

        switch (patternIndex%5)
        { 
            case 0: yield return StartCoroutine(Rush()); break;
            case 1: yield return StartCoroutine(Hiding()); break;
            case 2: yield return StartCoroutine(Screaming()); break;
            case 3: yield return StartCoroutine(MadRush()); break;
            case 4: yield return StartCoroutine(Crying());break;
        }
        patternIndex++;
        
    }


    IEnumerator Rush() 
    {
        //start
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Rush");
        enemyAnim.changeAnimToAdult();

        float time = 0;

        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);


        while(time<0.2f)
        {
            if (isHitWall == false)
            {
                enemyStatus.moveVec=enemyStatus.targetDirVec;
                yield return new WaitForSeconds(0.1f);
            }
            else 
            {
                enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
                enemyStatus.moveVec = enemyStatus.targetDirVec;
                yield return new WaitForSeconds(0.1f);
                isHitWall = false;
            }
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);

        rigid.velocity = Vector2.zero;

        //end
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }


    IEnumerator Crying() 
    {
        

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Crying");
        enemyAnim.changeAnimToBaby();

        //move to corner
        corner = FindCorner();

        while(Vector2.Distance(transform.position, corner) > 60f) 
        {
            yield return new WaitForSeconds(0.1f);
            enemyStatus.moveVec = (corner - transform.position).normalized;
        }
        rigid.velocity = Vector2.zero;



        //start crying
        for (int i = 0; i < 15; i++)
        { 
            StartCoroutine(DropTear());
            yield return new WaitForSeconds(0.5f);
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }


    IEnumerator DropTear() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        randomX = Random.Range(bounds.min.x, bounds.max.x);
        randomY = Random.Range(bounds.min.y, bounds.max.y);

        GameObject thisTear = Instantiate(tear);
        thisTear.SetActive(true);
        thisTear.transform.position=new Vector2(randomX, randomY);

        thisTear.GetComponent<SpriteRenderer>().color=Color.white;
        yield return new WaitForSeconds(0.5f);
        thisTear.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        Destroy(thisTear);

    }


    IEnumerator MadRush() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("MadRush");
        enemyAnim.changeAnimToAdult();

        randomX = Random.Range(bounds.min.x, bounds.max.x);
        randomY = Random.Range(bounds.min.y, bounds.max.y);
        madRushVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;


        float time = 0;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);


        while (time < 0.3f)
        {
            if(isHitWall==true)
            {
                print("hit wall!");
                isHitWall = false;

                //벽에 부딫힘
                randomX = Random.Range(bounds.min.x, bounds.max.x);
                randomY = Random.Range(bounds.min.y, bounds.max.y);
                madRushVec= (new Vector3(randomX, randomY, 0) - transform.position).normalized;
                yield return new WaitForSeconds(0.1f);
            }

            rigid.AddForce(madRushVec * GetComponent<EnemyStats>().defaultMoveSpeed * 400);
            yield return new WaitForSeconds(0.05f);
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;


        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }



    IEnumerator Hiding() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Hiding");
        enemyAnim.changeAnimToBaby();

        SafeArea.SetActive(true);
        DamageArea.SetActive(true);

        yield return StartCoroutine(DamageAreaIncrease());
        yield return new WaitForSeconds(1f);

        SafeArea.SetActive(false);
        DamageArea.SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator DamageAreaIncrease() 
    {
        float scaleFactor = 1.1f;

        // initiate scale
        DamageArea.transform.localScale = Vector3.one;


        Renderer renderer1 = DamageArea.GetComponent<Renderer>();
        Renderer renderer2 = floor.GetComponent<Renderer>();


        // 완전히 가릴 때까지 스케일 조정
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (DamageArea.transform.localScale.x<100)
        {
            DamageArea.transform.localScale *= scaleFactor;
            yield return new WaitForSeconds(0.1f);
        }


    }


    IEnumerator Screaming() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Screaming");
        enemyAnim.changeAnimToBaby();

        ScreamArea.SetActive(true);
        //this.GetComponent<SpriteRenderer>().color = Color.white;
        //플레이어 느려지게 만든다.

        yield return new WaitForSeconds(1f);
        ScreamArea.SetActive(false);


        yield return new WaitForSeconds(3f);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }




    private Vector3 FindCorner()
    {
        float floorPosX=floor.transform.position.x;
        float floorPosY=floor.transform.position.y;
        float floorSizeX=floor.transform.localScale.x;
        float floorSizeY=floor.transform.localScale.y;
        float monsterSize = 10;

        float x=0;
        float y=0;

        int randNum=Random.Range(0, 4);

        

        switch(randNum)
        {
            case 0://left up
                x = floorPosX - floorSizeX / 2+monsterSize;
                y = floorPosY + floorSizeY / 2-monsterSize;
                break;
            case 1://right up
                x = floorPosX + floorSizeX / 2 - monsterSize;
                y = floorPosY + floorSizeY / 2 - monsterSize;
                break;
            case 2://left down
                x = floorPosX - floorSizeX / 2 + monsterSize;
                y = floorPosY - floorSizeY / 2 - monsterSize;
                break;
            case 3://right down
                x = floorPosX + floorSizeX / 2 - monsterSize;
                y = floorPosY - floorSizeY / 2 + monsterSize;
                break;
            default: 
                break;
        }

        return new Vector3(x,y,0);
    }



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "Wall")
        {
            isHitWall = true;
        }
        
    }
}
