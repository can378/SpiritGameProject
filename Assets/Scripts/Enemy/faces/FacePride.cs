using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FacePride : BossFace
{
    //����=����������. ��� �������ϰ� ���δ�. 1�� �ڿ� Ƣ��ͼ� ����. �ٽ� ����������. �ݺ�

    // protected override void Attack()
    // {
    //     if (!enemyStatus.enemyTarget)
    //         return;

    //     enemyStatus.targetDis = Vector2.Distance(this.transform.position, enemyStatus.enemyTarget.position);
    //     enemyStatus.targetDirVec = (enemyStatus.enemyTarget.position - transform.position).normalized;

    //     //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

    //     if (!enemyStatus.isRun && !enemyStatus.isFlinch && !enemyStatus.isAttack && enemyStatus.isAttackReady)
    //     {
    //         enemyStatus.moveVec = Vector2.zero;
    //         AttackPattern();
    //     }
    // }

    protected override void MovePattern()
    {
        
    }
    protected override void AttackPattern()
    {
        StartCoroutine(pride());
    }


    IEnumerator pride()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;


        //attack
        //����
        if (enemyStatus.targetDis <= enemyStats.detectionDis)
        {
            if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
            {
                //�����Ÿ� ��
                print("pride-attack");
                sprite.color = new Color(1f, 0f, 0f, 1f);
                yield return new WaitForSecondsRealtime(2f);
            }
            else
            {
                if (enemyStatus.isFlinch == false && enemyStatus.isRun == false)
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
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
    }
}
