using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType {Bullet, Bomb, Arrow, Rotor}

public class Projectile : HitDetection
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || other.gameObject.tag == "ShabbyWall")
        {
            Destroy(gameObject);
        }
    }

}
