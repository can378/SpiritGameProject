using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FacePride : BossFace
{
    //교만=투명해진다. 잠시 반투명하게 보인다. 1초 뒤에 튀어나와서 공격. 다시 투명해진다. 반복

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
        //감지

        attackArea.transform.localPosition = new Vector3(0, 0, 0);
        if (enemyStatus.targetDis <= 15f)
        {
            if (enemyStatus.targetDis <= 5.5f)
            {
                //사정거리 안
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
