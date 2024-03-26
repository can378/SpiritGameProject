using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tiger_tiger : EnemyBasic
{
    public GameObject bloodDebuff;
    public GameObject trans;
    public bool isTransform=false;

    void Start()
    {    StartCoroutine(tiger());    }
    void OnEnable() { StartCoroutine(tiger()); }
    void OnDisable() { StopCoroutine(tiger()); }



    IEnumerator tiger() 
    { 
        yield return null;
        
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

        //hunt
        yield return new WaitForSeconds(3f);
        rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 100);
        yield return new WaitForSeconds(2f);
        rigid.velocity = new Vector2(0, 0);


        //hit
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        while (targetDis > 2f)
        {
            targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
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

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //blood debuff
            //enemyTarget.GetComponent<Player>().ApplyBuff(bloodDebuff);
        }
        if (collision.tag == "TigerHead" && isTransform == false)
        {
            //transform
            isTransform = true;
            trans.SetActive(true);

            //stronger
            //???????????????????
            GetComponent<EnemyStats>().defaultMoveSpeed += 5;
        }   
    }
}
