using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryThief : EnemyBasic
{
    bool isSteal=false;
    [SerializeField] GameObject stealArea;

    protected override void Update()
    {
        base.Update();
        Steal();
    }

    protected override void MovePattern()
    {
        if (!enemyTarget)
        {
            RandomMove();
        }
        else if (isSteal)
        {
            Run();
        }
        else if (targetDis <= enemyStats.maxAttackRange)
        {
            
        }
        else
        {
            Chase();
        }
    }


    protected override void AttackPattern()
    {
        if (isSteal == false)
        {
            //StartCoroutine(StealAttack());
            return;
        }
    }

    // µµµÏÁú

    IEnumerator StealAttack()
    {
        HitDetection hitDetection;
        Vector3 hitDir = targetDirVec;

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.3f);

        hitDetection = stealArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        stealArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        stealArea.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        stealArea.SetActive(false);
        isAttack = false;
        isAttackReady = true;
    }

    void Steal()
    {
        if (isSteal || !hitTarget)
            return;
        /*
        if (hitTarget.tag == "Player")
        {
            if(hitTarget.gameObject.GetComponent<Player>().playerItem != null)
            {
                Destroy(enemyTarget.GetComponent<Player>().playerItem);
                enemyTarget.GetComponent<Player>().playerItem = null;
                //MapUIManager.instance.updateItemUI(null);
                isSteal = true;
            }
        }
        */
    }

    /*
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
    */



}
