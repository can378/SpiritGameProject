using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : EnemyBasic
{
    float randomMove = 0;
    float size = 1f;
    bool isHit;
    [SerializeField] HitDetection snowballHitDetection;

    protected override void Update()
    {
        base.Update();
        Size();
        randomMove -= Time.deltaTime;
    }

    void Size()
    {
        if (moveVec != Vector2.zero && size < 4f && !isAttack)
        {
            size += Time.deltaTime * 0.1f;
        }

        if(hitTarget)
        {
            isHit = true;
            if (size >= 1f)
            {
                size -= 0.5f;
            }
            else
            {
                Dead();
            }
        }

        transform.localScale = new Vector3(size, size, 1f);
        enemyStats.increasedAttackPower = size - 1f;
        snowballHitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower * size, 10, 0, 0, null);

    }

    protected override void MovePattern()
    {
        if (-1f < randomMove && randomMove < 0f)
        {
            moveVec = Vector2.zero;
            randomMove -= 1;
        }
        else if(randomMove < -3f)
        {
            moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
            randomMove = Random.Range(2, 5);
        }
    }

    protected override void AttackPattern()
    {
        if (targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(Tackle());
        }
    }

    IEnumerator Tackle()
    {
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

        // µ¹Áø Àü ÁØºñ
        isAttack = true;
        isAttackReady = false;
        snowballHitDetection.gameObject.SetActive(true);
        snowballHitDetection.SetHitDetection(false,-1,false,-1,enemyStats.attackPower * size,20,0,0,null);
        snowballHitDetection.user = this.gameObject;
        yield return new WaitForSeconds(0.6f);

        moveVec = targetDirVec * 5;
        for(int i = 0; i < 10 ;i++)
        {
            moveVec -= moveVec * 0.1f;
            if(isHit)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        // ¸ØÃã
        moveVec = new Vector3(0, 0, 0);
        snowballHitDetection.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        isAttack = false;
        isAttackReady = true;
        isHit = false;
        
    }

    /*
    IEnumerator snowBall()
    {
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);


        //ROLLING
        float rightOrLeft = Vector2.Dot(rigid.velocity.normalized, Vector2.right);
        if (rightOrLeft > 0)
        {
            //move right
            transform.Rotate(new Vector3(0, 0, -GetComponent<EnemyStats>().defaultMoveSpeed));
        }
        else if (rightOrLeft < 0)
        {
            //move left
            transform.Rotate(new Vector3(0, 0, GetComponent<EnemyStats>().defaultMoveSpeed));
        }







        if (targetDis > GetComponent<EnemyStats>().detectionDis)
        {
            //MOVE
            //setting random dirVec
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

            //STRONGER
            if (transform.localScale.x <= 4)
            {
                transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
        //ATTACK
        else
        {
            print("snow ball attack");

            isSnowBallAngry = true;

            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1f);

            //move Backward
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 20);

            //rush to target
            while (targetDis > 0 && isSnowBallAngry == true)
            {
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                //targetDirVec = (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 50);
                yield return new WaitForSeconds(0.1f);
                print("rush snowball");
            }

            //pause
            rigid.velocity = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(4f);
            isSnowBallAngry = false;

        }



        //NEXT
        StartCoroutine(snowBall());
    }
*/

}
