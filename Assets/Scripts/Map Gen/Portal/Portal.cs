using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    public Transform Destination;
    void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.transform.position = Destination.position;
        }
    }
}
