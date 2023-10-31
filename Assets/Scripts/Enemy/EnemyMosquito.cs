using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMosquito : EnemyBasic
{

    void Start()
    {
        print("target="+enemyTarget.gameObject.name);
    }


    void Update()
    {
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        //target과 가까워지면 이동
        if (targetDistance <= 5 && targetDistance >= 1.2f)
        {
            //겹쳐지는 현상 해결하기
            Chase();
        }
        else //그 외에는 혼자 배회
        {
            Wander();
        }
    }

    //target으로 이동
    void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * speed * Time.deltaTime);

    }
    void Wander()
    {
        Vector2 direction = transform.position;
        direction.x += Random.Range(-2f, 2f);
        transform.Translate(direction * 0.1f * Time.deltaTime);

    }


}
