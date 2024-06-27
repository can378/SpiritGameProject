using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    [SerializeField] GameObject biteArea;
    [SerializeField] GameObject[] spawnCandidate;
    private int patternNum;
    private int biteTime = 1;
    private int blindTime = 5;

    private List<GameObject> spawnEnemy;
    private GameObject floor;
    int randomNum;
    float randomX, randomY;
    Bounds bounds;

    private void Start()
    {
        floor = GameManager.instance.nowRoom;
        bounds = floor.GetComponent<Collider2D>().bounds;

        
        
        hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        patternNum = 0;
    }

    private void Update()
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
                //StartCoroutine(fastAttack());
                StartCoroutine(RandomSpawn());
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
        isAttack = true;
        isAttackReady = false;

        print("faset attack");
        int time = 10;
        while(time>0)
        {
            if (targetDis > 1f)
            {
                //�ָ� �ִٸ� �������� 3ȸ ������ �ٰ�����
                targetDirVec = (enemyTarget.position - transform.position).normalized;
                //move to player
                for (int j = 0; j < 5; j++)
                {
                    rigid.AddForce(targetDirVec * enemyStats.defaultMoveSpeed * 500);
                    yield return new WaitForSeconds(0.01f);
                }
                //attack
                bite();
                yield return new WaitForSeconds(biteTime * 0.6f);
                biteArea.SetActive(false);

            }
            //��ó�� ������ ����ģ��
            else
            {
                //attack
                bite();
                biteArea.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                yield return new WaitForSeconds(3f);
                biteArea.transform.localScale = new Vector3(1, 1, 1);
                biteArea.SetActive(false);
            }
            time--;
        }
        

        isAttack = false;
        isAttackReady = true;
    }

    IEnumerator Eating() 
    {
        print("eating");
        //�Ѿư���. ��Ƹ������Ѵ�.
        //��Ƹ����� �÷��̾� ū �����԰� ������ �Ϻ� ü��ȸ��
        isAttack = true;
        isAttackReady = false;


        int time = 100;
        while (time > 0) 
        {
            if (targetDis > 3f)
            {
                Chase();
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                //���� ũ�� ������ ���
                bite();
                yield return new WaitForSeconds(biteTime * 0.6f);
                biteArea.SetActive(false);

            }
            time--;
        }
        


        isAttack = false;
        isAttackReady = true;
    }


    IEnumerator SnowSplash() 
    {
        print("snow splash");
        isAttack = true;
        isAttackReady = false;


        //�÷��̾� �������� ���� ������ �� �Ѹ���. ���� ������ ��� �Ǹ�
        bite();
        biteArea.transform.localScale = new Vector3(2, 2, 2);
        yield return new WaitForSeconds(3f);
        biteArea.transform.localScale = new Vector3(1,1, 1);
        biteArea.SetActive(false);


        isAttack = false;
        isAttackReady = true;
    }


    IEnumerator RandomSpawn()
    {
        print("random spawn");
        isAttack = true;
        isAttackReady = false;


        //���� �� ��ȯ. 
        for (int i = 0; i < 4; i++)
        {
            randomNum = UnityEngine.Random.Range(0, spawnCandidate.Length-1);
            randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            //print(spawnCandidate[i].name);
            //Instantiate(spawnCandidate[randomNum]);
            //spawnEnemy[i] = spawnCandidate[randomNum];
            //spawnEnemy[i].SetActive(true);
     
            
            //spawnEnemy[i].transform.position = new Vector2(randomX, randomY);
            
        }

        //0���� enemy�� ������ �����̹Ƿ� �� ���� �׾���� ������.
        /*
        while (spawnEnemy[0].GetComponent<EnemyStats>().HP > 0)
        {
            yield return new WaitForSeconds(0.01f);
        }
        */


        yield return new WaitForSeconds(3f);
        isAttack = false;
        isAttackReady = true;
    }

    private void bite() 
    {
        //attack
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        biteArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x) * Mathf.Rad2Deg - 90);
        biteArea.SetActive(true);
    }
    private void checkHit()
    {
        //�÷��̾ hit detection�� �ɷ��� �� �߰� ȿ��
        if (hitDetection.isHit)
        {
            if (patternNum == 2) { enemyStats.HP += 10; }
            else if (patternNum == 3) { enemyTarget.GetComponent<PlayerStats>().blind = blindTime; }
        }
    }
}
