using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMonster : EnemyBasic
{
    
    public bool isEye;
    public bool isMouth;
    public bool isDetection;

    public GameObject[] mouths;

    private bool isDetected;



    protected override void MovePattern()
    {
        if (!enemyTarget)
        {
            RandomMove();
            print("enemyTarget is null");
        }
        else
        {
            //EYE
            if (isEye)
            {
                RandomMove();
            }
            //MOUTH
            else if(isMouth==true)
            {
                if (isDetected == true) { Chase(); }
                else { RandomMove(); }
                
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&isDetection==true)
        {
            //Eye Screaming?
            isDetected = true;
            for(int i=0;i<mouths.Length;i++) 
            {
                mouths[i].GetComponent<LittleMonster>().isDetected = true;
            }
        }

        //DAMAGE
        if (collision.tag == "PlayerAttack" || collision.tag == "AllAttack")
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>());
        }
    }
}
