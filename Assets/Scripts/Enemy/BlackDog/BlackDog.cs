using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackDog : EnemyBasic
{
    BlackDogStatus blackDogStatus;
    [SerializeField] LayerMask detectLayer;         //공격 탐지 레이어
    [SerializeField] int attackDetectRange;         //공격 감지 범위

    Vector3 mousePos;
    Vector3 playerForward;
    float angle;
    Vector2 perpendicularDir;

    protected override void Awake()
    {
        base.Awake();
        status = blackDogStatus = blackDogStatus = GetComponent<BlackDogStatus>();
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
            blackDogStatus.isDetectAttack = false;
            return;
        }

        blackDogStatus.isDetectAttack = true;
    }

    void Dodge()
    {
        if(!blackDogStatus.isDetectAttack)
        {
            return;
        }

        if(!blackDogStatus.isDodge && !blackDogStatus.isAttack && (0 >= blackDogStatus.isFlinch))
        {
            StartCoroutine(DodgeCoroutine());
        }
    }

    IEnumerator DodgeCoroutine()
    {
        blackDogStatus.isDodge = true;
        blackDogStatus.isAttackReady = false;
        //적 방향 수직으로 회피
        if(Random.Range(0,2) == 0)
        {
            blackDogStatus.moveVec = new Vector2(blackDogStatus.targetDirVec.y, -blackDogStatus.targetDirVec.x).normalized;
        }
        else
        {
            blackDogStatus.moveVec = new Vector2(-blackDogStatus.targetDirVec.y, blackDogStatus.targetDirVec.x).normalized;
        }
        enemyStats.MoveSpeed.IncreasedValue += 2f;

        yield return new WaitForSeconds(0.3f);
        blackDogStatus.moveVec = Vector2.zero;
        enemyStats.MoveSpeed.IncreasedValue -= 2f;

        yield return new WaitForSeconds(0.3f);
        blackDogStatus.isDodge = false;
        blackDogStatus.isAttackReady = true;

    }

    #endregion Dodge

    protected override void MovePattern()
    {
        if(!blackDogStatus.isTarget)
        {
            RandomMove();
        }
        else if(blackDogStatus.isDodge)
        {
            
        }
        else if (blackDogStatus.targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (blackDogStatus.targetDis <= enemyStats.maxAttackRange)
        {
            blackDogStatus.attackCoroutine = StartCoroutine(HitAndRun());
        }
        // 이거는 어떤 패턴? 작동을 안하는 거 같아서 우선 주석 처리
        // else
        // {
        //     StartCoroutine(Evasion());
        // }
    }

    IEnumerator HitAndRun()
    {
        blackDogStatus.isAttack = true;
        blackDogStatus.isAttackReady = false;
        //yield return new WaitForSeconds(biteTime * 0.4f);

        if (hitEffects.Length > 0 && hitEffects[0] != null)
        {
            hitEffects[0].GetComponent<HitDetection>().SetHit_Ratio(5, 0.5f, enemyStats.AttackPower);

            hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(blackDogStatus.targetDirVec.y, blackDogStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
            hitEffects[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            hitEffects[0].gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("hitEffects[0] is missing in BlackDog!");
        }

        blackDogStatus.isAttack = false;
        blackDogStatus.isAttackReady = true;

        if(enemyStatus.EnemyTarget) RunAway(enemyStatus.EnemyTarget.transform, 2.0f);

    }


    IEnumerator Evasion() 
    {
        print("blackdog evasion start");
        blackDogStatus.isAttack = true;
        blackDogStatus.isAttackReady = false;


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerForward = (blackDogStatus.EnemyTarget.CenterPivot.position - mousePos);
        angle = Vector3.Angle(playerForward, blackDogStatus.targetDirVec);


        if (angle < 70f)
        {
            print("blackdog evasion1!!!");
            //evasion!!
            perpendicularDir = new Vector2(blackDogStatus.targetDirVec.y, -blackDogStatus.targetDirVec.x).normalized;
            rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().MoveSpeed.Value * 200);
        }
        else 
        {
            rigid.AddForce(blackDogStatus.targetDirVec * GetComponent<EnemyStats>().MoveSpeed.Value * 100);
        }

        yield return new WaitForSeconds(0.01f);
        blackDogStatus.isAttack = false;
        blackDogStatus.isAttackReady = true;
    }

    public override void FlinchCancle() 
    {
        base.FlinchCancle();
        if(enemyStatus.EnemyTarget) RunAway(enemyStatus.EnemyTarget.transform, 5.0f);
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

    public override void InitStatus()
    {
        blackDogStatus.isDodge = false;
        blackDogStatus.isDetectAttack = false;
        base.InitStatus();
    }


}
