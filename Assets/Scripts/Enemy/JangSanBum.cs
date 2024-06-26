using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JangSanBum : EnemyBasic
{
    private int patternNum;
    private void Start()
    {
        patternNum = 0;
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

        if (targetDis > 4f)
        {
            //멀리 있다면 연속으로 4회 할퀴며 다가간다
            for (int i = 0; i < 4; i++)
            {
                targetDirVec = (enemyTarget.position - transform.position).normalized;
                //move to player
                rigid.AddForce(targetDirVec * enemyStats.defaultMoveSpeed * 200);
                //attack
            }
        }
        else
        {
            //근처에 있으면 내리친다
            yield return null;
        }

        isAttack = false;
        isAttackReady = true;
    }

    IEnumerator Eating() 
    {
        //쫓아간다. 잡아먹으려한다.
        //잡아먹으면 프렐이어 큰 피해입고 장산범은 일부 체력회복
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }
    IEnumerator SnowSplash() 
    {
        //플레이어 방향으로 넓은 범위에 눈 뿌린다. 눈에 맞으면 잠시 실명
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }
    IEnumerator RandomSpawn() 
    {
        //랜덤 적 소환. 
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }

}
