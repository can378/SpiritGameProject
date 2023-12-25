using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    float runCurrentCoolTime;        // 달리기 대기시간
    public float attackDelay;        // 공격 대기시간

    float hAxis;
    float vAxis;

    bool rDown;             //재장전
    bool dDown;             //회피
    bool aDown;             //공격
    bool iDown;             //상호작용

    public bool isReload = false;
    public bool isSprint = true;              //달리기
    public bool isDodge = false;              //회피
    public bool isAttack = false;           //공격
    public bool isAttackReady = false;
    public bool isEquip = false;           //무기 장비

    public LayerMask layerMask;//접근 불가한 레이어 설정
    private Collider2D collider;
    private Vector2 playerPosition;

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
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        GetInput();
        if (isMoveable())
        {
            Dodge();
            Move();  
        }
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

    private bool isMoveable() 
    {
        // 레이저가 제대로 도착했을때 Null, 막혔을때 방해물이 Return
        RaycastHit2D hit;

        playerPosition = transform.position;
        Vector2 end= playerPosition + new Vector2(playerPosition.x * status.speed, playerPosition.y * status.speed);
        
        // 캐릭터에 BoxCollider가 적용되어 있어 그걸 충돌체로 인식하므로 해제 후 설정
        collider.enabled = false;
        
        // 레이저 발사 (시작, 끝, 레이어마스크)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);
        collider.enabled = true;

        // 벽으로 막혔을때 실행하지 않게 처리
        if (hit.transform == null)
        {  return true;   }
        return false;
    }

    void Move()     //이동
    {
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack || isReload)       // 공격시 정지
        {
            moveVec = Vector2.zero;
        }
        if (isDodge)             // 회피시 현재 속도 유지
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

    // 다듬을 것
    void Turn()
    {
        // 기본 상태
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

    void Dodge()    // 회피
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

    void DodgeOut() // 회피 빠져나가기
    {
        sprite.color = Color.blue;
        isDodge = false;
    }

    // 달리기 대기
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
        // *임시* 아이템 획득
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
