using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePride : EnemyBasic
{
    //����=����������. ��� �������ϰ� ���δ�. 1�� �ڿ� Ƣ��ͼ� ����. �ٽ� ����������. �ݺ�

    protected override void AttackPattern()
    {
        StartCoroutine(pride());
    }

    IEnumerator pride()
    {
        isAttack = true;
        isAttackReady = false;

        /*
        //attack
        if (targetDis <= 3f)
        {
            sprite.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSecondsRealtime(3f);
        }
        //chase
        else
        {
            Chase();
            sprite.color = new Color(1f, 1f, 1f, 0.5f);
        }
        */
        //END
        yield return new WaitForSeconds(1f);
        isAttack = false;
        yield return new WaitForSeconds(1f);
        isAttackReady = true;
    }
}
