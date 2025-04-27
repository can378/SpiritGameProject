using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PivotOrder : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y/1000);
        
    }
}
