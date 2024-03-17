using System.Collections;
using System.Collections.Generic;
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
        if (isChange == false)
        {
            Chase();
        }
        else
        { 
            //Attack
            /*
            if(player스킬가지고 있음)
            {player 스킬 모방 사용}
            else
            {
            가까이 접근
            혼돈 부여
            멀리 도망
            }
            */
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
            transform.localScale = enemyTarget.transform.localScale;
            isChange = true;

            //Run away
            targetDirVec = enemyTarget.position - transform.position;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 1000);


        }

    }

}
