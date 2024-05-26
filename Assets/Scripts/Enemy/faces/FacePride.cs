using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FacePride : EnemyBasic
{
    //교만=투명해진다. 잠시 반투명하게 보인다. 1초 뒤에 튀어나와서 공격. 다시 투명해진다. 반복

    protected override void Attack()
    {
        if (!enemyTarget)
            return;

        targetDis = Vector2.Distance(this.transform.position, enemyTarget.position);
        targetDirVec = (enemyTarget.position - transform.position).normalized;

        //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

        if (!isRun && !isFlinch && !isAttack && isAttackReady)
        {
            moveVec = Vector2.zero;
            AttackPattern();
        }
    }
    protected override void MovePattern()
    {
        
    }
    protected override void AttackPattern()
    {
        StartCoroutine(pride());
    }


    IEnumerator pride()
    {
        isAttack = true;
        isAttackReady = false;


        //attack
        //감지
        if (targetDis <= enemyStats.detectionDis)
        {
            if (targetDis <= enemyStats.maxAttackRange)
            {
                //사정거리 안
                print("pride-attack");
                sprite.color = new Color(1f, 0f, 0f, 1f);
                yield return new WaitForSecondsRealtime(2f);
            }
            else
            {
                if (isFlinch == false && isRun == false)
                {
                    print("pride-chase");
                    Chase();
                    sprite.color = new Color(1f, 1f, 1f, 0.2f);

                }
            
            }
        }
        else
        {
            //hide
            print("pride-hide");
            sprite.color = new Color(1f, 1f, 1f, 0.005f);
        }


        //END
        isAttack = false;
        isAttackReady = true;
    }
}
