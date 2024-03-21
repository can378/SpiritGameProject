using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pox : EnemyBasic
{
    void Start()
    {
        StartCoroutine(pox());
    }

    private void OnEnable()
    {
        StartCoroutine(pox());
    }

    private void OnDisable()
    {
        StopCoroutine(pox());
    }


    IEnumerator pox()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = enemyTarget.position - transform.position;


        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            print("throwing stone");

            //throwing stone
            yield return new WaitForSeconds(1f);
            GameObject bullet = ObjectPoolManager.instance.Get(0);
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
}
