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


    //collider로 player감지하는법도 있다.
    //public CircleCollider2D playerDetectionCollider;
    public int detectionDistance;
    //public bool isEnemyAttack=true;


    private void Awake()
    {
        enemyTarget=GameObject.Find("Player").transform;
        this.transform.position = spawnPosition;
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
            health--;
            print(this.name+" attaked");
            if (health <= 0f) { EnemyDead(); }
        }
    }
    public void EnemyDead()
    {
        int dropCoinNum = 10;
        //drop coin
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy 사라짐
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);

    }

    /*
    private void OnTriggerStay2D(CircleCollider2D collision)
    {
        if (collision.tag == "Player") { isEnemyAttack = true; }
    }
    */
}
