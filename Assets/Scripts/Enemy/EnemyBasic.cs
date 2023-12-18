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

    



    private void Awake()
    {
        enemyTarget=GameObject.Find("Player").transform;
        this.transform.position = spawnPosition;
    }


    public void EnemyDead() 
    {
        Debug.Log(this.gameObject.name + "is dead");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" || collision.tag == "PlayerBullet")
        {
            Debug.Log("enemy attacked");
            health--;
        }
    }
}
