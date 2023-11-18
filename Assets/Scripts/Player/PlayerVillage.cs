using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVillage:MonoBehaviour
{
    Vector2 moveVec;
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    float hAxis;
    float vAxis;
    bool isRun = true;
    
    float speed=5;
    float runSpeed = 1;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

        
    }
    void getInput() 
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

    }

    void Update()
    {
        getInput();
        Move();
    }

    void Move()     //¿Ãµø
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        
        rigid.velocity = moveVec * speed * (isRun ? runSpeed : 1f);
        
        if (isRun && moveVec != Vector2.zero)
            sprite.color = Color.white;
        else
            sprite.color = Color.blue;

    }

}
