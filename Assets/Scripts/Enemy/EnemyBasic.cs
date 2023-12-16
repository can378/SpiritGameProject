using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{

    public Transform enemyTarget;
    public Vector2 spawnPosition;
    public int health;
    public float walkSpeed;
    //public float runSpeed;
    //public int attackSpeed;
    //public int attackPower;
    //public int attackRange;
    public int detectionDistance;

    



    private void Start()
    {
        enemyTarget=GameObject.Find("Player").transform;
        this.transform.position = spawnPosition;
    }

    private void Update()
    {

        //이동 구현? 장애물 피하거나 피하지 않고 이동

        //애니메이터

        //Death
        if (health < 0.1f) { EnemyDead(); }
        else 
        {
            //살아있음 = 가만히 / 쫓아가기 / 탐색(숨어있을때)
            //공격하기 / 도망가기 / 특수행동

        }


    }

    public void EnemyDead() 
    {
        Debug.Log(this.gameObject.name + "는 죽었다.");
        //animation

        //drop item
        //몬스터에 따라 특수 행동?
        //이 게임 오브젝트 파괴?
    
    }



    public void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * walkSpeed * Time.deltaTime);

    }


    public void Wander()
    {
        /*
        Vector2 direction = transform.position;
        direction.x += Random.Range(-2f, 2f);
        transform.Translate(direction * 0.1f * Time.deltaTime);
        */
    }
}
