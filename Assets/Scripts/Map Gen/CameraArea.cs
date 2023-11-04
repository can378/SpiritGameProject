using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    CameraManager cam;

    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == true)
        {
            Debug.Log(name);
            cam.postCenter = transform.position;
        }
    }
}
