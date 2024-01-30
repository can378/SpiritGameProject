using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : Projectile
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" || other.tag == "Door" || other.tag == "ShabbyWall")
        {
            Destroy(gameObject);
        }
    }
}
