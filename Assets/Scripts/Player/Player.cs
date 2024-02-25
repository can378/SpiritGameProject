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
    public  PlayerStatus status { get; private set; }

    float hAxis;
    float vAxis;

    #region Key Input

    bool rDown;            //재장전
    bool dDown;           //회피
    bool aDown;            //공격
    bool siDown;           // 선택 아이템
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

    public MainWeaponController mainWeaponController;
    public SkillController skillController;

    public UserData userData { get; private set; }

    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        status = GetComponent<PlayerStatus>();
        
        mainWeaponController = GetComponent<MainWeaponController>();
        skillController = GetComponent<SkillController>();
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
            RunCoolTime();
            Dodge();
            Move();  
        }

        UseItem();
        
        if (status.isAttackable)
        {
            Attack();
            Reload();
            Skill();
            ReadyOut();
            HoldOut();
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

        skDown = Input.GetButtonDown("Skill");      //e Down
        skUp = Input.GetButtonUp("Skill");          //e Up
        
    }

    #region Moving
    

    private bool isMoveable() 
    {
        // 레이저가 제대로 도착하면 Null, 막혔을때 방해물 Return
        RaycastHit2D hit;

        // 기본 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도
        float moveSpeed = userData.playerSpeed * userData.playerDefaultSpeed;
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
            float moveSpeed = userData.playerSpeed * userData.playerDefaultSpeed;
            rigid.velocity = moveVec * moveSpeed * (status.isSprint ? userData.playerRunSpeed : 1f);
            
        }
    }

    void Turn()
    {
        status.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        status.mouseDir = (status.mousePos - (Vector2)transform.position).normalized;

        status.mouseAngle = Mathf.Atan2(status.mousePos.y - transform.position.y, status.mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(mouseAngle - 90, Vector3.forward);

    }
 
    void Dodge()    // 회피
    {
        if (dDown && !status.isAttack && !status.isSkill && moveVec != Vector2.zero && !status.isDodge && !status.isSkillHold)
        {
            
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            // 회피 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도 * 회피속도
            float dodgeSpeed = userData.playerSpeed * userData.playerDefaultSpeed * userData.playerDodgeSpeed;
            rigid.velocity = moveVec * dodgeSpeed;
            status.isDodge = true;

            Invoke("DodgeOut", userData.playerDodgeTime);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        status.isDodge = false;
    }

    void RunCoolTime()
    {
        if(status.isAttack || status.isSkill || status.isSkillHold)
        {
            status.isSprint = false;
            status.runCurrentCoolTime = userData.playerRunCoolTime;
            return;
        }

        status.runCurrentCoolTime -= Time.deltaTime;
        status.isSprint = status.runCurrentCoolTime > 0 ? false : true;
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

        if (rDown && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            //장전 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = mainWeaponController.mainWeapon.reloadTime / userData.playerAttackSpeed;
            Invoke("ReloadOut", mainWeaponController.mainWeapon.reloadTime);
        }
    }

    void ReloadOut()
    {
        Debug.Log("스킬 홀드 중단");
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

        if (aDown && !status.isAttack && !status.isDodge && status.isAttackReady && !status.isSkill && !status.isSkillReady && !status.isSkillHold)
        {
            status.isAttack = true;

            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            mainWeaponController.Use(status.mousePos);
            // 초당 공격 횟수 = 플레이어 공속 * 무기 공속
            float attackRate = mainWeaponController.mainWeapon.attackSpeed * userData.playerAttackSpeed;
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

    #region Skill

    void Skill()
    {
        if (skillController.skill == null)
            return;

        if (skillController.skill.skillCoolTime > 0)
            return;

        if (skillController.skill.skillLimit != SkillLimit.None && mainWeaponController.mainWeapon == null)
        {
            Debug.Log("무기 없음");
            return;
        }
            

        if (skillController.skill.skillLimit == SkillLimit.Shot && mainWeaponController.mainWeapon.weaponType != MainWeaponType.Shot)
        {
            Debug.Log("원거리 전용 스킬");
            return;
        }

        if (skillController.skill.skillLimit == SkillLimit.Melee && mainWeaponController.mainWeapon.weaponType != MainWeaponType.Melee)
        {
            Debug.Log("근거리 전용 스킬");
            return;
        }

        // 스킬 키 다운
        if (skDown && !status.isAttack && !status.isDodge && !status.isSkill)
        {
            skillController.SkillDown();
        }

    }

    void ReadyOut()
    {
        if (skillController.skill == null)
            return;

        // 스킬 준비 상태에서 공격 키 다운
        if (aDown && !status.isAttack && !status.isDodge && !status.isSkill && status.isSkillReady)
        {
            StartCoroutine(skillController.Immediate());
        }
    }

    void HoldOut()
    {
        if (skillController.skill == null)
            return;

        //스킬 hold 상태에서 스킬 키 up
        if (skUp && !status.isAttack && !status.isDodge && !status.isSkill && status.isSkillHold)
        {
            skillController.HoldOut();
        }
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
                if (userData.key > 0) 
                { 
                    userData.key--;
                    MapUIManager.instance.UpdateKeyUI();
                    nearObject.GetComponent<Door>().DoorInteraction(); 
                }
                
            }
            else if (nearObject.tag == "ShabbyWall")
            {
                //open with bomb
                //nearObject.GetComponent<Wall>().WallInteraction();
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
        else if (selectItem.selectItemClass == SelectItemClass.Equipments)
        {
            
        }
        else if (selectItem.selectItemClass == SelectItemClass.Skill)
        {
            if (skillController.skill != null)
            {
                skillController.UnEquipSkill();
            }
            // 스킬 장착
            skillController.EquipSkill(nearObject.GetComponent<Skill>());
        }
        else if(selectItem.selectItemClass == SelectItemClass.Consumable || selectItem.selectItemClass==SelectItemClass.ThrowWeapon  )
        {
            //전에 가지고 있던 아이템 드랍
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //아이템 갱신
            userData.playerItem = selectItem.GetComponent<ItemInfo>().selectItemName.ToString();
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
            { mainWeaponController.UseItem(playerItem, status.mousePos); }
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
            userData.playerItem = "";

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

    // 상태 관련
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
