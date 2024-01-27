using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    float runCurrentCoolTime;        // 달리기 대기시간
    float attackDelay;        // 공격 대기시간

    Vector2 mousePos;
    public Vector2 mouseDir;
    public float mouseAngle;

    float hAxis;
    float vAxis;

    bool rDown;             //재장전
    bool dDown;             //회피
    bool aDown;             //공격
    bool iDown;             //상호작용

    [SerializeField] bool isReload = false;               //장전
    [SerializeField] bool isSprint = true;                //달리기
    [SerializeField] bool isDodge = false;                //회피
    [SerializeField] bool isAttack = false;               //공격
    [SerializeField] bool isAttackReady = false;          //공격 준비 완료
    [SerializeField] bool isEquip = false;                //무기 장비
    [SerializeField] bool isInvincible = false;          //무적 상태

    public LayerMask layerMask;//접근 불가한 레이어 설정
    public GameObject nearObject;
    public GameObject playerItem;

    Vector2 playerPosition;
    
    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    PlayerStatus status;
    Attack attack;

    GameObject weaponGameObject;
    Weapon weapon;


    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();
        attack = GetComponentInChildren<Attack>();
    }

    void Start()
    {
        int layerNum = LayerMask.NameToLayer("Default");
        this.layerMask = layerNum;
    }

    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        GetInput();

        Turn();
        
        if (isMoveable())
        {
            Dodge();
            Move();  
        }

        UseItem();
        Attack();
        Reload();
        Interaction();

        string layerName = LayerMask.LayerToName(gameObject.layer);
        //Debug.Log("My layer name is: " + layerName);
    }
    
    void FixedUpdate()
    {

    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Reload");
        dDown = Input.GetButtonDown("Dodge");
        aDown = Input.GetButton("Attack");
        iDown = Input.GetButtonDown("Interaction");//f
    }

    #region Moving
    

    private bool isMoveable() 
    {
        // 레이저가 제대로 도착하면 Null, 막혔을때 방해물 Return
        RaycastHit2D hit;

        playerPosition = transform.position;
        Vector2 end= 
            playerPosition + 
            new Vector2(playerPosition.x * status.speed, playerPosition.y * status.speed);
        

        // 레이저 발사 (시작, 끝, 레이어마스크)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);


        // 벽으로 막혔을때 실행하지 않게 처리
        if (hit.transform == null) {  return true;   }
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

        }
    }

    void Turn()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = (mousePos - (Vector2)transform.position).normalized;

        mouseAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(mouseAngle - 90, Vector3.forward);

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

    #endregion

    #region Attack

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
        weapon.Reload();
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

    #endregion

    void Interaction()
    {
        if (iDown && nearObject != null && !isEquip && !isDodge && !isAttack && moveVec == Vector2.zero)
        {
            if (nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
        }
        if (iDown && nearObject != null && !isEquip && !isDodge && !isAttack && moveVec == Vector2.zero)
        {
            if (nearObject.tag == "Door")
            {
                nearObject.GetComponent<Door>().DoorInteraction();
            }
        }
    }

    void GainSelectItem()
    {
        SelectItem selectItem = nearObject.GetComponent<SelectItem>();
        if (selectItem.selectItemClass == SelectItemClass.Weapon)
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
            DataManager.instance.userData.Weapon = weapon.name.ToString();

            MapUIManager.instance.UpdateWeaponUI();
            attack.EquipWeapon(weapon);
            attackDelay = 0;
            weaponGameObject.SetActive(false);
        }
        else if
            (
            selectItem.selectItemClass == SelectItemClass.Consumable&&
            selectItem.GetComponent<ItemStatus>().obtainable
            )
        {
            //전에 가지고 있던 아이템 드랍
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //아이템 갱신
            //DataManager.instance.userData.playerItem = selectItem.name;
            playerItem = selectItem.gameObject;
            MapUIManager.instance.updateItemUI(selectItem.gameObject);
            playerItem.SetActive(false);
            //Destroy(selectItem.gameObject);
        }
    }

    void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.H) && playerItem != null)
        {
            MapUIManager.instance.updateItemUI(null);
            

            switch (playerItem.GetComponent<ItemStatus>().itemName)
            {
                case "bomb":
                case "Item":
                    StartCoroutine(attack.ThrowWeapon(playerItem));
                    break;
                case "HPPortion":
                    DataManager.instance.userData.playerHP += 10;
                    MapUIManager.instance.UpdateHealthUI();
                    Destroy(playerItem);
                    break;
                default: print("wrong item process"+ playerItem.GetComponent<ItemStatus>().itemName); break;
            }
            playerItem = null;


        }


    }


    #region Trigger


    void OnTriggerEnter2D(Collider2D other)
    {
        //공격받음
        //Enter로 하는게 나을 듯?
        if (other.tag == "Enemy" || other.tag == "EnemyAttack")
        {
            if (DataManager.instance.userData.playerHP < 0)
            {
                Debug.Log("player dead");
                DataManager.instance.InitData();
                MapUIManager.instance.diePanel.SetActive(true);
            }
            else if (isInvincible == false)
            {

                //DataManager.instance.userData.playerHP -= other.GetComponent<EnemyStatus>().damage;
                DataManager.instance.userData.playerHP -= 10;
                MapUIManager.instance.UpdateHealthUI();


                //무적
                isInvincible = true;
                int layerNum = LayerMask.NameToLayer("Invincible");
                this.layerMask = layerNum;
                sprite.color = new Color(1, 1, 1, 0.4f);


                //튕겨나감
                Vector2 dir = (transform.position - other.transform.position).normalized;
                rigid.AddForce(dir * 50f, ForceMode2D.Impulse);


                Invoke("OffDamaged", 0.2f);

            }

        }
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SelectItem" || other.tag == "Door")
        {
            nearObject = other.gameObject;
        }

    }
    void OffDamaged()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        this.layerMask = 0;
        isInvincible = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    #endregion
}
