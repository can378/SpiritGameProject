using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePride : EnemyBasic
{
    //교만=투명해진다. 잠시 반투명하게 보인다. 1초 뒤에 튀어나와서 공격. 다시 투명해진다. 반복

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
