using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaby : Boss
{
    /// <summary>
    /// 저퀴의 공격 이펙트 자료형, hitEffect에 저장할 때 꼭 이 순서대로 저장할 것
    /// </summary>
    enum BossBabyHitEffect { Tear, SafeArea, DamageArea, ScreamArea, RushHitArea,None };

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

    protected override void MovePattern()
    {
        //Chase();
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
            case 0: yield return enemyStatus.attackCoroutine = StartCoroutine(Rush()); break;
            case 1: yield return enemyStatus.attackCoroutine = StartCoroutine(Hiding()); break;
            case 2: yield return enemyStatus.attackCoroutine = StartCoroutine(Screaming()); break;
            case 3: yield return enemyStatus.attackCoroutine = StartCoroutine(MadRush()); break;
            case 4: yield return enemyStatus.attackCoroutine = StartCoroutine(Crying());break;
        }
        patternIndex++;
        
    }


    IEnumerator Rush() 
    {
        //start
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Rush");
        //enemyAnim.changeAnimToAdult();

        float time = 0;


        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);



        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        
        while (time<3.0f)
        {
            hitEffects[(int)BossBabyHitEffect.RushHitArea].transform.position = transform.position;
            enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
            enemyStatus.moveVec = enemyStatus.targetDirVec;
            yield return new WaitForSeconds(0.01f);
            time += Time.deltaTime;
        }

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
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
        //enemyAnim.changeAnimToBaby();

        //move to corner
        corner = FindCorner();

        while(Vector2.Distance(transform.position, corner) > 60f) 
        {
            yield return new WaitForSeconds(0.1f);
            enemyStatus.moveVec = (corner - transform.position).normalized;
        }
        rigid.velocity = Vector2.zero;



        //start crying
        for (int i = 0; i < 20; i++)
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

        GameObject thisTear = Instantiate(hitEffects[(int)BossBabyHitEffect.Tear]);
        thisTear.SetActive(true);
        thisTear.transform.position=new Vector2(randomX, randomY);

        thisTear.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        
        thisTear.GetComponent<HitDetection>().enabled = true;
        thisTear.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        Destroy(thisTear);

    }


    IEnumerator MadRush() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("MadRush");
        //enemyAnim.changeAnimToAdult();

        randomX = Random.Range(bounds.min.x, bounds.max.x);
        randomY = Random.Range(bounds.min.y, bounds.max.y);
        madRushVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;


        float time = 0;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        while (time < 2.0f)
        {
            hitEffects[(int)BossBabyHitEffect.RushHitArea].transform.position = transform.position;
            if (isHitWall==true)
            {
                print("hit wall!");
                isHitWall = false;

                //벽에 부딫힘
                randomX = Random.Range(bounds.min.x, bounds.max.x);
                randomY = Random.Range(bounds.min.y, bounds.max.y);
                madRushVec= (new Vector3(randomX, randomY, 0) - transform.position).normalized;
                yield return new WaitForSeconds(0.1f);
            }

            rigid.AddForce(madRushVec * GetComponent<EnemyStats>().defaultMoveSpeed * 600);
            yield return new WaitForSeconds(0.05f);
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }



    IEnumerator Hiding() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Hiding");
        //enemyAnim.changeAnimToBaby();

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(true);
        yield return new WaitForSeconds(2f);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(true);

        yield return StartCoroutine(DamageAreaIncrease());
        yield return new WaitForSeconds(1f);

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(false);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator DamageAreaIncrease() 
    {
        float scaleFactor = 1.1f;

        // initiate scale
        hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale = new Vector3(50,50,1);


        Renderer renderer1 = hitEffects[(int)BossBabyHitEffect.DamageArea].GetComponent<Renderer>();
        Renderer renderer2 = floor.GetComponent<Renderer>();


        // 완전히 가릴 때까지 스케일 조정
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale.x<500)
        {
            hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale *= scaleFactor;
            yield return new WaitForSeconds(0.1f);
        }


    }


    IEnumerator Screaming() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Screaming");
        //enemyAnim.changeAnimToBaby();

        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(true);

        //플레이어 느려지게 만든다.

        yield return new WaitForSeconds(1f);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(false);


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
