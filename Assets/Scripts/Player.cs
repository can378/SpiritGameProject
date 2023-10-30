using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;             // 10
    public float runSpeed;          // 1.33f
    public float dodgeSpeed;        // 2
    public float dodgeFrame;        // 0.5f

    float hAxis;
    float vAxis;

    bool rDown;            //달리기
    public bool dDown;     //회피

    bool isRun;     //달리기
    bool isDodge;   //회피

    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        Dodge();
        Move();

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        dDown = Input.GetButtonDown("Dodge");
    }

    void Move()     //이동
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if(isDodge)
        {
            moveVec = dodgeVec;
        }
        else
        {
            rigid.velocity = moveVec * speed * (rDown ? runSpeed : 1f);
            if (rDown && moveVec != Vector2.zero)
                sprite.color = Color.magenta;
            else
                sprite.color = Color.blue;
        }
   
    }

    void Dodge()    // 회피
    {
        if(dDown && moveVec != Vector2.zero && !isDodge)
        {
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            rigid.velocity = moveVec * speed *dodgeSpeed;
            isDodge = true;

            Invoke("DodgeOut",dodgeFrame);

        }
    }

    void DodgeOut() // 회피 나가기
    {
        sprite.color = Color.blue;
        isDodge = false;
    }

    void FixedUpdate() {

    }
}
