using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : EnemyBasic
{
    void Start()
    {
        StartCoroutine(mouse());
    }

    private void OnEnable()
    {
        StartCoroutine(mouse());
    }

    private void OnDisable()
    {
        StopCoroutine(mouse());
    }

    private bool isChange=false;
    IEnumerator mouse()
    {
        targetDis=Vector2.Distance(transform.position, enemyTarget.position);
        if (isChange == false)
        {
            Chase();
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            //Attack
            if (targetDis > 3f) { Chase(); yield return new WaitForSeconds(0.1f); }
            else
            {
                print("user player skill");
                /*
                if (enemyTarget.GetComponent<PlayerStats>().skill != null)
                {
                    //mimic player skill
                    
                }
                else 
                {
                    //È¥µ·
                    print("chaos");
                }
                */

                yield return new WaitForSeconds(0.1f);
                //run away
                rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 100);

            }

        }
        
        yield return null;
        StartCoroutine(mouse());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isChange == false)
        {

            //Bite
            //print("Bite");

            //Transform
            GetComponent<SpriteRenderer>().sprite = enemyTarget.GetComponent<SpriteRenderer>().sprite;
            //transform.localScale = enemyTarget.transform.localScale;
            isChange = true;

            //Run away
            targetDirVec = enemyTarget.position - transform.position;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 100);


        }

    }

}
