using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnvy : EnemyBasic
{
    //����=��ų���(�� ó�� ��� ������ �� ���ϰ�)

    protected override void AttackPattern()
    {
        StartCoroutine(envy());
    }

    IEnumerator envy()
    {
        isAttack = true;
        isAttackReady = false;




        isAttack = false;
        yield return new WaitForSeconds(3f);
        isAttackReady = true;

    }
}
