using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum finalBossPhase { phase1, phase2};
public class FinalBoss : EnemyBasic
{

    private int patternNum=1;
    private int time;

    public finalBossPhase phase;
    public GameObject finalBoss1;
    public GameObject finalBoss2;

    [Header("phase1")]
    public GameObject circularCector;
    public GameObject hitGroundCol;

    [Header("phase2")]
    public GameObject fist;
    public GameObject knife;
    public GameObject thorn;
    public GameObject fistWave;
    public GameObject allFaces;


    protected override void AttackPattern()
    {
        StartCoroutine(finalBoss());
    }


    public IEnumerator finalBoss() 
    {
        print("start finall boss pattern");
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        if ( GetComponent<EnemyStats>().HP <= GetComponent<EnemyStats>().HPMax / 2
            && phase==finalBossPhase.phase1 )
        {
            finalBoss2.SetActive(true);
            StopAllCoroutines();
            finalBoss1.SetActive(false);
        }


        if (phase==finalBossPhase.phase1)
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
                case 1:
                    StartCoroutine(wind());
                    break;
                case 2:
                    StartCoroutine(knifeRun());
                    break;
                case 3:
                    StartCoroutine(punchFist());
                    break;
                case 4:
                    StartCoroutine(faces());
                    break;
                case 5:
                    enemyStatus.isAttack = false;
                    enemyStatus.isAttackReady = true;
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

        //isChase = true;

        //swing knife(circular cector)
        time = 2000;
        while (time > 0)
        {
            if (enemyStatus.targetDis < 10f)
            {
                //print("swing");
                circularCector.transform.rotation =
                    Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg);
                circularCector.transform.position = transform.position;
                circularCector.SetActive(true);
                yield return new WaitForSeconds(2f);
                circularCector.SetActive(false);
            }
            else { rigid.AddForce(enemyStatus.targetDirVec * 200); }

            yield return new WaitForSeconds(0.01f);
            time--;
        }
       

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator shotKnife() 
    {
        print("2. shot knife");

        for(int i=0;i<3;i++) 
        {
            shotWhat("knife");
            yield return new WaitForSeconds(2f);
        }

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator rushThrow() 
    {
        print("3. rush throw");
        time = 5000;
        while(time>0) 
        {
            if (enemyStatus.targetDis<10f)
            {
                //grab player and throw away
                //print("grab and throw away");

                for (int i = 0; i < 10; i++)
                {
                    enemyStatus.enemyTarget.gameObject.GetComponent<Rigidbody2D>().AddForce((enemyStatus.enemyTarget.position - transform.position).normalized * 10000);
                    yield return new WaitForSeconds(0.01f);
                }
                
                yield return new WaitForSeconds(3f);
                break;
            }
            else { rigid.AddForce(enemyStatus.targetDirVec * 500); }
            time--;

            yield return new WaitForSeconds(0.01f);
        }

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator hitGround() 
    {
        print("4. hit ground");

        hitGroundCol.SetActive(true);
        yield return new WaitForSeconds(5f);
        hitGroundCol.SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
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


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;



    }
    #endregion




    #region Phase2
    IEnumerator punchFist() 
    { 
        
        print("3. punchFist");

        fist.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Vector3 aboveTarget;
        time = 100;

        while (time > 0)
        {
            aboveTarget = new Vector3(enemyStatus.enemyTarget.transform.position.x, enemyStatus.enemyTarget.transform.position.y + 20, 0);

            //if find Player!
            if (MoveTo(fist, 100, fist.transform.position, aboveTarget) == true) 
            {
                Vector3 tar = enemyStatus.enemyTarget.position;
                fist.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                yield return new WaitForSeconds(2f);

                //내리친다
                while (time>0 && MoveTo(fist, 100, fist.transform.position, tar) == false) 
                { yield return new WaitForSeconds(0.01f); time--; }

                
                fistWave.SetActive(true);
                fistWave.transform.position = tar;
                fist.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                yield return new WaitForSeconds(2f);
                fistWave.SetActive(false);
            }
            time--;
            yield return new WaitForSeconds(0.1f);
        }


        fist.SetActive(false);

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator knifeRun() 
    {
        print("2. knife run");

        knife.SetActive(true);
        yield return new WaitForSeconds(10f);
        knife.SetActive(false);


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator wind() 
    {
        print("1. wind");
        thorn.SetActive(true);
        yield return new WaitForSeconds(2f);

        time = 1000;
        while (time > 0)
        {
           enemyStatus.enemyTarget.GetComponent<Rigidbody2D>().
                AddForce(new Vector3(0, -1, 0) * 300);
            time--;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);
        thorn.SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator faces() 
    {
        print("4. faces");
        //기독교 7가지 대죄+불교 3독

        //교만=투명해진다. 잠시 반투명하게 보인다. 1초 뒤에 튀어나와서 공격. 다시 투명해진다. 반복
        //질투=스킬모방(쥐 처럼 대신 강도는 더 강하게)
        //분노=머리랑 귀, 코 에서 불 나오고 랜덤으로 돌아다님
        //나태=아무것도 안함(공격안함. 쉬어가는 타이밍)
        //탐욕=입을 크게 벌린다. 이와중에 혓바닥이 길어지는데 혓바닥에 닿으면 안된다. 혓바닥은 지워지지않고 이 페이즈 끝날때까지 남아있어서 무빙을 잘 생각하고 쳐야한다.
        //무지=막 돌아다님
        //착각=무작위로 총 발사
        //적대감=총 부채꼴 모양으로 발사


        allFaces.SetActive(true);
        yield return new WaitForSeconds(5f);
        allFaces.SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
        
    }

    #endregion



    bool MoveTo(GameObject obj, float speed, Vector3 from, Vector3 to)
    {
        Vector3 vec = (to - from).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(vec * speed);
        if (Vector2.Distance(obj.transform.position, to) < 3f)
        { return true; }
        return false;
    }



}
