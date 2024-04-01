using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryThief : EnemyBasic
{
    private bool isSteal=false;
    
    void Start()
    {
        StartCoroutine(stealItem());
    }

    private void OnEnable()
    {
        StartCoroutine(stealItem());
    }

    private void OnDisable()
    {
        StopCoroutine(stealItem());
    }

    IEnumerator stealItem() 
    {
        if (isSteal==false)
        {
            Chase();
        }
        else 
        {
            //runaway

            targetDirVec = (enemyTarget.position - transform.position).normalized;
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed*10);


        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(stealItem());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (enemyTarget.GetComponent<Player>().playerItem != null)
            {

                Destroy(enemyTarget.GetComponent<Player>().playerItem);
                enemyTarget.GetComponent<Player>().playerItem = null;
                MapUIManager.instance.updateItemUI(null);
                isSteal = true;
            }
            
        }
        if (collision.tag == "PlayerAttack")
        {
            PlayerAttack(collision.gameObject);
        }
    }
}
