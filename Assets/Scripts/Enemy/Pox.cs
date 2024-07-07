using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pox : EnemyBasic
{
    [SerializeField] GameObject hitArea;
    //수두 걸린 사람
    //거리가 어느정도 있으면 총알 던짐
    //가까이 있다면 때리고 튄다.

    private void Start()
    {
        base.Start();
        hitDetection = hitArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
    }
    protected override void MovePattern()
    {

    }

    protected override void AttackPattern()
    {
        // 근거리 공격
        if(targetDis <= 8f)
        {
            StartCoroutine(HitAndRun());
        }
        // 원거리 공격
        else if( targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(Throw());
        }
    }

    IEnumerator HitAndRun()
    {
        Vector3 hitDir = targetDirVec;

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 5, 0, 0, null);
        hitArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        hitArea.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitArea.SetActive(false);
        
        isAttack = false;
        isAttackReady = true;

        isRun = true;
        yield return new WaitForSeconds(3f);
        isRun = false;
    }

    IEnumerator Throw()
    {
        //throwing stone
        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 7, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        isAttackReady = true;

    }


    /*
    IEnumerator pox()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = enemyTarget.position - transform.position;


        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            print("throwing stone");

            //throwing stone
            yield return new WaitForSeconds(1f);
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 7, ForceMode2D.Impulse);


            yield return new WaitForSeconds(1f);
            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1f);
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 5);
            

            rigid.velocity = new Vector3(0,0,0);
            yield return new WaitForSeconds(1f);

            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 2);
            
        }
        else
        {
            //hit and run
            print("hit and run");
            //getting closer
            do
            {
                Chase();
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                yield return new WaitForSeconds(0.01f);
            } while (targetDis > 1.2f);


            //getting farther
            do
            {
                rigid.AddForce(-targetDirVec * stats.defaultMoveSpeed, ForceMode2D.Impulse);
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
                yield return new WaitForSeconds(0.01f);
            } while (targetDis < 15f);

            yield return new WaitForSeconds(0.01f);
            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);

        }
        StartCoroutine(pox());

    }
    */
}
