using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    enum JangSanBumHitEffect { Scratch, Bite, Snow, Crack, None };

    [SerializeField] GameObject[] spawnCandidate;
    public GameObject spawnCandidateParent;

    
    private int patternNum;
    private int biteTime = 1;
    private int blindTime = 5;

    int randomNum;

    private List<GameObject> spawnEnemy;
    private GameObject floor;
    float randomX, randomY;
    Bounds bounds;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        floor = GameManager.instance.nowRoom;
        bounds = floor.GetComponent<Collider2D>().bounds;

        patternNum = 0;

    }

    protected override void Update()
    {
        base.Update();
        checkBite();
    }

    protected override void AttackPattern()
    {
        patternNum++;
        switch (patternNum)
        {
            case 1:
                enemyStatus.attackCoroutine = StartCoroutine(FastAttack());
                break;
            case 2:
                enemyStatus.attackCoroutine = StartCoroutine(Eating());
                break;
            case 3:
                enemyStatus.attackCoroutine = StartCoroutine(SnowSplash());
                break;
            case 4:
                StartCoroutine(RandomSpawn());
                break;
            default:
                patternNum = 0;
                break;
        }
    }

    IEnumerator FastAttack()
    {
        //enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        print("faset attack");
        int time = UnityEngine.Random.Range(3,5);
        while(time > 0)
        {
            //�ָ� �ִٸ� �������� ������ �ٰ�����
        
            //move to player
            for (int j = 0; j < 5; j++)
            {
                rigid.AddForce(enemyStatus.targetDirVec * enemyStats.defaultMoveSpeed * 500);
                yield return new WaitForSeconds(0.01f);
            }

            //attack
            hitEffects[(int)JangSanBumHitEffect.Scratch].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(true);
            yield return new WaitForSeconds(biteTime * 0.6f);
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(false);
            yield return new WaitForSeconds(biteTime * 0.4f);

            time--;
        }

        //��ó�� ������ ����ģ��
        if (enemyStatus.targetDis < 4f)
        {
            //attack
            hitEffects[(int)JangSanBumHitEffect.Crack].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Crack].SetActive(true);
            yield return new WaitForSeconds(3f);
            hitEffects[(int)JangSanBumHitEffect.Crack].SetActive(false);
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                rigid.AddForce(enemyStatus.targetDirVec * enemyStats.defaultMoveSpeed * 500);
                yield return new WaitForSeconds(0.01f);
            }
            hitEffects[(int)JangSanBumHitEffect.Scratch].transform.rotation = enemyStatus.targetQuaternion;
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(true);
            yield return new WaitForSeconds(biteTime * 0.6f);
            hitEffects[(int)JangSanBumHitEffect.Scratch].SetActive(false);
            yield return new WaitForSeconds(biteTime * 0.4f);
        }

        //enemyStatus.isAttack = false;
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
                hitEffects[(int)JangSanBumHitEffect.Bite].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
                hitEffects[(int)JangSanBumHitEffect.Bite].SetActive(true);
                yield return new WaitForSeconds(biteTime * 0.6f);
                hitEffects[(int)JangSanBumHitEffect.Bite].SetActive(false);
                yield return new WaitForSeconds(biteTime * 5f);
                break;
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
        hitEffects[(int)JangSanBumHitEffect.Snow].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
        hitEffects[(int)JangSanBumHitEffect.Snow].SetActive(true);
        yield return new WaitForSeconds(2f);
        hitEffects[(int)JangSanBumHitEffect.Snow].SetActive(false);


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

    private void BiteAttack()
    {
        //attack
        hitEffects[(int)JangSanBumHitEffect.Bite].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(enemyStatus.targetDirVec.y, enemyStatus.targetDirVec.x) * Mathf.Rad2Deg - 90);
        hitEffects[(int)JangSanBumHitEffect.Bite].SetActive(true);
    }

    private void checkBite()
    {
        if(enemyStatus.hitTarget == null)
            return;

        //�÷��̾ hit detection�� �ɷ��� �� �߰� ȿ��
        if (patternNum == 2 && enemyStatus.hitTarget.CompareTag("Player"))
        {
            enemyStats.HP += 10;
        }
        else if(patternNum == 3 && enemyStatus.hitTarget.CompareTag("Player"))
        {
            enemyStatus.enemyTarget.GetComponent<PlayerStats>().blind = blindTime;
        }
    }
}
