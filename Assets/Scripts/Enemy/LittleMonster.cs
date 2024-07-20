using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMonster : EnemyBasic
{
    
    public bool isEye;
    public bool isMouth;

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
            else if (isMouth)
            {
                if (isDetected) { Chase(); }
                else { RandomMove(); }
            }
        }
    }



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Detect player
        if (collision.tag == "Player" && isEye)
        {
            //Eye Screaming
            isDetected = true;
            for (int i = 0; i < mouths.Length; i++)
            {
                mouths[i].GetComponent<LittleMonster>().isDetected = true;
            }
        }
        base.OnTriggerEnter2D(collision);
    }
}
