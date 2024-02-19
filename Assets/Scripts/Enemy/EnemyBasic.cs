using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : MonoBehaviour
{
    [HideInInspector]
    public Transform enemyTarget;
    [HideInInspector]
    public EnemyStatus status;
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public Vector2 targetDirVec;
    [HideInInspector]
    public float timeValue=0;

    private void Awake()
    {
        enemyTarget = GameObject.FindWithTag("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        status = GetComponent<EnemyStatus>();
        sprite = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D (Collider2D collision) 
{
        if (collision.tag == "PlayerAttack")
        {
            //Damaged
            HitDetection hitDetection = collision.GetComponent<HitDetection>();

            int criticalHit = Random.Range(0, 100) < hitDetection.critical ? 1 : 0;
            int damage = (int)(hitDetection.damage + criticalHit * hitDetection.criticalDamage * hitDetection.damage);
            print("enemy damaged : " + damage);
            status.health -= damage;
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            rigid.AddForce(dir * hitDetection.knockBack, ForceMode2D.Impulse);

            //DIE
            if (status.health <= 0f)
            {
                DataManager.instance.userData.playerExp++;
                MapUIManager.instance.UpdateExpUI();
                EnemyDead();

            }
        }
    }



    public void EnemyDead()
    {

        //drop coin
        int dropCoinNum = 10;
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy disappear
        this.gameObject.SetActive(false);
    }

    public void Chase()
    {
        Vector2 direction = enemyTarget.position - transform.position;
        transform.Translate(direction * status.speed * Time.deltaTime);

    }

}
