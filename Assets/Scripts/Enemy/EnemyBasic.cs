using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{

    public Transform enemyTarget;
    public EnemyStatus status;


    private void Awake()
    {
        enemyTarget=GameObject.Find("Player").transform;
        status = GetComponent<EnemyStatus>();
        //this.transform.position = status.spawnPos;
    }


    public void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * status.speed * Time.deltaTime);

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
            status.health--;
            print(this.name+" attaked");
            if (status.health <= 0f) { EnemyDead(); }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void EnemyDead()
    {
        int dropCoinNum = 10;
        //drop coin
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy »ç¶óÁü
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);

    }

}
