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
    [SerializeField] bool isInvincible = false;           //무적 상태
    public bool isAttackable = true;            //공격가능 상태


    public LayerMask layerMask;//접근 불가한 레이어 설정
    public GameObject nearObject;
    public GameObject playerItem;

    [SerializeField] GameObject[] meleeWeaponList;

    Vector2 playerPosition;
    
    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;

    PlayerStatus status;
    Attack attack;

    MainWeapon mainWeapon;

    UserData userData;


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
        
        if (isAttackable)
        {
            Attack();
            Reload();
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
        if (mainWeapon == null)
            return;

        if (mainWeapon.weaponType != WeaponType.Shot)
            return;

        if (mainWeapon.maxAmmo == mainWeapon.ammo)
            return;

        if (rDown && !isDodge && !isReload && !isEquip && !isAttack)
        {
            isReload = true;
            Invoke("ReloadOut", mainWeapon.reloadTime);
        }
    }

    void ReloadOut()
    {
        mainWeapon.Reload();
        isReload = false;
    }

    void Attack()
    {
        if (mainWeapon == null)
            return;

        if (mainWeapon.ammo == 0)
            return;

        attackDelay -= Time.deltaTime;
        isAttackReady = attackDelay <= 0;

        if (aDown && !isAttack && !isDodge && isAttackReady)
        {
            isAttack = true;

            RunDelay();
            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            Use();
            attackDelay = (mainWeapon.preDelay + mainWeapon.rate + mainWeapon.postDelay) / mainWeapon.attackSpeed;
            isAttackReady = false;
            Invoke("AttackOut", (mainWeapon.preDelay + mainWeapon.rate) / mainWeapon.attackSpeed);

        }
    }

    void AttackOut()
    {
        isAttack = false;
    }

    void Use()
    {
        if (mainWeapon.weaponType == WeaponType.Melee)
        {
            // 플레이어 애니메이션 실행
            StartCoroutine("Swing");
        }
        else if (mainWeapon.weaponType == WeaponType.Shot)
        {
            // 플레이어 애니메이션 실행
            mainWeapon.ConsumeAmmo();
            StartCoroutine("Shot");
        }

    }

    // 이펙트 생성
    IEnumerator Swing()
    {
        Debug.Log("Swing");

        yield return new WaitForSeconds(mainWeapon.preDelay / mainWeapon.attackSpeed);

        // 무기 이펙트 유형 설정
        MeleeWeapon meleeWeapon = mainWeapon.GetComponent<MeleeWeapon>();
        GameObject mainWeaponGameObject = meleeWeaponList[meleeWeapon.attackType];
        mainWeaponGameObject.transform.localScale = new Vector3(meleeWeapon.weaponSize, meleeWeapon.weaponSize, 1);

        // 이펙트 수치 설정
        HitDetection hitDetection = mainWeaponGameObject.GetComponentInChildren<HitDetection>();
        hitDetection.SetHitDetection(mainWeapon.weaponAttribute, mainWeapon.damage * status.playerPower, mainWeapon.knockBack, status.playerCritical, status.playerCriticalDamage);
        
        // 무기 방향 
        mainWeaponGameObject.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);
        
        // 무기 이펙트 실행
        mainWeaponGameObject.SetActive(true);

        yield return new WaitForSeconds(mainWeapon.rate / mainWeapon.attackSpeed);

        mainWeaponGameObject.SetActive(false);

    }

    // 무기 투사체 발사
    IEnumerator Shot()
    {
        Debug.Log("Shot");

        yield return new WaitForSeconds(mainWeapon.preDelay / mainWeapon.attackSpeed);

        // 무기 투사체 적용
        ShotWeapon shotWeapon = mainWeapon.GetComponent<ShotWeapon>();
        GameObject instantProjectile = Instantiate(shotWeapon.projectile, transform.position, transform.rotation);

        //투사체 설정
        Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
        HitDetection projectile = instantProjectile.GetComponent<HitDetection>();


        //bulletRigid.velocity = shotPos.up * 25;
        // 투사체 설정
        projectile.SetHitDetection(shotWeapon.weaponAttribute, shotWeapon.damage * status.playerPower, shotWeapon.knockBack, status.playerCritical, status.playerCriticalDamage); //기본 설정
        instantProjectile.transform.rotation = Quaternion.AngleAxis(Player.instance.mouseAngle - 90, Vector3.forward);  // 방향 설정
        instantProjectile.transform.localScale = new Vector3(shotWeapon.projectileSize, shotWeapon.projectileSize, 1);  // 크기 설정
        bulletRigid.velocity = Player.instance.mouseDir * 25 * shotWeapon.projectileSpeed;  // 속도 설정
        Destroy(instantProjectile, shotWeapon.projectileTime);  //사거리 설정

        yield return new WaitForSeconds(mainWeapon.postDelay / mainWeapon.attackSpeed);

        yield return null;
    }
    public IEnumerator ThrowWeapon(GameObject explosive)
    {

        yield return new WaitForSeconds(0.1f);

        explosive.tag = "Weapon";
        explosive.SetActive(true);
        explosive.transform.position = transform.position;
        explosive.GetComponent<Rigidbody2D>().velocity = Player.instance.mouseDir * 25;

        float originalRadius = explosive.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < originalRadius * 1.5; i++)
        {
            explosive.GetComponent<CircleCollider2D>().radius++;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(explosive);
        yield return null;

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
            if (mainWeapon != null)
            {
                mainWeapon.gameObject.SetActive(true);
                mainWeapon.transform.position = transform.position;
                mainWeapon = null;
            }
            mainWeapon = nearObject.GetComponent<MainWeapon>();

            attackDelay = 0;
            mainWeapon.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.H) && playerItem != null)
        {
            //Throwing Items
            if (playerItem.GetComponent<SelectItem>().selectItemClass == SelectItemClass.ThrowWeapon)
            { StartCoroutine(ThrowWeapon(playerItem)); }
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        //공격받음
        //Enter로 하는게 나을 듯?
        if (other.tag == "Enemy" || other.tag == "EnemyAttack")
        {
            if (userData.playerHP < 0)
            {
                Debug.Log("player dead");
                DataManager.instance.InitData();
                DataManager.instance.SaveUserData();
                MapUIManager.instance.diePanel.SetActive(true);
            }
            else if (isInvincible == false)
            {
                Debug.Log("player Damaged");
                //userData.playerHP -= other.GetComponent<EnemyStatus>().damage;
                userData.playerHP -= 10;
                MapUIManager.instance.UpdateHealthUI();


                //무적
                isInvincible = true;
                int layerNum = LayerMask.NameToLayer("Invincible");
                this.layerMask = layerNum;
                sprite.color = new Color(1, 1, 1, 0.4f);


                //튕겨나감
                Vector2 dir = (transform.position - other.transform.position).normalized;
                rigid.AddForce(dir * 10f, ForceMode2D.Impulse);


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
        isInvincible = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    #endregion
}
