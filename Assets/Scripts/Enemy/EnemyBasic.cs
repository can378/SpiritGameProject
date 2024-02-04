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
        enemyTarget = GameObject.FindGameObjectWithTag("Player").transform;
        //enemyTarget=GameObject.Find("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        status = GetComponent<EnemyStatus>();
    }


    public void Chase()
    {
        Vector2 direction = enemyTarget.transform.position - transform.position;
        transform.Translate(direction * status.speed * Time.deltaTime);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerEnter2D (Collider2D collision) 
{
        if (collision.tag == "PlayerAttack")
        {
            print("enemy damaged");
            HitDetection hitDetection = collision.GetComponent<HitDetection>();

            int criticalHit = Random.Range(0, 100) < hitDetection.critical ? 1 : 0;

            status.health -= hitDetection.damage + criticalHit * hitDetection.criticalDamage * hitDetection.damage;
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            rigid.AddForce(dir * hitDetection.knockBack, ForceMode2D.Impulse);
            Invoke("OffDamaged", 0.2f);

            if (status.health <= 0f)
            {
                DataManager.instance.userData.playerExp++;
                MapUIManager.instance.UpdateExpUI();
                EnemyDead();

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

        //enemy »ç¶óÁü
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);

    }

}
