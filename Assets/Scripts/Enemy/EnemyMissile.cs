using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : EnemyBasic
{

    private GameObject bullet;
    public float attackRate = 1f;
    private float timeAfterAttack;
    private EnemyStatus status;

    void Start()
    {
        timeAfterAttack = 0f;
        status = GetComponent<EnemyStatus>();
    }

    void Update()
    {
        timeAfterAttack += Time.deltaTime;
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= status.detectionDis)
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
