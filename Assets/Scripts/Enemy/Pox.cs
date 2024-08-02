using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pox : EnemyBasic
{

    protected override void Start()
    {
        base.Start();
    }
    protected override void MovePattern()
    {

    }

    protected override void AttackPattern()
    {
        // 근거리 공격
        if(enemyStatus.targetDis <= 5f)
        {
            enemyStatus.attackCoroutine = StartCoroutine(HitAndRun());
        }
        // 원거리 공격
        else if( enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
           enemyStatus.attackCoroutine = StartCoroutine(Throw());
        }
    }

    IEnumerator HitAndRun()
    {
        Vector3 hitDir = enemyStatus.targetDirVec;

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        
        hitEffects[0].GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 5, 0, 0, null);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitEffects[0].gameObject.SetActive(false);
        
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        StartCoroutine(RunAway(3f));
    }

    IEnumerator Throw()
    {
        //throwing stone
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;
        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(enemyStatus.targetDirVec.normalized * 7, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.5f);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

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
