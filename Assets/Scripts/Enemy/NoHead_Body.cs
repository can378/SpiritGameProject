using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHead_Body : EnemyBasic
{
    private bool isTransform=false;
    public GameObject headPos;

    void Start()
    { StartCoroutine(body(Vector2.right)); }

    private void OnEnable()
    { StartCoroutine(body(Vector2.right)); }

    private void OnDisable()
    { StopCoroutine(body(Vector2.right)); }


    private bool isHit = false;
    public IEnumerator body(Vector2 dir) 
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
            Vector2 direction = enemyTarget.position - transform.position;
            transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);

        }


        yield return new WaitForSeconds(0.01f);

        StartCoroutine(body(dir));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTransform == false)
        {
            if (collision.name == "head")
            {
                //transform
                collision.gameObject.transform.parent = transform;
                collision.transform.position = headPos.transform.position;
                isTransform = true;
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
            PlayerAttack(collision.gameObject);
        }



    }
}
