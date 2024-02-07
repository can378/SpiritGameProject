using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    Vector2 mousePos;
    public Vector2 mouseDir;
    public float mouseAngle;

    float hAxis;
    float vAxis;

    bool rDown;             //재장전
    bool dDown;             //회피
    bool aDown;             //공격
    bool sDown;             // 보조무기 누른 상태
    bool sUp;               // 보조무기 뗀 상태
    bool siDown;            // 선택 아이템
    bool iDown;             //상호작용

    public LayerMask layerMask;//접근 불가한 레이어 설정
    public GameObject nearObject;
    public GameObject playerItem;

    Vector2 playerPosition;
    
    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    public PlayerStatus status { get; private set;}
    MainWeaponController mainWeaponController;
    SubWeaponController subWeaponController;
    

    UserData userData;


    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();
        mainWeaponController = GetComponent<MainWeaponController>();
        subWeaponController = GetComponent<SubWeaponController>();
    }

    void Start()
    {
        userData = DataManager.instance.userData;
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
        
        if (status.isAttackable)
        {
            Attack();
            Reload();
            UseSubWeapon();
            GuardOut();
        }
        
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
        iDown = Input.GetButtonDown("Interaction"); //f
        siDown = Input.GetButtonDown("SelectItem"); //h

        sDown = Input.GetButtonDown("SubWeapon");   //마우스 우클릭
        sUp = Input.GetButtonUp("SubWeapon");
        
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

        if (status.isAttack || status.isReload || status.isParry)       // 정지
        {
            moveVec = Vector2.zero;
        }
        if (status.isDodge)             // 회피시 현재 속도 유지
        {
            moveVec = dodgeVec;
        }
        else if(status.isGuard)
        {
            rigid.velocity = moveVec * status.speed * subWeaponController.subWeapon.ratio;    //임시
        }
        else
        {
            rigid.velocity = moveVec * status.speed * (status.isSprint ? status.runSpeed : 1f);
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
        if (dDown && !status.isAttack && moveVec != Vector2.zero && !status.isDodge)
        {
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            rigid.velocity = moveVec * status.speed * status.dodgeSpeed;
            status.isDodge = true;

            Invoke("DodgeOut", status.dodgeFrame);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        status.isDodge = false;
    }

    

    // 달리기 대기
    void RunDelay()
    {
        status.runCurrentCoolTime = status.runCoolTime;
        if (status.isSprint == true)
        {
            status.isSprint = false;
            StartCoroutine(RunCoolTime());
        }
    }

    IEnumerator RunCoolTime()
    {
        while (status.runCurrentCoolTime > 0.0f)
        {
            status.runCurrentCoolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        status.isSprint = true;
    }

    #endregion

    #region Attack

    void Reload()
    {
        if (mainWeaponController.mainWeapon == null)
            return;

        if (mainWeaponController.mainWeapon.maxAmmo < 0)
            return;

        if (mainWeaponController.mainWeapon.maxAmmo == mainWeaponController.mainWeapon.ammo)
            return;

        if (rDown && !status.isDodge && !status.isReload && !status.isEquip && !status.isAttack)
        {
            status.isReload = true;
            Invoke("ReloadOut", mainWeaponController.mainWeapon.reloadTime);
        }
    }

    void ReloadOut()
    {
        mainWeaponController.mainWeapon.Reload();
        status.isReload = false;
    }

    void Attack()
    {
        if (mainWeaponController.mainWeapon == null)
            return;

        if (mainWeaponController.mainWeapon.ammo == 0)
            return;

        status.attackDelay -= Time.deltaTime;
        status.isAttackReady = status.attackDelay <= 0;

        if (aDown && !status.isAttack && !status.isDodge && status.isAttackReady)
        {
            status.isAttack = true;

            RunDelay();
            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            mainWeaponController.Use(mousePos);
            status.attackDelay = (mainWeaponController.mainWeapon.preDelay + mainWeaponController.mainWeapon.rate + mainWeaponController.mainWeapon.postDelay) / mainWeaponController.mainWeapon.attackSpeed;
            status.isAttackReady = false;
            Invoke("AttackOut", (mainWeaponController.mainWeapon.preDelay + mainWeaponController.mainWeapon.rate) / mainWeaponController.mainWeapon.attackSpeed);

        }
    }

    void AttackOut()
    {
        status.isAttack = false;
    }
    
    #endregion

    #region SubWeapon
    void UseSubWeapon()
    {
        if (subWeaponController.subWeapon == null)
            return;

        status.subWeaponDelay -= Time.deltaTime;
        status.isSubWeaponReady = status.subWeaponDelay <= 0;

        if (sDown && !status.isSubWeapon && !status.isDodge && status.isSubWeaponReady)
        {
            status.isSubWeapon = true;
            RunDelay();
            SubWeaponUse();
            status.isSubWeaponReady = false;
        }
    }

    void SubWeaponUse()
    {
        if (subWeaponController.subWeapon.subWeaponType == SubWeaponType.Guard)
        {
            Debug.Log("막기");
            status.isGuard = true;
        }
        else if (subWeaponController.subWeapon.subWeaponType == SubWeaponType.Parry)
        {
            Debug.Log("반격");
            status.isParry = true;
            status.subWeaponDelay = (subWeaponController.subWeapon.preDelay + subWeaponController.subWeapon.rate + subWeaponController.subWeapon.coolTime);
            Invoke("ParryOut", subWeaponController.subWeapon.preDelay + subWeaponController.subWeapon.rate);
        }
        else if (subWeaponController.subWeapon.subWeaponType == SubWeaponType.Teleport)
        {
            Debug.Log("순간이동");
            status.subWeaponDelay = (subWeaponController.subWeapon.preDelay + subWeaponController.subWeapon.rate + subWeaponController.subWeapon.coolTime);
            Invoke("TeleportOut", subWeaponController.subWeapon.preDelay + subWeaponController.subWeapon.rate);
        }

    }

    void ParryOut()
    {
        status.isSubWeapon = false;
        status.isParry = false;
    }

    void TeleportOut()
    {
        gameObject.transform.position = mousePos;
        status.isSubWeapon = false;
    }

    void GuardOut()
    {
        if (sUp && status.isGuard && subWeaponController.subWeapon.subWeaponType == SubWeaponType.Guard)
        {
            status.isSubWeapon = false;
            status.isGuard = false;
            status.subWeaponDelay = subWeaponController.subWeapon.coolTime;
        }
    }

    #endregion SubWeapon

    void Interaction()
    {
        if (iDown && nearObject != null && !status.isEquip && !status.isDodge && !status.isAttack && moveVec == Vector2.zero
        && !status.isAttack && !status.isSubWeapon)
        {
            if (nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
        }
        if (iDown && nearObject != null && !status.isEquip && !status.isDodge && !status.isAttack && moveVec == Vector2.zero)
        {
            if (nearObject.tag == "Door")
            {
                nearObject.GetComponent<Door>().DoorInteraction();
            }
            else if (nearObject.tag == "ShabbyWall")
            {
                nearObject.GetComponent<Wall>().WallInteraction();
            }
        }
    }

    #region Item

    void GainSelectItem()
    {
        SelectItem selectItem = nearObject.GetComponent<SelectItem>();
        if (selectItem.selectItemClass == SelectItemClass.Weapon)
        {
            if (mainWeaponController.mainWeapon != null)
            {
                // 무기의 위치를 현재 위치로 옮긴 후 해체
                mainWeaponController.mainWeapon.gameObject.transform.position = transform.position;
                mainWeaponController.UnEquipWeapon();
            }
            // 무기 장비
            mainWeaponController.EquipWeapon(nearObject.GetComponent<MainWeapon>());
        }
        else if (selectItem.selectItemClass == SelectItemClass.SubWeapon)
        {
            Debug.Log("보조무기 사용법");
            Debug.Log("막기: 우클릭 동안 유지 - 데미지 감소");
            Debug.Log("반격 : 우클릭 시 raio초 동안 발동 - 적 스턴");
            Debug.Log("순간이동 : 우클릭 시 선딜레이 이후 클릭한 방향으로 이동");
            if (subWeaponController.subWeapon != null)
            {
                // 무기의 위치를 현재 위치로 옮긴 후 해체
                subWeaponController.subWeapon.gameObject.transform.position = transform.position;
                subWeaponController.UnEquipWeapon();
            }
            // 무기 장비
            subWeaponController.EquipSubWeapon(nearObject.GetComponent<SubWeapon>());
        }
        else if
        (   
            selectItem.selectItemClass == SelectItemClass.Consumable || 
            selectItem.selectItemClass==SelectItemClass.ThrowWeapon  
        )
        {
            //전에 가지고 있던 아이템 드랍
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //아이템 갱신
            DataManager.instance.userData.playerItem = selectItem.GetComponent<ItemInfo>().selectItemName.ToString();
            playerItem = selectItem.gameObject;
            MapUIManager.instance.updateItemUI(selectItem.gameObject);
            playerItem.SetActive(false);
        }
    }

    void UseItem()
    {
        if (siDown && playerItem != null)
        {
            Debug.Log("UseSelectItem");
            //Throwing Items
            if (playerItem.GetComponent<SelectItem>().selectItemClass == SelectItemClass.ThrowWeapon)
            { mainWeaponController.UseItem(playerItem, mousePos); }
            //Consumable Item
            else 
            {
                switch (playerItem.GetComponent<ItemInfo>().selectItemName)
                {
                    case SelectItemName.HPPortion:
                        userData.playerHP += 10;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.SpeedPortion:
                        break;
                    case SelectItemName.SkillPortion:
                        break;
                    // 밑에 아이템들은 획득 즉시로 바꾸었으면 좋겠습니다.
                    case SelectItemName.Insam:
                        userData.playerHP += 20;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.Sansam:
                        userData.playerHP += 30;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.SmallArmor:
                        userData.playerTempHP += 10;
                        break;
                    case SelectItemName.LargeArmor:
                        userData.playerTempHP += 20;
                        break;
                    case SelectItemName.NormalArmor:
                        userData.playerTempHP += 30;
                        break;


                    default:
                        Debug.LogWarning("no information item process" + playerItem.GetComponent<ItemInfo>().selectItemName);
                        break;
                }
                Destroy(playerItem);
            }
           
            
            //"no item" status
            MapUIManager.instance.updateItemUI(null);
            playerItem = null;
            DataManager.instance.userData.playerItem = "";

        }

    }
    #endregion

    #region SceneReload - item
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene reloaded: " + scene.name);
        //Scene reload 후에도 전에 얻은 아이템 유지
        string playerItemName = DataManager.instance.userData.playerItem;

        if (scene.name != "Main"&& playerItemName != "")
        {
            
            foreach (GameObject obj in DataManager.instance.gameData.selectItemList)
            { 
                if (obj.GetComponent<ItemInfo>().selectItemName.ToString() == playerItemName)
                {
                    playerItem = Instantiate(obj);
                    MapUIManager.instance.updateItemUI(playerItem.gameObject);
                    playerItem.SetActive(false);
                    break;
                }
            }

        
        }

    }
    #endregion

    #region Trigger


    void OnTriggerEnter2D(Collider2D other)
    {
        //공격받음
        if (other.tag == "Enemy" || other.tag == "EnemyAttack")
        {

            if (status.isInvincible == false)
            {
                Debug.Log("player Damaged");
                //userData.playerHP -= other.GetComponent<EnemyStatus>().damage;


                if(status.isGuard)
                {
                    Debug.Log("막기 성공");
                    userData.playerHP -= (10 - (10 * subWeaponController.subWeapon.ratio));
                    MapUIManager.instance.UpdateHealthUI();
                }
                else if(status.isParry)
                {
                    //적 스턴
                    Debug.Log("패링 성공");
                    return;
                }
                if (userData.playerHP < 0)
                {
                    Debug.Log("player dead");
                    DataManager.instance.InitData();
                    DataManager.instance.SaveUserData();
                    MapUIManager.instance.diePanel.SetActive(true);
                    return;
                }

                //무적
                status.isInvincible = true;
                int layerNum = LayerMask.NameToLayer("Invincible");
                this.layerMask = layerNum;
                sprite.color = new Color(1, 1, 1, 0.4f);


                //튕겨나감
                Vector2 dir = (transform.position - other.transform.position).normalized;
                rigid.AddForce(dir * (10 - (10 * subWeaponController.subWeapon.ratio)), ForceMode2D.Impulse);


                Invoke("OffDamaged", 0.2f);

            }

            

        }
        else if (other.tag == "EnterDungeon")
        {
            if (userData.nowChapter < 4)
            {
                
                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("Map");
            }
            else if (userData.nowChapter == 4)
            {
                
                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("FinalMap"); 
            }
            else if(userData.nowChapter==5) 
            {
                DataManager.instance.InitData();
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("Main"); 
            }
        }
        else if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            if (item.itemClass == ItemClass.Coin)
            {
                Destroy(other.gameObject); //코인 오브젝트 삭제
                userData.coin++;
                MapUIManager.instance.UpdateCoinUI();
            }

            if (item.itemClass == ItemClass.Key)
            {
                Destroy(other.gameObject); //키 오브젝트 삭제
                userData.key++;
                MapUIManager.instance.UpdateKeyUI();
            }
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SelectItem" || other.tag == "Door" || other.tag == "ShabbyWall")
        {
            nearObject = other.gameObject;
        }

    }
    
    void OffDamaged()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        this.layerMask = 0;
        status.isInvincible = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    #endregion
}
