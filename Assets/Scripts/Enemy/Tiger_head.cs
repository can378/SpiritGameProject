using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_head : EnemyBasic
{
    public GameObject screamObj;
    public GameObject screamBuff;
    public GameObject scatterBuff;

    private Transform parent;
    private GameObject detectTiger;
    private Vector2 dir = new Vector2(1, 0);



    public bool isDetectPlayer = false;
    private bool isHit = false;
    private bool isChaseTiger = false;

    protected override void AttackPattern()
    {
        StartCoroutine(tigerHead());
    }


    IEnumerator tigerHead()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        enemyStatus.targetDis = Vector2.Distance(transform.position, enemyStatus.enemyTarget.position);


        //ROLLING
        float rightOrLeft = Vector2.Dot(rigid.velocity.normalized, Vector2.right);
        if (rightOrLeft > 0)
        {
            //move right
            transform.Rotate(new Vector3(0, 0, -GetComponent<EnemyStats>().MoveSpeed.Value));
        }
        else if (rightOrLeft < 0)
        {
            //move left
            transform.Rotate(new Vector3(0, 0, GetComponent<EnemyStats>().MoveSpeed.Value));
        }

        if (isChaseTiger)
        {
            rigid.AddForce(dir * GetComponent<EnemyStats>().MoveSpeed.Value * 10);
            dir=(detectTiger.transform.position - transform.position).normalized;
            yield return new WaitForSeconds(0.1f);
        }
        else 
        { 
            if (enemyStatus.targetDis > GetComponent<EnemyStats>().detectionDis)
            {
                //MOVE
                //setting random dirVec
                if (isHit == true)
                {
                    rigid.velocity = new Vector2(0, 0);
                    yield return new WaitForSeconds(0.1f);
                    dir *= -1;
                    dir += new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                    isHit = false;
                }
                rigid.AddForce(dir * GetComponent<EnemyStats>().MoveSpeed.Value * 10);
                yield return new WaitForSeconds(0.1f);

            }
            //ATTACK
            else
            {
                //print("tiger head attack");

                rigid.velocity = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(1f);

                //scatter
                //enemyTarget.GetComponent<Player>().ApplyBuff(satterBuff);

                //scream
                screamObj.SetActive(true);
                yield return new WaitForSeconds(3f);
                screamObj.SetActive(false);
                //enemyTarget.GetComponent<Player>().ApplyBuff(screamBuff);

            }
        }

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

/*
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player"&&isDetectPlayer==false)
        {
            isHit = true;

            isDetectPlayer = true;

            if (parent != null)
            {
                foreach (Transform sibling in parent)
                {
                    //print("finding tiger now..................");
                    //find tiger
                    if (sibling != transform && sibling.GetComponent<Tiger_tiger>()!=null)
                    {
                        if (sibling.GetComponent<Tiger_tiger>().isTransform == false)
                        {
                            //print("head found tiger");
                            StopAllCoroutines();
                           

                            //move toward tiger
                            isChaseTiger = true;
                            detectTiger = sibling.gameObject;
                            dir = (detectTiger.transform.position - transform.position).normalized;

                            break;
                        }
                        
                    }
                    
                }
            }
            
        }
        else if(collision.tag !="Enemy")
        {
            isHit = true;
        }
        if (collision.tag == "PlayerAttack")
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>());
        }
    }
*/
}
