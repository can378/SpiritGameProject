using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;                     // 이동속도 기본 10
    public float runSpeed;                  // 달리기 속도 기본 1.33f
    public float dodgeSpeed;                // 회피 기본 2
    public float dodgeFrame;                // 회피 시간 기본 0.5f
    public float runCoolTime;               // 달리기 최대 대기 시간 기본 10
    public float runCurrentCoolTime;        // 달리기 쿨타임

    float hAxis;
    float vAxis;

    bool rDown;            //달리기
    bool dDown;     //회피

    bool isRun = true;     //달리기
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
            rigid.velocity = moveVec * speed * (isRun ? runSpeed : 1f);
            if (isRun && moveVec != Vector2.zero)
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

            Run();
            Invoke("DodgeOut",dodgeFrame);

        }
    }

    void DodgeOut() // 회피 나가기
    {
        sprite.color = Color.blue;
        isDodge = false;
    }

    // 다음 달리기까지 대기
    void Run()
    {
        runCurrentCoolTime = runCoolTime;
        if (isRun == true)
        {
            isRun = false;
            StartCoroutine(RunCoolTime());
        }
    }

    IEnumerator RunCoolTime()
    {
        while (runCurrentCoolTime > 0.0f)
        {
            runCurrentCoolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isRun = true;
    }

    void FixedUpdate() {

    }
}
