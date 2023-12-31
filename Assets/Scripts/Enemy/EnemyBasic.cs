using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{

    public Transform enemyTarget;
    public EnemyStatus status;
    private Rigidbody2D rigid;

    private void Awake()
    {
        enemyTarget=GameObject.Find("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        status = GetComponent<EnemyStatus>();
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" || collision.tag == "PlayerBullet")
        {
            //print(this.name + " attaked");
            if (status.health <= 0f) { EnemyDead(); }
            else 
            {
                print("enemy damaged");
                status.health--;
                Vector2 dir = (transform.position - collision.transform.position).normalized;
                rigid.AddForce(dir * 50f, ForceMode2D.Impulse);
                Invoke("OffDamaged", 0.2f);
            }
        }
    }
    void OffDamaged()
    {
        //this.layerMask = 0;
        //isInvincible = false;
    }

    public void EnemyDead()
    {
        int dropCoinNum = 10;
        //drop coin
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy �����
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);

    }

}
