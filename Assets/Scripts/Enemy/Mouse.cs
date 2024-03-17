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
            if(player��ų������ ����)
            {player ��ų ��� ���}
            else
            {
            ������ ����
            ȥ�� �ο�
            �ָ� ����
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
