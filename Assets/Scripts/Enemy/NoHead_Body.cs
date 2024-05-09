using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Body : EnemyBasic
{
    private bool isTransform=false;
    public GameObject headPos;
    public GameObject head;
    private Vector2 dir = new Vector2(1, 0);



    private void OnEnable()
    { StartNamedCoroutine("body", body()); }



    private bool isHit = false;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        if (collision.tag == "PlayerAttack")
        {
            BeAttacked(collision.gameObject);
        }



    }
}
