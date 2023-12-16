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

        if (targetDistance <= detectionDistance && targetDistance >= 0f)
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }


    //�÷��̾�� ������ ������ �ִ´�
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print("�÷��̾�� ����. �䱫�� �������� �ִ´�.");
        }
    }*/


}
