using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Head : EnemyBasic
{
    public LineCreator lineCreator;
    public GameObject vomit;


    //움직이지는 않고 토만 함

    protected override void AttackPattern()
    {
        StartCoroutine(head());
    }

    IEnumerator head() 
    {
        print("head vomit");
        //targetDirVec = (enemyTarget.position - transform.position).normalized;

        float angle = Mathf.Atan2
            (enemyStatus.EnemyTarget.transform.position.y - transform.position.y,
             enemyStatus.EnemyTarget.transform.position.x - transform.position.x)
           * Mathf.Rad2Deg;
        
        vomit.transform.rotation = Quaternion.Euler(0, 0, angle-180);


        // Quaternion rotation = Quaternion.LookRotation(0,0,targetDirVec.z);
        //vomit.transform.rotation = rotation;

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        vomit.SetActive(true);
        yield return new WaitForSeconds(3f);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        vomit.SetActive(false);
        yield return new WaitForSeconds(3f);

    }
    
}
