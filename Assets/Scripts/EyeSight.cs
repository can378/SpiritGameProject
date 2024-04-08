using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSight : MonoBehaviour
{
    public bool isPlayerSeeEnemy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") 
        {
            isPlayerSeeEnemy = true;
            print("player is seeing enemy");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            isPlayerSeeEnemy = false;
            print("player is not seeing enemy");
        }
    }
}
