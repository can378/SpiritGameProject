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

            Damaged(hitDetection.damage,hitDetection.critical, hitDetection.criticalDamage);
            KnockBack(collision.gameObject, hitDetection.knockBack);
            //DIE

        }
    }

    public void Damaged(float damage, float critical, float criticalDamage)
    {
        int criticalHit = Random.Range(0, 100) < critical ? 1 : 0;
        damage = (int)(damage + criticalHit * criticalDamage * damage);
        print("enemy damaged : " + damage);
        status.health -= damage;
        sprite.color = Color.red;
        Invoke("DamagedOut",0.05f);
        if (status.health <= 0f)
        {
            DataManager.instance.userData.playerExp++;
            MapUIManager.instance.UpdateExpUI();
            EnemyDead();
        }
    }

    void DamagedOut()
    {
        sprite.color = Color.white;
    }

    public void KnockBack(GameObject agent, float knockBack)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;
        rigid.AddForce(dir * knockBack, ForceMode2D.Impulse);
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
