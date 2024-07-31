using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    HitDetection biteHD;
    [SerializeField] GameObject[] spawnCandidate;
    public GameObject spawnCandidateParent;

    private int patternNum;
    private int biteTime = 1;
    private int blindTime = 5;

    private List<GameObject> spawnEnemy;
    private GameObject floor;
    int randomNum;
    float randomX, randomY;
    Bounds bounds;

    protected override void Start()
    {
        base.Start();
        floor = GameManager.instance.nowRoom;
        bounds = floor.GetComponent<Collider2D>().bounds;

        
        
        biteHD = hitEffects[0].GetComponent<HitDetection>();
        patternNum = 0;
    }

    protected override void Update()
    {
        base.Update();
        checkHit();
    }

    protected override void AttackPattern()
    {
        StartCoroutine(tiger());
    }
    IEnumerator tiger()
    {

        patternNum++;

        switch (patternNum)
        {
            case 1:
                StartCoroutine(fastAttack());
                //StartCoroutine(test());
                //StartCoroutine(RandomSpawn());
                break;
            case 2:
                StartCoroutine(Eating());
                break;
            case 3:
                StartCoroutine(SnowSplash());
                break;
            case 4:
                StartCoroutine(RandomSpawn());
                break;
            default:
                patternNum = 0;
                break;

        }
        yield return null;

    }

    IEnumerator fastAttack()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        print("faset attack");
        int time1 = 10;
        while(time1>0)
        {
            if (enemyStatus.targetDis > 1f)
            {
                //�ָ� �ִٸ� �������� 3ȸ ������ �ٰ�����
                enemyStatus.targetDirVec = (enemyStatus.enemyTarget.position - transform.position).normalized;
                //move to player
                for (int j = 0; j < 5; j++)
                {
                    rigid.AddForce(enemyStatus.targetDirVec * enemyStats.defaultMoveSpeed * 500);
                    yield return new WaitForSeconds(0.01f);
                }
                //attack
                bite();
                yield return new WaitForSeconds(biteTime * 0.6f);
                hitEffects[0].SetActive(false);

            }
            //��ó�� ������ ����ģ��
            else
            {
                //attack
                bite();
                hitEffects[0].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                yield return new WaitForSeconds(3f);
                hitEffects[0].transform.localScale = new Vector3(1, 1, 1);
                hitEffects[0].SetActive(false);
            }
            time1--;
        }
        

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }

    IEnumerator Eating() 
    {
        print("eating");
        //�Ѿư���. ��Ƹ������Ѵ�.
        //��Ƹ����� �÷��̾� ū �����԰� ������ �Ϻ� ü��ȸ��
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        int time2 = 100;
        while (time2 > 0) 
        {
            if (enemyStatus.targetDis > 3f)
            {
                Chase();
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                //���� ũ�� ������ ���
                bite();
                yield return new WaitForSeconds(biteTime * 0.6f);
                hitEffects[0].SetActive(false);

            }
            time2--;
        }
        


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }


    IEnumerator SnowSplash() 
    {
        print("snow splash");
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        //�÷��̾� �������� ���� ������ �� �Ѹ���. ���� ������ ��� �Ǹ�
        bite();
        hitEffects[0].transform.localScale = new Vector3(2, 2, 2);
        yield return new WaitForSeconds(4f);
        hitEffects[0].transform.localScale = new Vector3(1,1, 1);
        hitEffects[0].SetActive(false);


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }


    IEnumerator RandomSpawn()
    {
        print("random spawn");


        /*
        for (int i = 0; i < 4; i++)
        {
            //print(spawnCandidate[i].name);
            randomNum = UnityEngine.Random.Range(0, spawnCandidate.Length);
            spawnEnemy[i] = spawnCandidate[randomNum];
            spawnEnemy[i].SetActive(true);
            spawnEnemy[i].transform.position = new Vector2(randomX, randomY);
        }
        */



        //START
        gameObject.transform.position = new Vector3(-350, -1.4f, 0f);
        spawnCandidateParent.SetActive(true);
        //���� �� ��ȯ
        for (int i = 0; i < spawnCandidate.Length; i++)
        {

            randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            spawnCandidate[i].transform.position = new Vector2(randomX, randomY);

            spawnCandidate[i].GetComponent<EnemyStats>().HP = spawnCandidate[i].GetComponent<EnemyStats>().HPMax;
        }

        
        //CHECK
        //randumNum ��° enemy�� ������ �����̹Ƿ� �� ���� �׿����� ������.
        randomNum = UnityEngine.Random.Range(0, spawnCandidate.Length);
        print("real jangsanbum num="+ spawnCandidate[randomNum].name);

        while (true)
        {
            if (spawnCandidate[randomNum].GetComponent<EnemyStats>().HP <= 10){ break; }
            yield return new WaitForSeconds(0.1f);
        }


        //END
        print("spawn enemy end!!!!!!!!!!!!!!!!!!!!!!!");
        spawnCandidateParent.SetActive(false);
        gameObject.transform.position = GameManager.instance.nowRoom.transform.position;
        yield return new WaitForSeconds(2f);


        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        yield return null;
        
    }


    private void bite() 
    {
        //attack
        biteHD.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].SetActive(true);
    }
    private void checkHit()
    {
        //�÷��̾ hit detection�� �ɷ��� �� �߰� ȿ��
        if (biteHD.isHit)
        {
            if (patternNum == 2) { enemyStats.HP += 10; }
            else if (patternNum == 3) { enemyStatus.enemyTarget.GetComponent<PlayerStats>().blind = blindTime; }
        }
    }
}
