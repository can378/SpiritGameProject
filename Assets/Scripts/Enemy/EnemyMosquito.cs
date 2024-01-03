using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyBasic
{


    void Start()
    {

    }


    void Update()
    {
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= status.detectionDis && targetDistance >= 1f)
        {
            Chase();
        }
        else
        {
            Wander();
        }

    }

}
