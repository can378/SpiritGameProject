using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaby : Boss
{
    /// <summary>
    /// ������ ���� ����Ʈ �ڷ���, hitEffect�� ������ �� �� �� ������� ������ ��
    /// </summary>
    enum BossBabyHitEffect { Tear, SafeArea, DamageArea, ScreamArea, RushHitArea,None };

    public new AnimJukqwi enemyAnim;

    private GameObject floor;
    private int patternIndex;
    private bool isHitWall;
    Bounds bounds;
    float randomX, randomY;
    Vector2 madRushVec;
    Vector3 corner;

    protected override void Awake()
    {
        base.Awake();

        enemyAnim = animGameObject.GetComponent<AnimJukqwi>();
    }

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
        //CheckMovement();
    }

    protected override void AttackPattern()
    {
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        switch (patternIndex%5)
        { 
            case 0: yield return enemyStatus.attackCoroutine = StartCoroutine(Screaming()); break;
            case 1: yield return enemyStatus.attackCoroutine = StartCoroutine(MadRush()); break;
            case 2: yield return enemyStatus.attackCoroutine = StartCoroutine(Grap()); break;
            case 3: yield return enemyStatus.attackCoroutine = StartCoroutine(Crying()); break;
            case 4: yield return enemyStatus.attackCoroutine = StartCoroutine(Hiding());break;
        }
        patternIndex++;
        
    }

#region �

    IEnumerator Grap() 
    {
        bool grapSucces = false;
        GrapDeBuff grapDeBuff = null;
        float time = 0;

        //start
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Rush");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Monster);

        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        
        while (time<5.0f)
        {
            enemyStatus.moveVec = enemyStatus.targetDirVec * 5.0f;
            time += Time.deltaTime;

            // ��� ���� 
            if(status.hitTarget)
            {
                // ��󿡰� ��� ����� �ο�
                grapDeBuff = (GrapDeBuff)status.hitTarget.GetComponent<ObjectBasic>().ApplyBuff(9);
                grapDeBuff.SetGrapOwner(GetComponent<ObjectBasic>(),transform);
                grapSucces = true;
                break;
            }

            yield return null;
        }

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        enemyStatus.moveVec = Vector2.zero;

        // ��� ���� ��
        if (grapSucces)
        {
            time = 0;
            
            // ����� ��� ��� ���¶��
            while (grapDeBuff != null)
            {
                time += Time.deltaTime;

                // 3�ʵ��� ��� ���¶��
                if(3.5 < time)
                {
                    // ū ���ؿ� ��⸦ 1�� �Ŀ� ����
                    grapDeBuff.target.GetComponent<ObjectBasic>().Damaged(50);
                    break;
                }

                yield return null;
            }
        }

        yield return new WaitForSeconds(0.1f);

        //end
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator MadRush()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("MadRush");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Monster);

        madRushVec = enemyStatus.targetDirVec;

        float time = 0;
        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        while (time < 10.0f)
        {
            if (isHitWall)
            {
                isHitWall = false;
                madRushVec = enemyStatus.targetDirVec;
                yield return new WaitForSeconds(0.3f);
            }

            enemyStatus.moveVec = madRushVec * 10.0f;

            yield return null;
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        enemyStatus.moveVec = Vector2.zero;

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator Screaming()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Screaming");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Monster);

        enemyAnim.animator.SetBool("isScream", true);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(true);
        //�÷��̾� �������� �����.
        yield return new WaitForSeconds(1f);

        enemyAnim.animator.SetBool("isScream", false);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(false);

        yield return new WaitForSeconds(3f);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }




    #endregion �


    #region ����

    IEnumerator Crying() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Crying");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Baby);
        enemyAnim.animator.SetBool("isCry",true);

        //move to corner
        corner = FindCorner();

        while(Vector2.Distance(transform.position, corner) > 60f) 
        {
            yield return new WaitForSeconds(0.1f);
            enemyStatus.moveVec = (corner - transform.position).normalized;
        }
        rigid.velocity = Vector2.zero;

        //start crying
        for (int i = 0; i < 50; i++)
        {
            int DropCount = Random.Range(2,4);
            int PlayerPos = Random.Range(0, 5);
            for(int j = 0; j < DropCount; ++j)
            {
                StartCoroutine(DropTear());
            }
            if(PlayerPos == 0)
                StartCoroutine(DropTear_PlayerPos());

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
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

        thisTear.GetComponent<SpriteRenderer>().color = Color.blue / 2.0f;
        yield return new WaitForSeconds(0.5f);
        
        thisTear.GetComponent<Collider2D>().enabled = true;
        thisTear.GetComponent<SpriteRenderer>().color = Color.red / 2.0f;
        yield return new WaitForSeconds(0.5f);
        Destroy(thisTear);
    }

    IEnumerator DropTear_PlayerPos()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        GameObject thisTear = Instantiate(hitEffects[(int)BossBabyHitEffect.Tear]);
        thisTear.SetActive(true);
        thisTear.transform.position = enemyStatus.enemyTarget.position;

        thisTear.GetComponent<SpriteRenderer>().color = Color.blue / 2.0f;
        yield return new WaitForSeconds(0.5f);

        thisTear.GetComponent<Collider2D>().enabled = true;
        thisTear.GetComponent<SpriteRenderer>().color = Color.red / 2.0f;
        yield return new WaitForSeconds(0.5f);
        Destroy(thisTear);
    }

    IEnumerator Hiding() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Hiding");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Baby);
        enemyAnim.animator.SetBool("isHide", true);

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(true);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(true);
        
        yield return StartCoroutine(SafeAreaDecrease());
        yield return new WaitForSeconds(1f);

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(false);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(false);

        enemyStatus.isAttack = false;
        enemyAnim.animator.SetBool("isHide", false);
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator SafeAreaDecrease()
    {
        float time = 0.0f;

        // initiate scale
        hitEffects[(int)BossBabyHitEffect.SafeArea].transform.localScale = new Vector3(50, 50, 1);

        // ������ ���� ������ ������ ����
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (time < 3)
        {
            hitEffects[(int)BossBabyHitEffect.SafeArea].transform.localScale = new Vector3(50, 50, 1) / (1 + time);
            time += Time.deltaTime;
            yield return null;
        }


    }

    IEnumerator DamageAreaIncrease() 
    {
        float scaleFactor = 1.1f;

        // initiate scale
        hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale = new Vector3(50,50,1);


        Renderer renderer1 = hitEffects[(int)BossBabyHitEffect.DamageArea].GetComponent<Renderer>();
        Renderer renderer2 = floor.GetComponent<Renderer>();


        // ������ ���� ������ ������ ����
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale.x<500)
        {
            hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale *= scaleFactor;
            yield return new WaitForSeconds(0.1f);
        }


    }

    #endregion ����


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

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "EnemyWall")
        {
            isHitWall = true;
        }

    }
}
