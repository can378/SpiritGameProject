using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FacePride : BossFace
{
    //교만=투명해진다. 잠시 반투명하게 보인다. 1초 뒤에 튀어나와서 공격. 다시 투명해진다. 반복

    public GameObject attackArea;

    protected override void Update()
    {
        base.Update();
        attackArea.transform.localPosition = new Vector3(0,0,0);
    }

    protected override void faceAttack() 
    {
        //attack
        //감지
        if (enemyStatus.targetDis <= enemyStats.detectionDis)
        {
            if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
            {
                //사정거리 안
                print("pride-attack");
                sprite.color = new Color(1f, 0f, 0f, 1f);
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

    }


}
