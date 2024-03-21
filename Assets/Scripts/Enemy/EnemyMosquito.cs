using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyBasic
{

    private void Start()
    {
        //GetComponent<PathFinding>().seeker = transform;
    }


    private void FixedUpdate()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDis <= stats.detectionDis && targetDis >= 1f)
        {
            //GetComponent<PathFinding>().StartFinding((Vector2)transform.position, (Vector2)enemyTarget.transform.position);
            Chase();
        }

    }


}
