using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPattern : EnemyBasic
{


    #region Long distance Attack

    
    public IEnumerator LRShot(bool isRepeat)
    {
        for (int i = 0; i < 2; i++)
        {
            targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);

            for (int j = 0; j < 100; j++)
            { rigid.AddForce(targetDirVec * Mathf.Pow(-1, i) * 2 * Mathf.Pow(3, i)); }
            //{ rigid.AddForce(targetDirVec * Mathf.Pow(-1,i) * stats.speed*Mathf.Pow(3,i)); }

            yield return new WaitForSeconds(0.1f);
            rigid.velocity = Vector2.zero;

            shot();
        }
        
        yield return new WaitForSeconds(3);

        if(isRepeat==true) StartCoroutine(LRShot(true));
    }

    


    //multi shot=n갈래로 총을쏜다.
    public IEnumerator multiShot(int gunNum, List<GameObject> GunMuzzle, bool isRepeat)
    {

        for (int i = 0; i < gunNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get(0);

            bullet.transform.position = GunMuzzle[i].transform.position;
            bullet.transform.rotation = Quaternion.identity;

            targetDirVec = new Vector2(
                Mathf.Cos(Mathf.PI * 2 * i / gunNum),
                Mathf.Sin(Mathf.PI * 2 * i / gunNum));

            bullet.GetComponent<Rigidbody2D>().
                AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 260 * i / gunNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);

        }

        yield return new WaitForSeconds(1.5f);
        if (isRepeat == true) 
            StartCoroutine(multiShot(gunNum, GunMuzzle,true));


    }
    #endregion

    #region Melee Attack


    public IEnumerator rushHit(bool isRepeat) //돌진 후 대기 (반복)
    {
        //print("rushhit=" + stats.damage);
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        { rigid.AddForce(targetDirVec * stats.defaultMoveSpeed); }
        
        yield return new WaitForSeconds(0.1f);

        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(3);


        if (isRepeat == true) 
            StartCoroutine(rushHit(true));
    }




    [HideInInspector]
    public bool isHARRun = false;
    public IEnumerator hitAndRun(bool isRepeat)
    {
        isHARRun = true;
        //print("hit and run=" + stats.damage);
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        //if (targetDis < stats.detectionDis)
        //{
            //getting closer
            do
            {
                Chase();
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                yield return new WaitForSeconds(0.01f);
            } while (targetDis > 1.2f);


            //getting farther
            do
            {
                rigid.AddForce(-targetDirVec * stats.defaultMoveSpeed, ForceMode2D.Impulse);
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
                yield return new WaitForSeconds(0.01f);
            } while (targetDis < 10f);

        //}
        yield return new WaitForSeconds(0.01f);
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);

        
        if (isRepeat == true)
            StartCoroutine(hitAndRun(true));
        isHARRun = false;
    }


    //필요할 경우 활용? 아직 안씀
    public IEnumerator moveEllipse(bool isRepeat) 
    {
        //targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        int r = 10;

        for (int theta = 0; theta < 360; theta++)
        {
            transform.position = new Vector3(r * Mathf.Cos(theta * Mathf.Deg2Rad) * 0.5f, r * Mathf.Sin(theta * Mathf.Deg2Rad));
            yield return new WaitForSeconds(0.01f);
        }

        if (isRepeat == true) 
            StartCoroutine(moveEllipse(true));
    }



    //본인의 주변을 공격
    [HideInInspector]
    public bool isPARun = false;
    public IEnumerator peripheralAttack(GameObject colObj, float radius,float attackTime, bool isRepeat)
    {
        isPARun = true;
        //isCorRun = true;
        //extend collider itself

        float originRadius=colObj.GetComponent<CircleCollider2D>().radius;
        colObj.GetComponent<CircleCollider2D>().radius = radius;
        //print("peripheralAttack");

        yield return new WaitForSeconds(attackTime);

        colObj.GetComponent<CircleCollider2D>().radius = originRadius;



        if (isRepeat == true)
        {
            for (int i = 0; i < 10; i++) { Chase(); }

            yield return new WaitForSeconds(3f);
            StartCoroutine(peripheralAttack(colObj,radius, attackTime, isRepeat));
            
        }
            

        isPARun = false;
        
    }



    //range attack = 범위 공격
    public IEnumerator rangeAttack(GameObject AttackRange, bool isRepeat) 
    {
        yield return new WaitForSeconds(3);


        float newScale = 1f;
        while (newScale <= 4f)
        {
            newScale += Time.deltaTime;
            AttackRange.transform.localScale = new Vector3(newScale, newScale, 1f);
            yield return new WaitForSeconds(0.02f);
        }



        if (isRepeat == true) 
            StartCoroutine(rangeAttack(AttackRange,true));
    }

    //wave attack = 파장 범위 공격
    public IEnumerator waveAttack(GameObject AttackRange, GameObject NoAttackRange, bool isRepeat) 
    {
        yield return new WaitForSeconds(3);
        //print("wave attack=" + stats.damage);

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
        if (isRepeat == true) 
            StartCoroutine(waveAttack(AttackRange,NoAttackRange,true));

    }


    bool isAttacking = false;
    public IEnumerator pop(bool isRepeat)
    {
        //print("pop=" + stats.damage);
        if (isAttacking == false)
        {

            targetDis = Vector2.Distance(transform.position, enemyTarget.position);

            if (targetDis <= stats.detectionDis)
            {
                //attack
                if (targetDis <= 0.7f)
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

        if (isRepeat == true) 
            StartCoroutine(pop(true));
    }

    public IEnumerator chasing() 
    {
        //print("chasing=" + stats.damage);
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDis <= stats.detectionDis && targetDis >= 1f)
        {
            Chase();
        }
        yield return null;
        StartCoroutine(chasing());
    }

    bool isJumping = false;
    public IEnumerator jump(bool isRepeat)
    {
        //print("jump=" + stats.damage);
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDis <= stats.detectionDis && targetDis >= 0.2f)
        {
            if (isJumping == false)
            {

                // 이동 방향
                Vector3 direction = enemyTarget.position - transform.position;
                direction.Normalize();

                // 이동 속도, 시간
                float jumpDuration = Vector3.Distance(enemyTarget.position, transform.position) / stats.defaultMoveSpeed;

                // 점프 시작
                isJumping = true;
                float startTime = Time.time;
                float elapsedTime = 0f;

                while (elapsedTime < jumpDuration)
                {
                    transform.Translate(direction * (Time.deltaTime / jumpDuration) * stats.defaultMoveSpeed);
                    elapsedTime = Time.time - startTime;
                    yield return null;
                }

                // 점프 완료 후 상태 초기화
                isJumping = false;
            }
        }
        yield return null;
        if (isRepeat == true) 
            StartCoroutine(jump(true));
    }
    #endregion

    public IEnumerator Wander(bool isRepeat)
    {
        print("wander");

        float randomX, randomY;
        float time = 0;

        while (time < 0.1f)
        {
            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);


            //range value 조절 필요
            randomX = Random.Range(-25, 25);
            randomY = Random.Range(-25, 25);
            targetDirVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;
            //rigid.AddForce(targetDirVec * stats.speed);
            rigid.AddForce(targetDirVec * 2);
            yield return new WaitForSeconds(0.1f);

            time += Time.deltaTime;
        }


        if (isRepeat == true) 
            StartCoroutine(Wander(true));
    }


    [HideInInspector]
    public bool isBeamRun = false;
    public IEnumerator Beam(GameObject beam,float time, float range, bool isRepeat,GameObject target)
    {
        isBeamRun = true;
        float rangeScale = 0;

        beam.SetActive(true);

        targetDirVec = (enemyTarget.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, targetDirVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, GetComponent<EnemyStats>().defaultMoveSpeed * Time.deltaTime);
    
        
        while (rangeScale <= range)
        {
            rangeScale += Time.deltaTime;
            beam.transform.localScale += new Vector3(rangeScale, 0, 0);
        }
        
        yield return new WaitForSeconds(1f);

        while (rangeScale >=0)
        {
            rangeScale -= Time.deltaTime;
            beam.transform.localScale += new Vector3(rangeScale, 0, 0);
        }

        beam.SetActive(false);

        if (isRepeat == true) StartCoroutine(Beam(beam, time, range, isRepeat,target));
        isBeamRun = false;
    }


}
