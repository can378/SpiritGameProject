using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGreed : EnemyBasic
{

    //Ž��=���� ũ�� ������. �̿��߿� ���ٴ��� ������µ� ���ٴڿ� ������ �ȵȴ�. ���ٴ��� ���������ʰ� �� ������ ���������� �����־ ������ �� �����ϰ� �ľ��Ѵ�.

    protected override void AttackPattern()
    {
        StartCoroutine(greed());
    }

    IEnumerator greed()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
