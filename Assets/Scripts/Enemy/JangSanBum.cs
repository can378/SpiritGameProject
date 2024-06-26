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
            //�ָ� �ִٸ� �������� 4ȸ ������ �ٰ�����
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
            //��ó�� ������ ����ģ��
            yield return null;
        }

        isAttack = false;
        isAttackReady = true;
    }

    IEnumerator Eating() 
    {
        //�Ѿư���. ��Ƹ������Ѵ�.
        //��Ƹ����� �����̾� ū �����԰� ������ �Ϻ� ü��ȸ��
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }
    IEnumerator SnowSplash() 
    {
        //�÷��̾� �������� ���� ������ �� �Ѹ���. ���� ������ ��� �Ǹ�
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }
    IEnumerator RandomSpawn() 
    {
        //���� �� ��ȯ. 
        isAttack = true;
        isAttackReady = false;


        yield return null;

        isAttack = false;
        isAttackReady = true;
    }

}
