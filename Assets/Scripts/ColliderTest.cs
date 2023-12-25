using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{

    public GameObject playerTest;
    private Rigidbody2D rg;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") 
        {

            print("test and player are attached-collider");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            print("test and player are attached-trigger");
        }
    }

}
