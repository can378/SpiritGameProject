using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum finalBossPhase { phase1, phase2};
public class FinalBoss : EnemyBasic
{

    enum FinalBossHitEffect { CIRCULARCECTOR , HITGROUNDCOL, FIST, KNIFE, THORN, FISTWAVE, ALLFACES, NONE }

    private int patternNum=1;
    [SerializeField]
    private float time;

    public finalBossPhase phase;
    public GameObject finalBoss1;
    public GameObject finalBoss2;

    [Header("2phase")]
    public List<GameObject> faces;
    public List<GameObject> facesLoc;

    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackPattern()
    {
        finalBoss();
    }


    void finalBoss() 
    {
        print("start finall boss pattern");
        

        if ( (enemyStats.HP <= (enemyStats.HPMax / 2))
            && phase == finalBossPhase.phase1 )
        {
            finalBoss2.SetActive(true);
            InitStatus();
            finalBoss1.SetActive(false);
        }

        if (phase == finalBossPhase.phase1)
        {
            switch (patternNum)
            {
                case 1:
                    
                    enemyStatus.attackCoroutine = StartCoroutine(rushSwing());
                    break;
                case 2:

                    enemyStatus.attackCoroutine = StartCoroutine(shotKnife());
                    break;
                case 3:
                    enemyStatus.attackCoroutine = StartCoroutine(rushThrow());
                    break;
                case 4:
                    enemyStatus.attackCoroutine = StartCoroutine(hitGround());
                    break;
                case 5:
                    enemyStatus.attackCoroutine = StartCoroutine(fireShot());
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
                    enemyStatus.attackCoroutine = StartCoroutine(wind());
                    break;
                case 2:
                    enemyStatus.attackCoroutine = StartCoroutine(knifeRun());
                    break;
                case 3:
                    enemyStatus.attackCoroutine = StartCoroutine(punchFist());
                    break;
                case 4:
                    enemyStatus.attackCoroutine = StartCoroutine(facesAttack());
                    break;
                default:
                    break;
            }
        }


        if (patternNum == 5) { patternNum = 1; }
        else { patternNum++; }
        
    }

    #region Phase1
    IEnumerator rushSwing() 
    {
        print("1. rush and swing");

        enemyStatus.isAttackReady = false;

        //swing knife(circular cector)
        time = 10f;
        while (time > 0)
        {
            if (enemyStatus.targetDis < 10f)
            {
                //print("swing");
                enemyStatus.moveVec = Vector2.zero;

                yield return new WaitForSeconds(0.6f);
                hitEffects[(int)FinalBossHitEffect.CIRCULARCECTOR].transform.rotation =
                    Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg);
                hitEffects[(int)FinalBossHitEffect.CIRCULARCECTOR].transform.position = transform.position;
                hitEffects[(int)FinalBossHitEffect.CIRCULARCECTOR].SetActive(true);
                yield return new WaitForSeconds(1f);
                hitEffects[(int)FinalBossHitEffect.CIRCULARCECTOR].SetActive(false);
            }
            else 
            { Chase(); }

            yield return null;
            time -= Time.deltaTime;
        }
       

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator shotKnife() 
    {
        print("2. shot knife");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

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

        enemyStatus.isAttackReady = false;
        enemyStatus.isSuperArmor = true;

        time = 10f;
        while(time>0) 
        {
            if (enemyStatus.targetDis<10f)
            {
                //grab player and throw away
                //print("grab and throw away");
                enemyStatus.moveVec = Vector2.zero;

                for (int i = 0; i < 10; i++)
                {
                    enemyStatus.enemyTarget.gameObject.GetComponent<Rigidbody2D>().AddForce((enemyStatus.enemyTarget.position - transform.position).normalized * 10000);
                    yield return new WaitForSeconds(0.01f);
                }
                
                yield return new WaitForSeconds(3f);
                break;
            }
            else { Chase(); }
            time -= Time.deltaTime;

            yield return null;
        }

        //END
        enemyStatus.isAttack = false;
        enemyStatus.isSuperArmor = false;

        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
        
    }

    IEnumerator hitGround() 
    {
        print("4. hit ground");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        hitEffects[(int)FinalBossHitEffect.HITGROUNDCOL].SetActive(true);
        yield return new WaitForSeconds(5f);
        hitEffects[(int)FinalBossHitEffect.HITGROUNDCOL].SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator fireShot() 
    {
        print("5. fire shot");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        if (!gameObject.activeSelf) yield break;

        //���� �߻�
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

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        hitEffects[(int)FinalBossHitEffect.FIST].SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Vector3 aboveTarget;
        time = 120f;

        while (time > 0)
        {
            aboveTarget = new Vector3(enemyStatus.enemyTarget.transform.position.x, enemyStatus.enemyTarget.transform.position.y + 20, 0);

            //if find Player!
            if (MoveTo(hitEffects[(int)FinalBossHitEffect.FIST], 100, hitEffects[(int)FinalBossHitEffect.FIST].transform.position, aboveTarget) == true) 
            {
                Vector3 tar = enemyStatus.enemyTarget.position;
                hitEffects[(int)FinalBossHitEffect.FIST].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                yield return new WaitForSeconds(2f);

                //����ģ��
                while (time>0 && MoveTo(hitEffects[(int)FinalBossHitEffect.FIST], 100, hitEffects[(int)FinalBossHitEffect.FIST].transform.position, tar) == false) 
                { yield return new WaitForSeconds(0.01f); time--; }


                hitEffects[(int)FinalBossHitEffect.FISTWAVE].SetActive(true);
                hitEffects[(int)FinalBossHitEffect.FISTWAVE].transform.position = tar;
                hitEffects[(int)FinalBossHitEffect.FIST].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                yield return new WaitForSeconds(2f);
                hitEffects[(int)FinalBossHitEffect.FISTWAVE].SetActive(false);
            }

            time -= Time.deltaTime;

            yield return null;
        }


        hitEffects[(int)FinalBossHitEffect.FIST].SetActive(false);

        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator knifeRun() 
    {
        print("2. knife run");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        hitEffects[(int)FinalBossHitEffect.KNIFE].SetActive(true);
        yield return new WaitForSeconds(40f);
        hitEffects[(int)FinalBossHitEffect.KNIFE].SetActive(false);


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator wind() 
    {
        print("1. wind");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        hitEffects[(int)FinalBossHitEffect.THORN].SetActive(true);
        yield return new WaitForSeconds(2f);

        time = 5f;
        while (time > 0)
        {
           enemyStatus.enemyTarget.GetComponent<Rigidbody2D>().
                AddForce(new Vector3(0, -1, 0) * 100);
            time -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        hitEffects[(int)FinalBossHitEffect.THORN].SetActive(false);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;
    }

    IEnumerator facesAttack() 
    {
        //start
        print("4. faces");

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        enemyStatus.isSuperArmor = true;

        //�⵶�� 7���� ����+�ұ� 3��

        //����=����������. ��� �������ϰ� ���δ�. 1�� �ڿ� Ƣ��ͼ� ����. �ٽ� ����������. �ݺ�
        //����=��ų���(�� ó�� ��� ������ �� ���ϰ�)
        //�г�=�Ӹ��� ��, �� ���� �� ������ �������� ���ƴٴ�
        //����=�ƹ��͵� ����(���ݾ���. ����� Ÿ�̹�)
        //Ž��=���� ũ�� ������. �̿��߿� ���ٴ��� ������µ� ���ٴڿ� ������ �ȵȴ�. ���ٴ��� ���������ʰ� �� ������ ���������� �����־ ������ �� �����ϰ� �ľ��Ѵ�.
        //����=�� ���ƴٴ�
        //����=�������� �� �߻�
        //���밨=�� ��ä�� ������� �߻�
        hitEffects[(int)FinalBossHitEffect.ALLFACES].SetActive(true);
        for (int i=0;i<faces.Count;i++) 
        {
            faces[i].GetComponent<EnemyStatus>().isAttackReady = true;
            yield return new WaitForSeconds(10f);
            faces[i].GetComponent<EnemyStatus>().isAttackReady = false;

            while (MoveTo(faces[i], 100, faces[i].transform.position, facesLoc[i].transform.position))
            { yield return new WaitForSeconds(0.01f); }
            
        }
        hitEffects[(int)FinalBossHitEffect.ALLFACES].SetActive(false);

        //Finish
        enemyStatus.isAttack = false;
        enemyStatus.isSuperArmor = false;
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
