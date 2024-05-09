using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : EnemyBasic
{
    private Vector2 dir=new Vector2(1,0);


    private void OnEnable()
    {
        StartNamedCoroutine("snowBall", snowBall());
    }

    private bool isSnowBallAngry = false;
    private bool isHit = false;
    //private bool isSnowBallPause = false;
    IEnumerator snowBall()
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
                //float randomAngle = Random.Range(0, 360);
                dir *= -1;
                //dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;//random direction
                dir += new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                isHit = false;
            }
            rigid.AddForce(dir * GetComponent<EnemyStats>().defaultMoveSpeed * 20);
            yield return new WaitForSeconds(0.1f);

            //STRONGER
            if (transform.localScale.x <= 4)
            {
                transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
        //ATTACK
        else
        {
            print("snow ball attack");

            isSnowBallAngry = true;

            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1f);

            //move Backward
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 20);

            //rush to target
            while (targetDis > 0 && isSnowBallAngry == true)
            {
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                //targetDirVec = (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 50);
                yield return new WaitForSeconds(0.1f);
                print("rush snowball");
            }

            //pause
            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(4f);
            isSnowBallAngry = false;

        }



        //NEXT
        StartCoroutine(snowBall());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //stop attacking if is hitted by something
        isSnowBallAngry = false;
        isHit = true;
        transform.localScale = new Vector3(1, 1, 1);

        
        
        if (collision.tag == "PlayerAttack")
        {
            BeAttacked(collision.gameObject);
        }
    }




}
