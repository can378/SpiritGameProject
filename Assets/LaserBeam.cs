using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float laserLength;
    public GameObject startPos;
    //public Vector3 startPos;
    private LineRenderer lineRend;
    // Start is called before the first frame update
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 endPos = startPos.transform.position + (transform.up * laserLength);
        lineRend.SetPositions(new Vector3[] { startPos.transform.position, endPos });
        
    }
}
