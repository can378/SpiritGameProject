using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float speed { get; private set; }

    public void SetBullet( float damage, float speed = 10)
    {
        this.speed = speed;
        this.damage = damage;
    }

}
