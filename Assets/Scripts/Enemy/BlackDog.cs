using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDog : EnemyBasic
{
    [SerializeField] LayerMask detectLayer;
    [SerializeField] int attackDetectRange;       //공격 감지 범위
    [SerializeField] float dodgeTime;
    [SerializeField] bool isDodge;
    [SerializeField] bool isDetectAttack;

    [SerializeField] GameObject biteArea;
    [SerializeField] float biteTime;

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
        if(Random.Range(0,2) == 0)
        {
            moveVec = new Vector2(targetDirVec.y, -targetDirVec.x);
        }
        else
        {
            moveVec = new Vector2(-targetDirVec.y, targetDirVec.x);
        }
        enemyStats.increasedMoveSpeed += 10f;
        yield return new WaitForSeconds(0.3f);

        moveVec = Vector2.zero;
        enemyStats.increasedMoveSpeed -= 10f;
        yield return new WaitForSeconds(0.3f);

        isDodge = false;
        isAttackReady = true;
        yield return new WaitForSeconds(0.4f);

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
        if (targetDis <= 3f)
        {
            StartCoroutine(HitAndRun());
        }
    }

    IEnumerator HitAndRun()
    {
        isAttack = true;
        isAttackReady = false;

        yield return new WaitForSeconds(biteTime * 0.3f);

        HitDetection hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);

        biteArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        biteArea.SetActive(true);
        yield return new WaitForSeconds(biteTime * 0.4f);

        biteArea.SetActive(false);
        isAttack = false;
        yield return new WaitForSeconds(biteTime * 0.3f);

        isRun = true;
        yield return new WaitForSeconds(2f);

        isRun = false;
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
