using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dokkebie : EnemyBasic
{
    private int index = 0;
    void Start()
    {
        StartCoroutine(dokkebie());
    }
    private void OnEnable()
    {
        StartCoroutine(dokkebie());
    }

    private void OnDisable()
    {
        StopCoroutine(dokkebie());
    }

    IEnumerator dokkebie()
    {
        if (index == 0)
        { //shot dokkebie fire
            GameObject bullet = ObjectPoolManager.instance.Get(3);
            bullet.transform.position = transform.position;


            Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

            targetDirVec = (enemyTarget.position - transform.position).normalized;
            rigidBullet.AddForce(targetDirVec.normalized * 3, ForceMode2D.Impulse);
            yield return new WaitForSeconds(4f);
            
        }
        else 
        {
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 10);
            yield return new WaitForSeconds(0.1f);
            
        }
        index++;
        if (index == 100) { index = 0; }

        

        StartCoroutine(dokkebie());

    }
}
