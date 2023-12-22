using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    float runCurrentCoolTime;        // �޸��� ���ð�
    public float attackDelay;        // ���� ���ð�

    float hAxis;
    float vAxis;

    bool rDown;             //������
    bool dDown;             //ȸ��
    bool aDown;             //����
    bool iDown;             //��ȣ�ۿ�

    public bool isReload = false;
    public bool isSprint = true;              //�޸���
    public bool isDodge = false;              //ȸ��
    public bool isAttack = false;           //����
    public bool isAttackReady = false;
    public bool isEquip = false;           //���� ���

    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    PlayerStatus status;
    Attack attack;


    GameObject nearObject;

    GameObject weaponGameObject;
    Weapon weapon;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();
        attack = GetComponentInChildren<Attack>();
    }

    void Start()
    {

    }

    void Update()
    {
        GetInput();
        Dodge();
        Move();
        Attack();
        Reload();
        Turn();
        Interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Reload");
        dDown = Input.GetButtonDown("Dodge");
        aDown = Input.GetButton("Attack");
        iDown = Input.GetButtonDown("Interaction");
    }

    void Move()     //�̵�
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack || isReload)       // ���ݽ� ����
        {
            moveVec = Vector2.zero;
        }
        if (isDodge)             // ȸ�ǽ� ���� �ӵ� ����
        {
            moveVec = dodgeVec;
        }
        else
        {
            rigid.velocity = moveVec * status.speed * (isSprint ? status.runSpeed : 1f);
            if (isSprint && moveVec != Vector2.zero)
                sprite.color = Color.magenta;
            else
                sprite.color = Color.blue;
        }
    }

    // �ٵ��� ��
    void Turn()
    {
        // �⺻ ����
        if (moveVec == Vector2.zero)
            return;

        if (moveVec == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveVec == new Vector2(1, 1).normalized)
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

    void Reload()
    {
        if (weaponGameObject == null || weapon == null)
            return;

        if (weapon.weaponType != WeaponType.Shot)
            return;

        if (weapon.maxAmmo == weapon.ammo)
            return;

        if (rDown && !isDodge && !isReload && !isEquip && !isAttack)
        {
            isReload = true;
            Invoke("ReloadOut", weapon.reloadTime);
        }
    }

    void ReloadOut()
    {
        weapon.ammo = weapon.maxAmmo;
        isReload = false;
    }

    void Attack()
    {
        if (weapon == null && weaponGameObject == null)
            return;

        if (weapon.ammo == 0)
            return;

        attackDelay -= Time.deltaTime;
        isAttackReady = attackDelay <= 0;

        if (aDown && !isAttack && !isDodge && isAttackReady)
        {
            isAttack = true;

            RunDelay();
            attack.Use();
            attackDelay = weapon.delay;
            isAttackReady = false;
            Invoke("AttackOut", 1 / weapon.rate);

        }
    }

    void AttackOut()
    {
        isAttack = false;
    }

    void Dodge()    // ȸ��
    {
        if (dDown && !isAttack && moveVec != Vector2.zero && !isDodge)
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
        if (isSprint == true)
        {
            isSprint = false;
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
        isSprint = true;
    }

    void Interaction()
    {
        if (iDown && nearObject != null && !isEquip && !isDodge && !isAttack && moveVec == Vector2.zero)
        {
            if (nearObject.tag == "Weapon")
            {
                if (weaponGameObject != null)
                {
                    attack.UnEquipWeapon();
                    weaponGameObject.SetActive(true);
                    weaponGameObject.transform.position = transform.position;
                    weaponGameObject = null;
                }
                weaponGameObject = nearObject;
                weapon = weaponGameObject.GetComponent<Weapon>();
                attack.EquipWeapon(weapon);
                attackDelay = 0;
                weaponGameObject.SetActive(false);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // *�ӽ�* ������ ȹ��
        if (other.tag == "Item")
        {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log(other.name);
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    void FixedUpdate()
    {

    }
}
