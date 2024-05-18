using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyBasic
{
    private int phaseNum = 1;
    private int patternNum=1;
    private int time;
    private int count=0;

    
    public GameObject circularCector;
    public GameObject hitGroundCol;
    
    public GameObject fist;
    public GameObject knife;
    public GameObject thorn;


    protected override void AttackPattern()
    {
        print("final boss");
        StartCoroutine(finalBoss());
    }



    public IEnumerator finalBoss() 
    {
        isAttack = true;
        isAttackReady = false;

        if (
            GetComponent<EnemyStats>().HP <= GetComponent<EnemyStats>().HPMax / 2
            && phaseNum==1
            )
        { phaseNum = 2; patternNum = 1; }


        if (phaseNum==1)
        {
            switch (patternNum)
            {
                case 1:
                    StartCoroutine(rushSwing());
                    break;
                case 2:
                    StartCoroutine(shotKnife());
                    break;
                case 3:
                    StartCoroutine(rushThrow());
                    break;
                case 4:
                    StartCoroutine(hitGround());
                    break;
                case 5:
                    StartCoroutine(fireShot());
                    break;
                default:
                    break;
            }
        }
        else 
        {
            switch (patternNum)
            {
                case 1:StartCoroutine(punchFist());
                    break;
                case 2:
                    StartCoroutine(knifeRun());
                    break;
                case 3:
                    StartCoroutine(wind());
                    break;
                case 4:
                    StartCoroutine(faces());
                    break;
                case 5:
                    break;
                default:
                    break;
            }
        }


        if (patternNum == 5) { patternNum = 1; }
        else { patternNum++; }
        
        yield return null;
    }

    #region Phase1
    IEnumerator rushSwing() 
    {
        print("1. rush and swing");
        //targeting
        Vector3 targetPos=enemyTarget.position;
        //wait
        yield return new WaitForSeconds(3f);
        //swing knife(circular cector)
        //?????????????????
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator shotKnife() 
    {
        print("2. shot knife");
        for(int i=0;i<3;i++) 
        {
            shot();
            yield return new WaitForSeconds(1f);
        }
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator rushThrow() 
    {
        print("3. rush throw");
        time = 6000;
        while(time>0) 
        {
            if (Vector2.Distance(enemyTarget.transform.position,transform.position)<5f)
            {
                //grab player and throw away
                targetDirVec = (enemyTarget.position - transform.position).normalized;
                enemyTarget.gameObject.GetComponent<Rigidbody2D>().AddForce(targetDirVec * 50);
                yield return new WaitForSeconds(3f);
                break;
            }
            time--;
            //Chase();
            yield return new WaitForSeconds(0.01f);
        }
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator hitGround() 
    {
        print("4. hit ground");
        hitGroundCol.SetActive(true);
        yield return new WaitForSeconds(5f);
        hitGroundCol.SetActive(false);

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator fireShot() 
    {
        print("5. fire shot");
        if (!gameObject.activeSelf) yield break;

        //원형 발사
        int roundNumA = 30;
        int roundNumB = 20;
        int curPatternCount = 10;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(
                Mathf.Cos(Mathf.PI * 2 * i / roundNum),
                Mathf.Sin(Mathf.PI * 2 * i / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 260 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);

        }

        yield return new WaitForSeconds(1.5f);
        //curPatternCount++;

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;



    }
    #endregion

    #region Phase2
    IEnumerator punchFist() 
    { 
        fist.SetActive(true);

        print("1. punchFist");
        //????????????????????????
        yield return new WaitForSeconds(1f);
        time = 0;
        count = 0;
        fist.SetActive(false);
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator knifeRun() 
    {
        knife.SetActive(true);

        print("2. knife run");
        //????????????????????????????
        yield return new WaitForSeconds(1f);


        knife.SetActive(false);

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator wind() 
    {
        print("3. wind");
        thorn.SetActive(true);
        yield return new WaitForSeconds(2f);

        time = 1000;
        while (time > 0)
        {
            enemyTarget.GetComponent<Rigidbody2D>().
                AddForce(new Vector3(-1, 0, 0) * 2);
            time--;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);
        thorn.SetActive(false);

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator faces() 
    {
        print("4. faces");
        //????????????????????????
        yield return null;

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    #endregion
}
