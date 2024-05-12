using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTree : EnemyBasic
{
    protected override void MovePattern()
    {
        isChase = false;
        isRun = false;
    }

    protected override void AttackPattern()
    {
        if (targetDis <= enemyStats.maxAttackRange)
        {
            StartCoroutine(fakeTree());
            return;
        }
    }

    // ��ó�� ������ ����

    IEnumerator fakeTree()
    {
        isAttack = true;
        isAttackReady = false;
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);

        isAttack = false;
        isAttackReady = true;
        sprite.color = new Color(255, 255, 255);


    }



}
