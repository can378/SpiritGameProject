using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDog : EnemyBasic
{
    [SerializeField] LayerMask detectLayer;         //공격 탐지 레이어
    [SerializeField] int attackDetectRange;         //공격 감지 범위
    [SerializeField] bool isDodge;                  // 회피중
    [SerializeField] bool isDetectAttack;           // 공격 탐지

    [SerializeField] GameObject biteArea;
    [SerializeField] float biteTime;



    private Vector3 mousePos;
    private Vector3 playerForward;
    private float angle;
    private Vector2 perpendicularDir;


    private void Start()
    {
        base.Start();
        hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;

    }

    protected override void Update()
    {
        base.Update();
        DetectAttack();
        Dodge();
    }

    #region Dodge

    void DetectAttack()
    {
        var playerAttackObj = Physics2D.OverlapCircle(transform.position, attackDetectRange, detectLayer);

        if(playerAttackObj == null)
        {
            isDetectAttack = false;
            return;
        }

        isDetectAttack = true;
    }

    void Dodge()
    {
        if(!isDetectAttack)
        {
            return;
        }

        if(!isDodge && !isAttack && !isFlinch)
        {
            StartCoroutine(DodgeCoroutine());
        }
    }

    IEnumerator DodgeCoroutine()
    {
        isDodge = true;
        isAttackReady = false;
        //적 방향 수직으로 회피
        if(Random.Range(0,2) == 0)
        {
            moveVec = new Vector2(targetDirVec.y, -targetDirVec.x).normalized;
        }
        else
        {
            moveVec = new Vector2(-targetDirVec.y, targetDirVec.x).normalized;
        }
        enemyStats.increasedMoveSpeed += 10f;

        yield return new WaitForSeconds(0.3f);
        moveVec = Vector2.zero;
        enemyStats.increasedMoveSpeed -= 10f;

        yield return new WaitForSeconds(0.7f);
        isDodge = false;
        isAttackReady = true;

    }

    #endregion Dodge

    protected override void MovePattern()
    {
        if(!enemyTarget)
        {
            RandomMove();
        }
        else if(isDodge)
        {
            
        }
        else if (targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (targetDis <= 2f)
        {
            StartCoroutine(HitAndRun());
        }
        else
        {
            StartCoroutine(Evasion());
        }
    }

    IEnumerator HitAndRun()
    {
        isAttack = true;
        isAttackReady = false;
        //yield return new WaitForSeconds(biteTime * 0.4f);


        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        
        biteArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x) * Mathf.Rad2Deg - 90);
        biteArea.SetActive(true);
        yield return new WaitForSeconds(biteTime * 0.6f);
        biteArea.SetActive(false);
        

        isAttack = false;
        isAttackReady = true;
        
        isRun = true;
        yield return new WaitForSeconds(5f);
        isRun = false;
    }


    IEnumerator Evasion() 
    {
        print("blackdog evasion start");
        isAttack = true;
        isAttackReady = false;


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerForward = (enemyTarget.position - mousePos);
        angle = Vector3.Angle(playerForward, targetDirVec);


        if (angle < 70f)
        {
            print("blackdog evasion1!!!");
            //evasion!!
            perpendicularDir = new Vector2(targetDirVec.y, -targetDirVec.x).normalized;
            rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 200);
        }
        else 
        {
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 100);
        }

        yield return new WaitForSeconds(0.01f);
        isAttack = false;
        isAttackReady = true;
    }


    /*
    IEnumerator blackDog() 
    {

        Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDir = (enemyTarget.position - transform.position).normalized;
        Vector3 playerForward = (enemyTarget.position - mousePos);
        float angle = Vector3.Angle(playerForward, targetDir);


        if (Vector2.Distance(enemyTarget.position, transform.position) < 0.5f)
        { isRunaway = true; }



        if (angle < 70f)
        {

            Vector2 perpendicularDir = new Vector2(targetDir.y, -targetDir.x).normalized;
            rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);


        }
        else 
        {
            
            if (isRunaway) 
            {
                targetDirVec= (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
                
                if (Vector2.Distance(enemyTarget.position, transform.position) > 12f)
                { isRunaway = false;}
            }
            else 
            {
                //Chase(); 
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(blackDog());
    
    }
    */



}
