using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSight : MonoBehaviour
{
    public bool isPlayerSeeEnemy=true;


    private void Update()
    {
        playerEyeSight();
        print(isPlayerSeeEnemy);
    }


    void playerEyeSight()
    {
        transform.position = GameObject.FindWithTag("Player").transform.position;
        if (Player.instance.hAxis == 1)
        {

            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.rotation = Quaternion.Euler(0, 0, 180);

        }
        else if (Player.instance.hAxis == -1)
        {

            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Player.instance.vAxis == 1)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.rotation = Quaternion.Euler(0, 0, -90);

        }
        else if (Player.instance.vAxis == -1)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.rotation = Quaternion.Euler(0, 0, 90);

        }
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
