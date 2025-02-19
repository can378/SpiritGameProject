using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tiger_tiger : EnemyBasic
{
    public GameObject bloodDebuff;
    public GameObject trans;
    public bool isTransform=false;


    void OnEnable() 
    { //StartNamedCoroutine("tiger", tiger()); 
    }



    IEnumerator tiger() 
    { 
        yield return null;
        
        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;

        //hunt
        yield return new WaitForSeconds(3f);
        rigid.AddForce(enemyStatus.targetDirVec * GetComponent<EnemyStats>().MoveSpeed.Value * 100);
        yield return new WaitForSeconds(2f);
        rigid.velocity = new Vector2(0, 0);


        //hit
        enemyStatus.targetDis = Vector2.Distance(transform.position, enemyStatus.enemyTarget.position);
        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
        while (enemyStatus.targetDis > 2f)
        {
            enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
            rigid.AddForce(enemyStatus.targetDirVec * GetComponent<EnemyStats>().MoveSpeed.Value*50);
            enemyStatus.targetDis = Vector2.Distance(transform.position, enemyStatus.enemyTarget.position);
            yield return new WaitForSeconds(0.1f);
        }
        

        if (isTransform) 
        {
            //scream
            print("scream");
        }
        else 
        { }

        StartCoroutine(tiger());
 
    }

    
/*
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("tiger attack player");
            //blood debuff
            //enemyTarget.GetComponent<Player>().ApplyBuff(bloodDebuff);
        }
        if (
            collision.tag == "Enemy" && 
            isTransform == false && 
            collision.GetComponent<EnemyStats>().enemyName=="headTiger"&&
            collision.GetComponent<Tiger_head>().isDetectPlayer==true
            )
        {
            print("tiger meet head");
            //transform
            isTransform = true;
            trans.SetActive(true);

            collision.gameObject.SetActive(false);
            //stronger
            //???????????????????
            GetComponent<EnemyStats>().MoveSpeed.AddValue += 5;
        }
        if (collision.tag == "PlayerAttack")
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>());
        }
    }
    */
}
