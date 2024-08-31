using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FacePride : BossFace
{
    //����=����������. ��� �������ϰ� ���δ�. 1�� �ڿ� Ƣ��ͼ� ����. �ٽ� ����������. �ݺ�

    public GameObject attackArea;

    protected override void Update()
    {
        base.Update();
        attackArea.transform.localPosition = new Vector3(0,0,0);
    }

    protected override void faceAttack() 
    {
        //attack
        //����
        if (enemyStatus.targetDis <= enemyStats.detectionDis)
        {
            if (enemyStatus.targetDis <= enemyStats.maxAttackRange)
            {
                //�����Ÿ� ��
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
