using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float time=0;
    private float speed=2;
    Transform playerPos;

    void Awake()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        //GetComponent<Rigidbody2D>().AddForce((playerPos.position - transform.position) * Time.deltaTime * 10000);
    }
    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 5f) { this.gameObject.SetActive(false); time = 0; }

        Vector2 direction = transform.position- playerPos.position;
        transform.Translate(direction * speed * Time.deltaTime);    
    }

}
