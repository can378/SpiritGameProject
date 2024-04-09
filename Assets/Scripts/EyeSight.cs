using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSight : MonoBehaviour
{
    public bool isPlayerSeeEnemy=true;


    private void Update()
    {
        print(isPlayerSeeEnemy);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") 
        {
            if(collision.GetComponent<EnemyStats>().enemyName=="jigui")
            { 
                isPlayerSeeEnemy = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyStats>().enemyName == "jigui")
            {
                isPlayerSeeEnemy = false;
            }
        }
    }
}
