using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pox : EnemyBasic
{
    [SerializeField] GameObject HitArea;
    //수두 걸린 사람
    //거리가 어느정도 있으면 총알 던짐
    //가까이 있다면 때리고 튄다.

    protected override void MovePattern()
    {

    }

    protected override void AttackPattern()
    {
        // 근거리 공격
        if(targetDis <= 7f)
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
        isAttack = true;
        isAttackReady = false;
        Vector3 hitPos = enemyTarget.transform.position;
        yield return new WaitForSeconds(0.5f);

        HitDetection hitDetection = HitArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;

        HitArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(hitPos.y - transform.position.y, hitPos.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        HitArea.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        HitArea.SetActive(false);
        isAttack = false;
        isAttackReady = true;
        isRun = true;
        yield return new WaitForSeconds(2f);

        isRun = false;

    }

    IEnumerator Throw()
    {
        //throwing stone
        isAttack = true;
        isAttackReady = false;
        Vector3 throwPos = enemyTarget.transform.position;
        yield return new WaitForSeconds(1f);

        GameObject bullet = Instantiate(ObjectPoolManager.instance.Get2("Bullet"),transform.position,Quaternion.identity);
        targetDirVec = (throwPos - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 7, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);

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
