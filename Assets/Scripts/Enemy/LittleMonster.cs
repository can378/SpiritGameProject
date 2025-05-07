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
        if (!enemyStatus.EnemyTarget)
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


    /*
    protected override  void OnCollisionEnter2D(Collision2D other)
    {
        //Detect player
        if (other.collider.tag == "Player" && isEye)
        {
            //Eye Screaming
            isDetected = true;
            for (int i = 0; i < mouths.Length; i++)
            {
                mouths[i].GetComponent<LittleMonster>().isDetected = true;
            }
        }
        base.OnCollisionEnter2D(other);
    }
    */
}
