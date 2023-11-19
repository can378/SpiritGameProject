using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody;
    Transform playerPos;
    Vector2 dir;


    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        dir = playerPos.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * Time.deltaTime * 10000);

        Destroy(gameObject, 5f);
    
    }

    void Update()
    {
        
    }

}
