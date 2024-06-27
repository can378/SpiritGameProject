using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    [SerializeField] GameObject biteArea;
    private int patternNum;
    private int biteTime = 1;
    private int blindTime = 5;
    public List<GameObject> spawnEnemies;

    private void Start()
    {
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
                StartCoroutine(fastAttack());
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
                //멀리 있다면 연속으로 3회 할퀴며 다가간다
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
            //근처에 있으면 내리친다
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
        //쫓아간다. 잡아먹으려한다.
        //잡아먹으면 플레이어 큰 피해입고 장산범은 일부 체력회복
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
                //입을 크게 벌리는 모션
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
        //플레이어 방향으로 넓은 범위에 눈 뿌린다. 눈에 맞으면 잠시 실명
        isAttack = true;
        isAttackReady = false;

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
        //랜덤 적 소환. 
        isAttack = true;
        isAttackReady = false;

        int realTiger = 0;
        for (int i = 0; i < 10; i++)
        {
            //spawnEnemies[i]=Instantiate()
        }

        yield return null;

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
        //플레이어가 hit detection에 걸렸을 때 추가 효과
        if (hitDetection.isHit)
        {
            if (patternNum == 2) { enemyStats.HP += 10; }
            else if (patternNum == 3) { enemyTarget.GetComponent<PlayerStats>().blind = blindTime; }
        }
    }
}
