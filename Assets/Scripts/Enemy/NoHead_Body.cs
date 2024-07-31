using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Body : EnemyBasic
{
    private bool isTransform=false;
    bool isHit = false;
    public GameObject headPos;
    public GameObject head;
    //private Vector2 dir = new Vector2(1, 0);


    protected override void MovePattern()
    {
        if(isTransform)
        {
           Chase();
        }
    }

    protected override void AttackPattern()
    {
        StartCoroutine(ChangeMove());
    }

    IEnumerator ChangeMove()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.1f);

        enemyStatus.moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        enemyStatus.isAttack = false;

        yield return new WaitForSeconds(5f);

        enemyStatus.isAttackReady = false;
    }


    // 머리 없는 사람 몸
    // 그냥 무작위로 돌아다님
    // 머리랑 합체하면 플레이어를 쫒음

    /*
    public IEnumerator body() 
    {
        if (isTransform == false)
        {
            //move randomly
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

        }
        else 
        {
            //chase player
            head.transform.position = headPos.transform.position;
            Vector2 direction = enemyTarget.position - transform.position;
            transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);

        }


        yield return new WaitForSeconds(0.01f);

        StartCoroutine(body());
    }

    */

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (isTransform == false)
        {
            if (collision.tag == "Enemy" && collision.GetComponent<EnemyStats>().enemyName=="head")
            {
                //transform
                isTransform = true;
                head.transform.parent = headPos.transform;
                head.transform.position = headPos.transform.position;

            }
            else if (collision.tag == "Player")
            {
                //attack animation
            }
            else
            { isHit = true; }

        }

    }
}
