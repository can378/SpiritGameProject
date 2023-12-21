using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{


    float runCurrentCoolTime;        // �޸��� ���ð�
    float attackDelay;        // ���� ���ð�

    float hAxis;
    float vAxis;

    bool rDown;             //�޸���
    bool dDown;             //ȸ��
    bool aDown;             //����

    bool isRun = true;      //�޸���
    bool isDodge;           //ȸ��
    bool isAttack;   //����

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

    void Move()     //�̵�
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack)       // ���ݽ� ����
        {
            moveVec = Vector2.zero;
        }
        if (isDodge)             // ȸ�ǽ� ���� �ӵ� ����
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

    // �ٵ��� ��
    void Turn()
    {
        // �⺻ ����
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

    void Dodge()    // ȸ��
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

    void DodgeOut() // ȸ�� ����������
    {
        sprite.color = Color.blue;
        isDodge = false;
    }

    // �޸��� ���
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
        // *�ӽ�* ������ ȹ��
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
