using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyBasic
{

    private void Start()
    {
        GetComponent<PathFinding>().seeker = transform;
    }


    private void FixedUpdate()
    {
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= status.detectionDis && targetDistance >= 1f)
        {
            //GetComponent<PathFinding>().StartFinding((Vector2)transform.position, (Vector2)enemyTarget.transform.position);
            Chase();
        }

    }


}
