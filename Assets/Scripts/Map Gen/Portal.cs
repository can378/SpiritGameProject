using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    public Transform Destination;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            other.transform.position = Destination.position;
        }
    }
}
