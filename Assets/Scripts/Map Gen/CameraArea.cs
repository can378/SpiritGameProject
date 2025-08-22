using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    CameraManager cam;
    [SerializeField]
    Transform OtherTransform;

    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == true)
        {
            if (OtherTransform != null)
            {
                cam.postCenter = OtherTransform.position;
            }
            else
            {
                cam.postCenter = transform.position;
            }
        }
    }
}
