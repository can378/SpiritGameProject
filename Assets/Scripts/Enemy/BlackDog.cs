using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDog : EnemyBasic
{
    private bool isRunaway=false;
    void Start()
    {
        StartCoroutine(blackDog());
    }
    private void OnEnable()
    {
        StartCoroutine(blackDog());
    }

    private void OnDisable()
    {
        StopCoroutine(blackDog());
    }

    IEnumerator blackDog() 
    {

        Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDir = (enemyTarget.position - transform.position).normalized;
        Vector3 playerForward = (enemyTarget.position - mousePos);
        float angle = Vector3.Angle(playerForward, targetDir);

        if (angle < 70f)
        {
            Vector2 perpendicularDir = new Vector2(targetDir.y, -targetDir.x).normalized;
            rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);
            //print("black dog is in player attack arrray"); 

        }
        else 
        {
            if (isRunaway) 
            {
                targetDirVec= (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
                if (Vector2.Distance(enemyTarget.position, transform.position) > 10f)
                { isRunaway = false;yield return new WaitForSeconds(1f); }
            }
            else { Chase(); }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(blackDog());
    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { isRunaway = true; }
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }
    }



}
