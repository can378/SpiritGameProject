using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_head : EnemyBasic
{
    
    void Start()
    {
        StartCoroutine(tigerHead(new Vector2(1, 0)));
    }
    private void OnEnable()
    {
        StartCoroutine(tigerHead(new Vector2(1,0)));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private bool isPlayer = false;
    private bool isHit = false;
    IEnumerator tigerHead(Vector2 dir)
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);


        //ROLLING
        float rightOrLeft = Vector2.Dot(rigid.velocity.normalized, Vector2.right);
        if (rightOrLeft > 0)
        {
            //move right
            transform.Rotate(new Vector3(0, 0, -GetComponent<EnemyStats>().defaultMoveSpeed));
        }
        else if (rightOrLeft < 0)
        {
            //move left
            transform.Rotate(new Vector3(0, 0, GetComponent<EnemyStats>().defaultMoveSpeed));
        }



        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //MOVE
            //setting random dirVec
            if (isHit == true)
            {
                rigid.velocity = new Vector2(0, 0);
                yield return new WaitForSeconds(0.1f);
                dir *= -1;
                dir += new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                isHit = false;
            }
            rigid.AddForce(dir * GetComponent<EnemyStats>().defaultMoveSpeed * 10);
            yield return new WaitForSeconds(0.1f);

        }
        //ATTACK
        else
        {
            print("tiger head attack");

            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1f);

            //scatter


            //scream
            

        }


        StartCoroutine(tigerHead(dir));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            isPlayer = true;
            
            //check tiger
            /*
            if(tiger){
            stopallcoroutines();
            move toward tiger
            
            }
            
             
             */
        }
        else if (collision.tag == "Tiger" && isPlayer == true)
        {
            transform.gameObject.SetActive(false);
        }
        else if(collision.tag !="Enemy")
        {
            isHit = true;
        }
    }

}
