using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : EnemyBasic
{
    private float ballSize;
    float targetDistance;
    void Start()
    {
        ballSize = transform.localScale.x;
    }

    
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, enemyTarget.position);
        if (targetDistance > GetComponent<EnemyStatus>().detectionDis)
        { roll(); }
        else 
        { }
    }

    private void roll()
    {
        //3�������
        //size
        transform.localScale += new Vector3(0.2f, 0.2f, 0);
        //rotation
        //ü��, ���ݷ� ����

        
    }
    private void snowballAttack() 
    { 
    
        //rotation and approach
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //ü��, ���ݷ� ���󺹱�
        
        }
    }
}
