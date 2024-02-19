using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPattern : EnemyBasic
{

    #region Long distance Attack

    
    public IEnumerator LRShot()
    {
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        { rigid.AddForce(targetDirVec * status.speed); }

        yield return new WaitForSeconds(0.1f);

        rigid.velocity = Vector2.zero;
        shot();
        yield return new WaitForSeconds(3);
        StartCoroutine("LRShot");
    }

    private void shot()
    {
        GameObject bullet = ObjectPoolManager.instance.Get(0);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        bullet.transform.position = transform.position;
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        rigid.AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
    }


    //multi shot=n갈래로 총을쏜다.
    public IEnumerator multiShot(int gunNum, List<GameObject> GunMuzzle)
    {

        for (int i = 0; i < gunNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get(0);
            bullet.transform.position = GunMuzzle[i].transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 targetDirVec = new Vector2(
                Mathf.Cos(Mathf.PI * 2 * i / gunNum),
                Mathf.Sin(Mathf.PI * 2 * i / gunNum));

            rigid.AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 260 * i / gunNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);

        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(multiShot(gunNum, GunMuzzle));


    }
    #endregion

    #region Melee Attack


    public IEnumerator rushHit() //돌진 후 대기 (반복)
    {
        //print("rushhit=" + status.damage);
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        { rigid.AddForce(targetDirVec * status.speed); }
        
        yield return new WaitForSeconds(0.1f);

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(3);
        StartCoroutine("rushHit");
    }





    public IEnumerator hitAndRun()
    {
        //print("hit and run=" + status.damage);
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
        if (targetDistance < status.detectionDis)
        {
            //getting closer
            do
            {
                Chase();
                targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
                yield return new WaitForSeconds(0.01f);
            } while (targetDistance > 1.2f);


            //getting farther
            do
            {
                rigid.AddForce(-targetDirVec * status.speed, ForceMode2D.Impulse);
                targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
                targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
                yield return new WaitForSeconds(0.01f);
            } while (targetDistance < 10f);

        }
        yield return new WaitForSeconds(0.01f);
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);

        StartCoroutine("hitAndRun");
    }


    //필요할 경우 활용? 아직 안씀
    public IEnumerator moveEllipse() 
    {
        //targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        int r = 10;

        for (int theta = 0; theta < 360; theta++)
        {
            transform.position = new Vector3(r * Mathf.Cos(theta * Mathf.Deg2Rad) * 0.5f, r * Mathf.Sin(theta * Mathf.Deg2Rad));
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine("moveEllipse");
    }



    



    //range attack = 범위 공격
    public IEnumerator rangeAttack(GameObject AttackRange) 
    {
        yield return new WaitForSeconds(3);


        float newScale = 1f;
        while (newScale <= 4f)
        {
            newScale += Time.deltaTime;
            AttackRange.transform.localScale = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.02f);
        }
        
        

        StartCoroutine(rangeAttack(AttackRange));
    }

    //wave attack = 파장 범위 공격
    public IEnumerator waveAttack(GameObject AttackRange, GameObject NoAttackRange) 
    {
        yield return new WaitForSeconds(3);
        //print("wave attack=" + status.damage);

        float newScale = 1f;
        while (newScale <= 9f)
        {
            newScale += Time.deltaTime;
            AttackRange.transform.localScale
                = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.001f);
        }

        newScale = 0.5f;
        while (newScale <= 1f)
        {
            newScale += Time.deltaTime;
            NoAttackRange.transform.localScale
                = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.001f);
        }

        NoAttackRange.transform.localScale
                = new Vector3(0.5f, 0.5f, 1f);
        AttackRange.transform.localScale
                = new Vector3(1f, 1f, 1f);
        StartCoroutine(waveAttack(AttackRange,NoAttackRange));

    }


    bool isAttacking = false;
    public IEnumerator pop()
    {
        //print("pop=" + status.damage);
        if (isAttacking == false)
        {

            float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

            if (targetDistance <= status.detectionDis)
            {
                //attack
                if (targetDistance <= 0.7f)
                {
                    
                    isAttacking = true;
                    sprite.color = new Color(1f, 0f, 0f, 0.5f);

                    yield return new WaitForSecondsRealtime(3f);

                    isAttacking = false;

                }
                //chase
                else
                {
                    Chase();
                    sprite.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
            else
            {
                //hide
                sprite.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(pop());
    }

    public IEnumerator chasing() 
    {
        //print("chasing=" + status.damage);
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= status.detectionDis && targetDistance >= 1f)
        {
            Chase();
        }
        yield return null;
        StartCoroutine(chasing());
    }

    bool isJumping = false;
    public IEnumerator jump()
    {
        //print("jump=" + status.damage);
        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= status.detectionDis && targetDistance >= 0.2f)
        {
            if (isJumping == false)
            {

                // 이동 방향
                Vector3 direction = enemyTarget.position - transform.position;
                direction.Normalize();

                // 이동 속도, 시간
                float jumpDuration = Vector3.Distance(enemyTarget.position, transform.position) / status.speed;

                // 점프 시작
                isJumping = true;
                float startTime = Time.time;
                float elapsedTime = 0f;

                while (elapsedTime < jumpDuration)
                {
                    transform.Translate(direction * (Time.deltaTime / jumpDuration) * status.speed);
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }

                // 점프 완료 후 상태 초기화
                isJumping = false;
            }
        }
        yield return null;
        StartCoroutine(jump());
    }
    #endregion


}
