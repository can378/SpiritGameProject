using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : EnemyBasic
{
    //�г�=�Ӹ��� ��, �� ���� �� ������ �������� ���ƴٴ�
    protected override void AttackPattern()
    {
        StartCoroutine(angry());
    }

    IEnumerator angry()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
