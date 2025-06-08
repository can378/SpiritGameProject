using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CenterPivot : MonoBehaviour
{
    public Transform Center;
    public Vector3 WorldPos;

    void Update()
    {
        if (Center != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y + Center.localPosition.y) / 1000);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 1000);
        }
        WorldPos = transform.position;

    }
}
