using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{

    public GameObject Inside;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            if (this.gameObject.name == "Exit") { Inside.SetActive(false); }
            else { Inside.SetActive(true); }
            print("player collision");
        }
    }
}
