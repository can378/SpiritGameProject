using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    // player 현재 능력치
    public static Player instance { get; private set; }
    // player 현재 상태
    public PlayerStatus status { get; set; }

    Vector2 mousePos;
    public Vector2 mouseDir;
    public float mouseAngle;

    float hAxis;
    float vAxis;

    #region Key Input

    bool rDown;             //재장전
    bool dDown;             //회피
    bool aDown;             //공격
    bool sDown;             // 보조무기 누른 상태
    bool sUp;               // 보조무기 뗀 상태
    bool siDown;            // 선택 아이템
    bool iDown;             //상호작용

    bool skDown;
    bool skUp;

    #endregion

    public LayerMask layerMask;//접근 불가한 레이어 설정
    public GameObject nearObject;
    public GameObject playerItem;

    Vector2 playerPosition;
    
    Vector2 moveVec;
    Vector2 dodgeVec;

    Rigidbody2D rigid;
    SpriteRenderer sprite;


    public MainWeaponController mainWeaponController { get; private set; }
    public SubWeaponController subWeaponController { get; private set; }
    public Skill skill;

    public UserData userData { get; private set; }

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
            //UseSubWeapon();
            //GuardOut();
            Skill();
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

        skDown = Input.GetButtonDown("Skill");
        skUp = Input.GetButtonUp("Skill");
        
    }

    #region Moving
    

    private bool isMoveable() 
    {
        // 레이저가 제대로 도착하면 Null, 막혔을때 방해물 Return
        RaycastHit2D hit;

        // 기본 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도
        float moveSpeed = DataManager.instance.userData.playerSpeed * DataManager.instance.userData.playerDefaultSpeed;
        playerPosition = transform.position;
        Vector2 end= 
            playerPosition + 
            new Vector2(playerPosition.x * moveSpeed, playerPosition.y * moveSpeed);
        

        // 레이저 발사 (시작, 끝, 레이어마스크)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);


        // 벽으로 막혔을때 실행하지 않게 처리
        if (hit.transform == null) {  return true;   }
        return false;
    }

    void Move()     //이동
    {
        
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (status.isAttack || status.isReload || status.isSkill)       // 정지
        {
            moveVec = Vector2.zero;
        }
        if (status.isDodge)             // 회피시 현재 속도 유지
        {
            moveVec = dodgeVec;
        }
        else
        {
            // 기본 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도
            float moveSpeed = DataManager.instance.userData.playerSpeed * DataManager.instance.userData.playerDefaultSpeed;
            rigid.velocity = moveVec * moveSpeed * (status.isSprint ? DataManager.instance.userData.playerRunSpeed : 1f);
            
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
        if (dDown && !status.isAttack && !status.isSkill && moveVec != Vector2.zero && !status.isDodge)
        {
            
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            // 회피 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도 * 회피속도
            float dodgeSpeed = DataManager.instance.userData.playerSpeed * DataManager.instance.userData.playerDefaultSpeed * DataManager.instance.userData.playerDodgeSpeed;
            rigid.velocity = moveVec * dodgeSpeed;
            status.isDodge = true;

            Invoke("DodgeOut", DataManager.instance.userData.playerDodgeTime);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        status.isDodge = false;
    }

    

    // 달리기 대기
    void RunDelay()
    {
        status.runCurrentCoolTime = DataManager.instance.userData.playerRunCoolTime;
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

        if (rDown && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill)
        {
            status.isReload = true;
            //장전 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = mainWeaponController.mainWeapon.reloadTime / DataManager.instance.userData.playerAttackSpeed;
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

        if (aDown && !status.isAttack && !status.isDodge && status.isAttackReady && !status.isSkill)
        {
            status.isAttack = true;

            RunDelay();
            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            mainWeaponController.Use(mousePos);
            // 초당 공격 횟수 = 플레이어 공속 * 무기 공속
            float attackRate = mainWeaponController.mainWeapon.attackSpeed * DataManager.instance.userData.playerAttackSpeed;
            AudioManager.instance.SFXPlay("attack_sword");
            // 다음 공격까지 대기 시간 = 1 / 초당 공격 횟수
            status.attackDelay = 1 / attackRate;
            // 공격 준비 안됨
            status.isAttackReady = false;
            // 공격 완료까지 시간 = (선딜레이 * 공격 중인 시간) / 초당 공격 속도
            Invoke("AttackOut", (mainWeaponController.mainWeapon.preDelay + mainWeaponController.mainWeapon.rate) / attackRate);

        }
    }

    void AttackOut()
    {
        status.isAttack = false;
    }
    
    #endregion

    #region SubWeapon
    
    /*
    // 잠시 비활성화
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

    // 시간이 지나면 자동으로 반격 해체
    void ParryOut()
    {
        status.isSubWeapon = false;
        status.isParry = false;
    }

    // 시간이 지나면 해당 위치로 순간이동
    void TeleportOut()
    {
        gameObject.transform.position = mousePos;
        status.isSubWeapon = false;
    }

    // 클릭을 떼면 가드가 풀림
    void GuardOut()
    {
        if (sUp && status.isGuard && subWeaponController.subWeapon.subWeaponType == SubWeaponType.Guard)
        {
            status.isSubWeapon = false;
            status.isGuard = false;
            status.subWeaponDelay = subWeaponController.subWeapon.coolTime;
        }
    }
    */
    #endregion SubWeapon

    #region Skill

    void Skill()
    {
        if (skill == null)
            return;

        if (skill.skillCoolTime > 0)
            return;

        if (skill.skillLimit != SkillLimit.None && mainWeaponController.mainWeapon == null)
        {
            Debug.Log("무기 없음");
            return;
        }
            

        if (skill.skillLimit == SkillLimit.Shot && mainWeaponController.mainWeapon.weaponType != MainWeaponType.Shot)
        {
            Debug.Log("원거리 전용 스킬");
            return;
        }

        if (skill.skillLimit == SkillLimit.Melee && mainWeaponController.mainWeapon.weaponType != MainWeaponType.Melee)
        {
            Debug.Log("근거리 전용 스킬");
            return;
        }



        if (skDown && !status.isAttack && !status.isDodge && !status.isSkill)
        {
            Debug.Log("스킬 사용");
            status.isSkill = true;

            RunDelay();
            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            skill.Use(gameObject);

            //스킬 사용 중인 시간 = 선딜 + 시전 중 + 후딜
            float skillRate = skill.preDelay + skill.rate + skill.postDelay;
            // 일반 스킬은 플레이어 공속에 영향
            // 그 외에 스킬은 플레이어 공속 * 무기 공속
            if(skill.skillLimit == SkillLimit.None)
            {
                Invoke("SkillOut", skillRate / userData.playerAttackSpeed);
            }
            else 
            {
                Invoke("SkillOut", skillRate / (userData.playerAttackSpeed * mainWeaponController.mainWeapon.attackSpeed));
            }
            
        }
    }

    void SkillOut()
    {
        status.isSkill = false;
    }

    #endregion

    void Interaction()
    {
        if (iDown && nearObject != null && !status.isDodge && !status.isAttack && moveVec == Vector2.zero
        && !status.isAttack && !status.isSkill)
        {
            if (nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
        }
        if (iDown && nearObject != null && !status.isDodge && !status.isAttack && !status.isSkill && moveVec == Vector2.zero)
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
                subWeaponController.UnEquipWeapon();
            }
            // 무기 장비
            subWeaponController.EquipSubWeapon(nearObject.GetComponent<SubWeapon>());
        }
        else if (selectItem.selectItemClass == SelectItemClass.Skill)
        {
            if (skill != null)
            {
                skill.transform.position = transform.position;
                //skill.gameObject.SetActive(true);
            }
            // 스킬 장착
            skill = nearObject.GetComponent<Skill>();
            //skill.gameObject.SetActive(false);
        }
        else if(selectItem.selectItemClass == SelectItemClass.Consumable || selectItem.selectItemClass==SelectItemClass.ThrowWeapon  )
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
            // 적에게 공격 당할시
            // 피해를 입고
            // 뒤로 밀려나며
            // 잠시 무적이 된다.


            Damaged(other.GetComponent<EnemyStatus>().damage);
            //Damaged(10);
            KnockBack(other.gameObject);
            Invincible();
            Invoke("OutInvincible", 0.3f);
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

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    #endregion

    #region Effect
    public void Damaged(float damage)
    {
        if (status.isInvincible)
        {
            damage = 0;
            return;
        }

        //받는 피해 = 감소 전 피해 * 플레이어 피해 감소율
        //damage = damage * DataManager.instance.userData.playerReductionRatio;

        Debug.Log("Player Damaged" + damage);
        userData.playerHP -= damage;
        MapUIManager.instance.UpdateHealthUI();

        
        if(userData.playerHP >= userData.playerHPMax)
        {
            userData.playerHP = userData.playerHPMax;
        }

        if (userData.playerHP < 0)
        {
            Dead();
        }

    }

    public void KnockBack(GameObject agent)
    {
        //튕겨나감
        float distance = 10;
        Vector2 dir = (transform.position - agent.transform.position).normalized;

        //rigid.AddForce(dir * (10 - (10 * subWeaponController.subWeapon.ratio)), ForceMode2D.Impulse);

        if(status.isInvincible)
        {
            distance = 0;
        }

        rigid.AddForce(dir * (distance), ForceMode2D.Impulse);
    }

    public void Invincible()
    {
        status.isInvincible = true;
        int layerNum = LayerMask.NameToLayer("Invincible");
        this.layerMask = layerNum;
        sprite.color = new Color(1, 1, 1, 0.4f);
    }

    void OutInvincible()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        this.layerMask = 0;
        status.isInvincible = false;
    }

    void Dead()
    {
        Debug.Log("player dead");
        DataManager.instance.InitData();
        DataManager.instance.SaveUserData();
        MapUIManager.instance.diePanel.SetActive(true);
    }

    public void ApplyBuff(GameObject effect)
    {
        GameObject Buff = Instantiate(effect);
        StatusEffect statusEffect = Buff.GetComponent<StatusEffect>();
        statusEffect.SetTarget(gameObject);

        statusEffect.ApplyEffect();
        status.activeEffects.Add(statusEffect);
        StartCoroutine(RemoveEffectAfterDuration(statusEffect));
    }

    private IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        yield return new WaitForSeconds(effect.duration);
        effect.RemoveEffect();
        status.activeEffects.Remove(effect);

        Destroy(effect.gameObject);
    }

    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in status.activeEffects)
        {
            effect.RemoveEffect();
        }
        status.activeEffects.Clear();
    }

    #endregion


}
