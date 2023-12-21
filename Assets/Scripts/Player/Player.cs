using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{


    float runCurrentCoolTime;        // 달리기 대기시간
    float attackDelay;        // 공격 대기시간

    float hAxis;
    float vAxis;

    bool rDown;             //달리기
    bool dDown;             //회피
    bool aDown;             //공격

    bool isRun = true;      //달리기
    bool isDodge;           //회피
    bool isAttack;   //공격

    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    PlayerStatus status;
    Weapon weapon;
    Attack attack;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();
        weapon = GetComponent<Weapon>();
        attack = GetComponentInChildren<Attack>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (status.isPlayerMove)
        {
            GetInput();
            Dodge();
            Move();
            Attack();
            Turn();
        }

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        dDown = Input.GetButtonDown("Dodge");
        aDown = Input.GetButton("Attack");
    }

    void Move()     //이동
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack)       // 공격시 정지
        {
            moveVec = Vector2.zero;
        }
        if (isDodge)             // 회피시 현재 속도 유지
        {
            moveVec = dodgeVec;
        }
        else
        {
            rigid.velocity = moveVec * status.speed * (isRun ? status.runSpeed : 1f);
            if (isRun && moveVec != Vector2.zero)
                sprite.color = Color.magenta;
            else 
                sprite.color = Color.blue;
        }
    }

    // 다듬을 것
    void Turn()
    {
        // 기본 상태
        if(moveVec == Vector2.zero)
            return;

        if(moveVec == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveVec == new Vector2(1,1).normalized)
        {
            transform.rotation = Quaternion.Euler(0, 0, 315);
        }
        else if (moveVec == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (moveVec == new Vector2(1, -1).normalized)
        {
            transform.rotation = Quaternion.Euler(0, 0, 225);
        }
        else if (moveVec == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (moveVec == new Vector2(-1, -1).normalized)
        {
            transform.rotation = Quaternion.Euler(0, 0, 135);
        }
        else if (moveVec == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (moveVec == new Vector2(-1, 1).normalized)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
    }

    void Attack()
    {
        if(weapon == null)
            return;

        attackDelay += Time.deltaTime;
        isAttack = (1/weapon.rate) > attackDelay;
        if (aDown && !isAttack && !isDodge)
        {
            RunDelay();
            Debug.Log("Attack");
            attack.Use();
            attackDelay = 0;
        }
    }

    void Dodge()    // 회피
    {
        if(dDown && !isAttack && moveVec != Vector2.zero && !isDodge)
        {
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            rigid.velocity = moveVec * status.speed * status.dodgeSpeed;
            isDodge = true;

            Invoke("DodgeOut", status.dodgeFrame);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        sprite.color = Color.blue;
        isDodge = false;
    }

    // 달리기 대기
    void RunDelay()
    {
        runCurrentCoolTime = status.runCoolTime;
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

    void OnTriggerEnter2D(Collider2D other) {
        // *임시* 아이템 획득
        if(other.tag == "Item")
        {
            Destroy(other.gameObject);
        }
        if (other.tag == "Weapon")
        {
            Debug.Log("Weapon");
            weapon = other.GetComponent<Weapon>();
            attack.GainWeapon(weapon);
            attackDelay = weapon.rate;
            other.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {

    }
}
