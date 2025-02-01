using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : EnemyBasic
{
    SnowBallStatus snowBallStatus;

    protected override void Awake()
    {
        base.Awake();
        status = snowBallStatus = snowBallStatus = GetComponent<SnowBallStatus>();
    }

    protected override void Update()
    {
        base.Update();
        SizeUp();
    }

    void SizeUp()
    {
        if (snowBallStatus.moveVec != Vector2.zero && snowBallStatus.size < 4f && !snowBallStatus.isAttack)
        {
            snowBallStatus.size += Time.deltaTime * 0.1f;
        }

        if(snowBallStatus.hitTarget)
        {
            print("snowball hit");
            snowBallStatus.isHit = true;
            if (snowBallStatus.size >= 1.5f)
            {
                snowBallStatus.size -= 1f;
            }
            else
            {
                Dead();
            }
        }

        transform.localScale = new Vector3(snowBallStatus.size, snowBallStatus.size, 1f);
        enemyStats.AttackPower.IncreasedValue = snowBallStatus.size - 1f;
    }

    protected override void AttackPattern()
    {
        if (snowBallStatus.targetDis <= enemyStats.maxAttackRange)
        {
            snowBallStatus.attackCoroutine = StartCoroutine(Tackle());
        }
    }

    IEnumerator Tackle()
    {
        // µ¹Áø Àü ÁØºñ
        snowBallStatus.isAttack = true;
        snowBallStatus.isAttackReady = false;
        hitEffects[0].GetComponent<HitDetection>().SetHit_Ratio(10, 1, enemyStats.AttackPower, 20);
        yield return new WaitForSeconds(0.6f);

        hitEffects[0].gameObject.SetActive(true);
        snowBallStatus.moveVec = snowBallStatus.targetDirVec * 5;
        for(int i = 0; i < 20 ;i++)
        {
            snowBallStatus.moveVec -= snowBallStatus.moveVec * 0.1f;
            if(snowBallStatus.isHit || (0 < snowBallStatus.isFlinch))
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        // ¸ØÃã
        snowBallStatus.moveVec = new Vector3(0, 0, 0);
        hitEffects[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        snowBallStatus.isAttack = false;
        snowBallStatus.isAttackReady = true;
        snowBallStatus.isHit = false;
        
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

    public override void InitStatus()
    {
        snowBallStatus.isHit = false;
        base.InitStatus();
    }


}
