using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyBasic
{
    /*
    private void OnEnable()
    {
        StartNamedCoroutine("attack", attack());
    }
    
    */


    IEnumerator attack() 
    {
        enemyStatus.targetDis = Vector2.Distance(transform.position, enemyStatus.enemyTarget.position);

        if (enemyStatus.targetDis <= enemyStats.detectionDis && enemyStatus.targetDis >= 1f)
        {
            //GetComponent<PathFinding>().StartFinding((Vector2)transform.position, (Vector2)enemyTarget.transform.position);
            //Chase();
        }
        yield return new WaitForSeconds(0.1f);
    
    }



}
