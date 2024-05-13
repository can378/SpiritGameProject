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
        StealItem();
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
            StartCoroutine(stealItem());
            return;
        }
    }

    // µµµÏÁú

    IEnumerator stealItem()
    {
        isAttack = true;
        isAttackReady = false;

        yield return new WaitForSeconds(0.5f);

        HitDetection hitDetection = stealArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;

        stealArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        stealArea.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        stealArea.SetActive(false);

        isAttack = false;
        isAttackReady = true;
    }

    void StealItem()
    {
        if (isSteal || !hitTarget)
            return;

        if (hitTarget.tag == "Player")
        {
            if(hitTarget.gameObject.GetComponent<Player>().playerItem != null)
            {
                Destroy(enemyTarget.GetComponent<Player>().playerItem);
                enemyTarget.GetComponent<Player>().playerItem = null;
                MapUIManager.instance.updateItemUI(null);
                isSteal = true;
            }
        }
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
