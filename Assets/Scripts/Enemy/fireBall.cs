using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBall : EnemyBasic
{
    public int damage;
    GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D)//변경
        {
            //데미지 넣기
        
        }
    }
    void Chase()
    {
        Vector2 direction = Target.transform.position - transform.position;
        transform.Translate(direction * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            this.gameObject.SetActive(false);
            print("fireball이 player공격");
        
        
        }
    }
}
