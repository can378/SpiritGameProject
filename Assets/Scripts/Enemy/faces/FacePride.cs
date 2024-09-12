using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FacePride : BossFace
{
    //����=����������. ��� �������ϰ� ���δ�. 1�� �ڿ� Ƣ��ͼ� ����. �ٽ� ����������. �ݺ�

    public GameObject attackArea;

    protected override void init()
    {
        base.init();
        attackArea.SetActive(false);
    }
    protected override void Finish()
    {
        base.Finish();
        attackArea.SetActive(false);
    }
    protected override void faceAttack() 
    {
        //print("pride");
        //attack
        //����

        attackArea.transform.localPosition = new Vector3(0, 0, 0);
        if (enemyStatus.targetDis <= 15f)
        {
            if (enemyStatus.targetDis <= 5.5f)
            {
                //�����Ÿ� ��
                //print("pride-attack");
                sprite.color = new Color(1f, 0f, 0f, 1f);
                attackArea.SetActive(true);
            }
            else
            {
                if (enemyStatus.isFlinch == false && enemyStatus.isRun == false)
                {
                    //print("pride-chase");
                    Chase();
                    sprite.color = new Color(1f, 1f, 1f, 0.2f);
                    attackArea.SetActive(false);
                }

            }
        }
        else
        {
            //hide
            //print("pride-hide");
            Chase();
            sprite.color = new Color(1f, 1f, 1f, 0.05f);
            attackArea.SetActive(false);
        }

    }


}
