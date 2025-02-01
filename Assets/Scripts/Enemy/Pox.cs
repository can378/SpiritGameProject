using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pox : EnemyBasic
{
    public Transform ThrowPos;

    protected override void AttackPattern()
    {
        // �ٰŸ� ����
        if(enemyStatus.targetDis <= 5f)
        {
            enemyStatus.attackCoroutine = StartCoroutine(HitAndRun());
        }
        // ���Ÿ� ����
        else if( enemyStatus.targetDis <= enemyStats.maxAttackRange)
        {
           enemyStatus.attackCoroutine = StartCoroutine(Throw());
        }
    }

    IEnumerator HitAndRun()
    {
        Vector3 hitDir = enemyStatus.targetDirVec;

        enemyAnim.animator.SetTrigger("isHit");
        enemyAnim.ChangeDirection(enemyStatus.targetDirVec);
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.3f);

        enemyAnim.ChangeDirection(hitDir);
        hitEffects[0].GetComponent<HitDetection>().SetHit_Ratio(10, 1, enemyStats.AttackPower);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        hitEffects[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);

        enemyAnim.animator.SetBool("isHit", false);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        RunAway(enemyStatus.enemyTarget.transform, 3.0f);
    }

    IEnumerator Throw()
    {

        // �ִϸ��̼��� Ų��.
        enemyAnim.animator.SetTrigger("isThrow");
        enemyAnim.ChangeDirection(enemyStatus.targetDirVec);

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.75f);
        
        if (enemyStatus.enemyTarget != null)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = ThrowPos.position;
            Vector3 TargetDirVec = (enemyStatus.enemyTarget.position - ThrowPos.position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(TargetDirVec * 7, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(2f);

        enemyAnim.animator.SetBool("isThrow", false);
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
