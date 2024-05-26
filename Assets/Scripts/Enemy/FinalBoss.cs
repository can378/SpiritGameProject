using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyBasic
{
    private int phaseNum = 1;
    private int patternNum=1;
    private int time;



    public GameObject circularCector;
    public GameObject hitGroundCol;
    
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
                case 1:
                    StartCoroutine(faces()); 
                    break;
                case 2:
                    StartCoroutine(knifeRun());
                    break;
                case 3:
                    StartCoroutine(wind());
                    break;
                case 4:
                    StartCoroutine(punchFist());
                    break;
                case 5:
                    isAttack = false;
                    isAttackReady = true;
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
        time = 1000;
        while (time > 0)
        {
            if (targetDis < 5f)
            {
                //print("swing");
                circularCector.transform.rotation =
                    Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x) * Mathf.Rad2Deg);
                circularCector.transform.position = transform.position;
                circularCector.SetActive(true);
                yield return new WaitForSeconds(2f);
                circularCector.SetActive(false);
            }
            else { rigid.AddForce(targetDirVec * 20); }

            yield return new WaitForSeconds(0.01f);
            time--;
        }
       

        //END
        isAttack = false;
        yield return new WaitForSeconds(1f);
        isAttackReady = true;
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
            if (targetDis<5f)
            {
                //grab player and throw away
                //print("grab and throw away");

                for (int i = 0; i < 10; i++)
                {
                    enemyTarget.gameObject.GetComponent<Rigidbody2D>().AddForce((enemyTarget.position - transform.position).normalized * 10000);
                    yield return new WaitForSeconds(0.01f);
                }
                
                yield return new WaitForSeconds(3f);
                break;
            }
            else { rigid.AddForce(targetDirVec * 50); }
            time--;

            yield return new WaitForSeconds(0.01f);
        }

        //END
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


        //END
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;



    }
    #endregion




    #region Phase2
    IEnumerator punchFist() 
    { 
        
        print("1. punchFist");

        fist.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Vector3 aboveTarget;
        time = 100;

        while (time > 0)
        {
            aboveTarget = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y + 20, 0);

            //if find Player!
            if (MoveTo(fist, 100, fist.transform.position, aboveTarget) == true) 
            {
                Vector3 tar = enemyTarget.position;
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
        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
    }

    IEnumerator knifeRun() 
    {
        print("2. knife run");

        knife.SetActive(true);
        yield return new WaitForSeconds(10f);
        knife.SetActive(false);


        //END
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
                AddForce(new Vector3(0, -1, 0) * 200);
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
        yield return new WaitForSeconds(10f);
        allFaces.SetActive(false);

        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;
        
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
