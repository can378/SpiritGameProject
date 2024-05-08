using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    Rigidbody2D rigid;
    public float angular;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.angularVelocity = angular;
        //rigid.AddTorque(90,ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
