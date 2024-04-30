using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryThief : EnemyBasic
{
    private bool isSteal=false;
    


    private void OnEnable()
    {
        StartNamedCoroutine("stealItem", stealItem());
    }



    IEnumerator stealItem() 
    {
        if (isSteal==false)
        {
            Chase();

            //steal success!
            if (Vector2.Distance(enemyTarget.transform.position, transform.position) < 1f)
            {
                if (enemyTarget.GetComponent<Player>().playerItem != null)
                {

                    Destroy(enemyTarget.GetComponent<Player>().playerItem);
                    enemyTarget.GetComponent<Player>().playerItem = null;
                    MapUIManager.instance.updateItemUI(null);
                    isSteal = true;
                }
            }
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

    
}
