using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : EnemyBasic
{

    GameObject bullet;
    public float attackRate = 1f;
    private float timeAfterAttack;


    void Start()
    {
        timeAfterAttack = 0f;
        
    }

    void Update()
    {
        timeAfterAttack += Time.deltaTime;
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= detectionDistance)
        {
            if (timeAfterAttack >= attackRate)
            {
                timeAfterAttack = 0f;
                bullet = ObjectPoolManager.instance.Get(1);
                bullet.transform.position = this.transform.position;
            }
        }

    }

}
