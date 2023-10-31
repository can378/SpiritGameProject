using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    /*
    public Transform enemyTarget;


    GameObject enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy");

    }

    private void Update()
    {

        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        //target과 가까워지면 이동
        if (targetDistance <= 5 && targetDistance >= 1.2f)
        {
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
        transform.Translate(direction *1* Time.deltaTime);

    }
    void Wander()
    {
        Vector2 direction = transform.position;
        direction.x += Random.Range(-2f, 2f);
        direction.y += Random.Range(-2f, 2f);

        transform.Translate(direction * 0.1f*Time.deltaTime);
    
    }
   */

    //장애물을 피해서 이동하는 기능

    //애니메이터 입히기

    //<특성>
    //외형, 이름, 스폰 위치
    //피, 공격(공격력, 공격속도,공격 범위, 공격 사거리), 이동속도, 특수 특성

    //<상태>
    //살아있음 = 가만히 / 쫓아가기 / 탐색(숨어있을때) / 공격하기 / 도망가기 / 특수행동
    //죽음 = 사라지는 모션 / 아이템 떨구기 / 특수 행동
}
