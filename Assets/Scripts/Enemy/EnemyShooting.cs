using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float attackRate = 2f;
    private Transform target;
    private float timeAfterAttack;

    void Start()
    {
        timeAfterAttack = 0f;
        target = GameObject.Find("Player").transform;

    }

    void Update()
    {
        timeAfterAttack += Time.deltaTime;

        if (timeAfterAttack >= attackRate) 
        {
            timeAfterAttack = 0f;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        
        
        }
        
    }

    
    
}
