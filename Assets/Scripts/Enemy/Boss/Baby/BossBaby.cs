using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaby : Boss
{
    /// <summary>
    /// ������ ���� ����Ʈ �ڷ���, hitEffect�� ������ �� �� �� ������� ������ ��
    /// </summary>
    enum BossBabyHitEffect {SafeArea, DamageArea, ScreamArea, RushHitArea, Poision_Trail, Tear, None };

    [ReadOnly]
    new public AnimJukqwi jukqwiAnim;

    private GameObject floor;
    private int patternIndex;
    Bounds bounds;
    Vector2 madRushVec;
    Vector3 corner;


    [SerializeField] Transform GrapPos;

    [SerializeField] GameObject CrackPrefab;

    bool HitWall;

    protected override void Awake()
    {
        base.Awake();

        jukqwiAnim = animGameObject.GetComponent<AnimJukqwi>();
    }

    protected override void Start()
    {
        base.Start();
        floor = GameManager.instance.nowRoom;

        enemyStatus.isAttackReady = true;
        bounds = floor.GetComponent<Collider2D>().bounds;


        // ���� ���� �ʱ�ȭ
        hitEffects[(int)BossBabyHitEffect.RushHitArea].GetComponent<HitDetection>().SetHit_Ratio(0, 1, stats.AttackPower, 100);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].GetComponent<HitDetection>().SetHit_Ratio(0, 1, stats.SkillPower, 10);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].GetComponent<HitDetection>().SetMultiHit(true, 4);
        //hitEffects[(int)BossBabyHitEffect.Tear].GetComponent<HitDetection>().SetHit_Ratio(0, 2, stats.SkillPower);
    }

    protected override void MovePattern()
    {
        if(madRushVec != Vector2.zero)
        {

        }
    }

    protected override void Update()
    {
        base.Update();
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
        jukqwiAnim.ChangeVersion(AnimJukqwi.Version.Adult);

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
                grapDeBuff.SetGrapOwner(GetComponent<ObjectBasic>(), GrapPos);
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
            jukqwiAnim.animator.SetBool("isGrap", true);
            // ����� ��� ��� ���¶��
            while (grapDeBuff != null)
            {
                time += Time.deltaTime;
                // 3�ʵ��� ��� ���¶��
                if (4.0f < time)
                {
                    // ū ���ؿ� ��⸦ 1�� �Ŀ� ����
                    jukqwiAnim.animator.SetTrigger("Hug");
                    grapDeBuff.target.GetComponent<ObjectBasic>().BeAttacked(stats.AttackPower.Value, grapDeBuff.target.transform.position);
                    break;
                }

                yield return null;
            }
        }
        jukqwiAnim.animator.SetBool("isGrap", false);

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
        enemyStatus.moveSpeedMultiplier = 4.0f;
        print("MadRush");
        jukqwiAnim.ChangeVersion(AnimJukqwi.Version.Adult);

        madRushVec = (new Vector2(Random.Range(-10, 10), Random.Range(-10, 10))).normalized;

        float time = 0;
        Vector2 VecSize = new Vector2(0, 0);
        HitWall = false;
        yield return new WaitForSeconds(0.1f);

        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
        hitEffects[(int)BossBabyHitEffect.Poision_Trail].SetActive(true);
        jukqwiAnim.animator.SetBool("isRush",true);

        while (true)
        {
            //VecSize = madRushVec * Mathf.Abs(Vector2.Dot(BabyBounds.extents, new Vector2(madRushVec.x, madRushVec.y)));
            Debug.DrawRay(transform.position, madRushVec.normalized, Color.green);
            //ray = Physics2D.Raycast(transform.position, madRushVec.normalized, 9.0f, LayerMask.GetMask("EnemyWall") | LayerMask.GetMask("Wall"));
            if (HitWall)
            {
                Instantiate(CrackPrefab, CenterPivot.position, Quaternion.identity);
                hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
                hitEffects[(int)BossBabyHitEffect.Poision_Trail].SetActive(false);
                enemyStatus.moveVec = Vector2.zero;
                yield return new WaitForSeconds(0.5f);

                if(time > 10.0f)
                    break;

                // ���� ���� ����� ���� ���� ������ ������ 120 �� ���϶�� �ڷ� ����
                HitWall = false;
                madRushVec = NextRushVec(madRushVec, enemyStatus.targetDirVec).normalized;
                hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(true);
                hitEffects[(int)BossBabyHitEffect.Poision_Trail].SetActive(true);
                continue;
            }
            
            enemyStatus.moveVec = madRushVec;
            yield return null;

            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        enemyStatus.moveVec = Vector2.zero;
        enemyStatus.moveSpeedMultiplier = 1.0f;

        hitEffects[(int)BossBabyHitEffect.Poision_Trail].SetActive(false);
        hitEffects[(int)BossBabyHitEffect.RushHitArea].SetActive(false);
        jukqwiAnim.animator.SetBool("isRush", false);
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator Screaming()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Screaming");
        jukqwiAnim.ChangeVersion(AnimJukqwi.Version.Adult);

        jukqwiAnim.animator.SetBool("isScream", true);
        hitEffects[(int)BossBabyHitEffect.ScreamArea].SetActive(true);

        //�÷��̾� �������� �����.
        yield return new WaitForSeconds(5f);

        jukqwiAnim.animator.SetBool("isScream", false);
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


    #endregion �


    #region ����

    IEnumerator Crying() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Crying");
        jukqwiAnim.ChangeVersion(AnimJukqwi.Version.Baby);
        jukqwiAnim.animator.SetBool("isCry",true);

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
                Vector2 RoomSize = bounds.extents * 0.2f;
                DropPos = new (Random.Range(bounds.min.x + RoomSize.x, bounds.max.x - RoomSize.x), Random.Range(bounds.min.y + RoomSize.y, bounds.max.y - RoomSize.y));
                GameObject ThisTear = ObjectPoolManager.instance.Get(hitEffects[(int)BossBabyHitEffect.Tear], DropPos);
                HitDetection hitDetection = ThisTear.GetComponent<HitDetection>();
                hitDetection.user = this;
                hitDetection.SetDisableTime(0.5f, ENABLE_TYPE.Time);
                hitDetection.SetHit_Ratio(10, 1.0f, enemyStats.SkillPower, 10);

            }
            if(PlayerPos == 0)
            {
                DropPos = enemyStatus.enemyTarget.position;
                GameObject ThisTear = ObjectPoolManager.instance.Get(hitEffects[(int)BossBabyHitEffect.Tear], DropPos);
                HitDetection hitDetection = ThisTear.GetComponent<HitDetection>();
                hitDetection.user = this;
                hitDetection.SetDisableTime(0.5f, ENABLE_TYPE.Time);
                hitDetection.SetHit_Ratio(10, 1.0f, enemyStats.SkillPower, 10);
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        jukqwiAnim.animator.SetBool("isCry", false);

        yield return new WaitForSeconds(3f);
        
        enemyStatus.isAttackReady = true;
    }

    /*
    IEnumerator DropTear(Vector2 _Pos) 
    {
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
    */
    IEnumerator Hiding() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        print("Hiding");

        // �������� �̵��Ѵ�.
        enemyStatus.moveVec = (FindCorner() - transform.position).normalized;
        while (true)
        {
            // ���� ������ �������� �����.
            Debug.DrawRay(transform.position, madRushVec.normalized * 5.0f, Color.green);
            if (Physics2D.Raycast(transform.position, enemyStatus.moveVec, 5.0f, LayerMask.GetMask("EnemyWall") | LayerMask.GetMask("Wall")))
            {
               break;
            }
            yield return null;
        }
        enemyStatus.moveVec = Vector3.zero;

        yield return new WaitForSeconds(1f);
        jukqwiAnim.ChangeVersion(AnimJukqwi.Version.Baby);
        jukqwiAnim.animator.SetBool("isHide", true);

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(true);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(true);
        
        yield return StartCoroutine(SafeAreaDecrease());
        yield return new WaitForSeconds(1f);

        hitEffects[(int)BossBabyHitEffect.SafeArea].SetActive(false);
        hitEffects[(int)BossBabyHitEffect.DamageArea].SetActive(false);

        enemyStatus.isAttack = false;
        jukqwiAnim.animator.SetBool("isHide", false);
        yield return new WaitForSeconds(2f);
        enemyStatus.isAttackReady = true;

    }

    IEnumerator SafeAreaDecrease()
    {
        float time = 0.0f;

        // initiate scale
        hitEffects[(int)BossBabyHitEffect.SafeArea].transform.localScale = new Vector3(80, 80, 1);

        // ������ ���� ������ ������ ����
        //while (renderer1.bounds.Intersects(renderer2.bounds))
        while (time < 8)
        {

            Vector3 Scale = hitEffects[(int)BossBabyHitEffect.SafeArea].transform.localScale;
            Scale.x -= Time.deltaTime * 10;
            Scale.y -= Time.deltaTime * 10;
            hitEffects[(int)BossBabyHitEffect.SafeArea].transform.localScale = Scale;
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

    void Disarm()
    {
        enemyStatus.enemyTarget.GetComponent<ObjectBasic>().ApplyBuff(10);
    }
    void RemoveDisarm()
    {
        enemyStatus.enemyTarget.GetComponent<ObjectBasic>().RemoveBuff(10);
    }


    #endregion ����

    Vector3 FindCorner()
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

    public override void Dead()
    {
        RemoveDisarm();
        base.Dead();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹ü �浹 �� ���� ����
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "EnemyWall")
        {
            HitWall = true;
        }
    }

}
