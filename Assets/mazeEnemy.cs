using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeEnemy : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("maze Enemy touch player");
        }
        else
        {
            print("enemy no touch");
        }
    }
}
