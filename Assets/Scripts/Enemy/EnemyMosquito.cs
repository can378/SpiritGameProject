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

        if (targetDistance <= detectionDistance && targetDistance >= 1f)
        {
            Chase();
        }
        else
        {
            Wander();
        }


        if (health <= 0f) { EnemyDead(); }

    }


    //플레이어와 닿으면 데미지 넣는다
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print("플레이어와 접촉. 요괴가 데미지를 넣는다.");
        }
    }*/


}
