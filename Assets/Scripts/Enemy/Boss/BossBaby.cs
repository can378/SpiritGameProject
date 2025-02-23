using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaby : Boss
{
    /// <summary>
    /// 저퀴의 공격 이펙트 자료형, hitEffect에 저장할 때 꼭 이 순서대로 저장할 것
    /// </summary>
    enum BossBabyHitEffect { Tear, SafeArea, DamageArea, ScreamArea, RushHitArea, None };

    new public AnimJukqwi enemyAnim;

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


        // 공격 판정 초기화

        hitEffects[(int)BossBabyHitEffect.RushHitArea].GetComponent<HitDetection>().SetHit_Ratio(0, 1, stats.AttackPower, 100);

        hitEffects[(int)BossBabyHitEffect.ScreamArea].GetComponent<HitDetection>().SetHit_Ratio(0, 1, stats.SkillPower, 50);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].GetComponent<HitDetection>().SetMultiHit(true, 4);

        hitEffects[(int)BossBabyHitEffect.Tear].GetComponent<HitDetection>().SetHit_Ratio(0, 2, stats.SkillPower);
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
        int Pattern = patternIndex++;
        switch (Pattern % 5)
        { 
            case 0:
                RemoveDisarm();
                yield return enemyStatus.attackCoroutine = StartCoroutine(Screaming()); 
                break;
            case 1: yield return enemyStatus.attackCoroutine = StartCoroutine(Grap()); break;
            case 2: yield return enemyStatus.attackCoroutine = StartCoroutine(MadRush()); break;
            case 3: 
                Disarm();
                yield return enemyStatus.attackCoroutine = StartCoroutine(Crying());
                break;
            case 4: yield return enemyStatus.attackCoroutine = StartCoroutine(Hiding());break;
        }
        
    }

    #region 어른

    IEnumerator Grap() 
    {
        bool grapSucces = false;
        GrapDeBuff grapDeBuff = null;
        float time = 0;

        //start
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Rush");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Adult);

        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        
        while (time<5.0f)
        {
            enemyStatus.moveVec = enemyStatus.targetDirVec * 5.0f;
            time += Time.deltaTime;

            // 잡기 성공 
            if(status.hitTarget)
            {
                // 대상에게 잡기 디버프 부여
                grapDeBuff = (GrapDeBuff)status.hitTarget.GetComponent<ObjectBasic>().ApplyBuff(9);
                grapDeBuff.SetGrapOwner(GetComponent<ObjectBasic>(),transform);
                grapSucces = true;
                break;
            }

            yield return null;
        }

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        enemyStatus.moveVec = Vector2.zero;

        // 잡기 성공 시
        if (grapSucces)
        {
            time = 0;
            enemyAnim.animator.SetBool("isGrap", true);
            // 대상이 계속 잡기 상태라면
            while (grapDeBuff != null)
            {
                time += Time.deltaTime;
                // 3초동안 잡기 상태라면
                if (4.0f < time)
                {
                    // 큰 피해와 잡기를 1초 후에 해제
                    enemyAnim.animator.SetTrigger("Hug");
                    grapDeBuff.target.GetComponent<ObjectBasic>().BeAttacked(stats.AttackPower.Value, grapDeBuff.target.transform.position);
                    break;
                }

                yield return null;
            }
        }
        enemyAnim.animator.SetBool("isGrap", false);

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
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Adult);

        madRushVec = enemyStatus.targetDirVec;

        float time = 0;
        RaycastHit2D ray;
        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        enemyAnim.animator.SetBool("isRush",true);

        while (time < 10.0f)
        {
            Debug.DrawRay(transform.position, madRushVec.normalized * 9.0f, Color.green);
            ray = Physics2D.Raycast(transform.position, madRushVec.normalized, 9.0f, LayerMask.GetMask("EnemyWall") | LayerMask.GetMask("Wall"));
            if (ray)
            {
                hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
                enemyStatus.moveVec = Vector2.zero;
                yield return new WaitForSeconds(1.0f);

                // 이전 돌진 방향과 다음 돌진 방향의 각도가 120 도 이하라면 뒤로 돌진
                madRushVec = NextRushVec(madRushVec, enemyStatus.targetDirVec);
                hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
                continue;
            }
            
            enemyStatus.moveVec = madRushVec * 10.0f;
            yield return null;

            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        enemyStatus.moveVec = Vector2.zero;

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        enemyAnim.animator.SetBool("isRush", false);
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator Screaming()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Screaming");
        enemyAnim.ChangeVersion(AnimJukqwi.Version.Adult);

        enemyAnim.animator.SetBool("isScream", true);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(true);

        //플레이어 느려지게 만든다.
        yield return new WaitForSeconds(5f);

        enemyAnim.animator.SetBool("isScream", false);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(false);

        yield return new WaitForSeconds(2f);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }

    
    public Vector3 NextRushVec(Vector2 _PrevVec, Vector2 _TargetVec)
    {
        Vector2 v = _TargetVec - _PrevVec;

        float degree = Mathf.Atan2(_TargetVec.y, _TargetVec.x) * Mathf.Rad2Deg - Mathf.Atan2(_PrevVec.y, _PrevVec.x) * Mathf.Rad2Deg;

        if (-60.0f < degree && degree < 60.0f)
        {
            return Quaternion.Euler(0, 0, 180.0f) * _PrevVec;
        }

        return _TargetVec;
    }


    #endregion 어른


    #region 응애

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

        yield return new WaitForSeconds(1.5f);

        //start crying
        for (int i = 0; i < 50; i++)
        {
            int DropCount = Random.Range(2, 4);
            int PlayerPos = Random.Range(0, 5);
            Vector2 DropPos;
            for (int j = 0; j < DropCount; ++j)
            {
                DropPos = new (Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
                StartCoroutine(DropTear(DropPos));
            }
            if(PlayerPos == 0)
            {
                DropPos = enemyStatus.enemyTarget.position;
                StartCoroutine(DropTear(DropPos));
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        enemyStatus.isAttack = false;
        
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator DropTear(Vector2 _Pos) 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        GameObject thisTear = Instantiate(hitEffects[(int)BossBabyHitEffect.Tear]);
        thisTear.GetComponent<HitDetection>().SetHit_Ratio(0, 2, stats.SkillPower);
        thisTear.SetActive(true);
        thisTear.transform.position = _Pos;

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

        // 완전히 가릴 때까지 스케일 조정
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


        // 완전히 가릴 때까지 스케일 조정
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale.x<500)
        {
            hitEffects[(int)BossBabyHitEffect.DamageArea].transform.localScale *= scaleFactor;
            yield return new WaitForSeconds(0.1f);
        }


    }

    void Disarm()
    {
        enemyStatus.enemyTarget.GetComponent<ObjectBasic>().ApplyBuff(10);
    }
    void RemoveDisarm()
    {
        enemyStatus.enemyTarget.GetComponent<ObjectBasic>().RemoveBuff(10);
    }


    #endregion 응애


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

    public override void Dead()
    {
        RemoveDisarm();
        base.Dead();
    }
}
