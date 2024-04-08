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
    public PlayerStats stats {get; private set; }

    public float hAxis;
    public float vAxis;

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

    public WeaponController WeaponController;
    public SkillController skillController;

    public UserData userData { get; private set; }

    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
        
        WeaponController = GetComponent<WeaponController>();
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
            Reload();
            Attack();
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
        playerPosition = transform.position;
        Vector2 end= 
            playerPosition + 
            new Vector2(playerPosition.x * stats.moveSpeed, playerPosition.y * stats.moveSpeed);
        

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
            rigid.velocity = moveVec * stats.moveSpeed * (status.isSprint ? stats.runSpeed : 1f);
            
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
            // 회피 속도 = 플레이어 이동속도 * 회피속도
            float dodgeSpeed = stats.moveSpeed * stats.dodgeSpeed;
            rigid.velocity = moveVec * dodgeSpeed;
            status.isDodge = true;

            Invoke("DodgeOut", stats.dodgeTime);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        status.isDodge = false;
    }

    void RunCoolTime()
    {
        if(status.isAttack || status.isSkillHold)
        {
            status.isSprint = false;
            status.runCurrentCoolTime = stats.runCoolTime;
            return;
        }

        status.runCurrentCoolTime -= Time.deltaTime;
        status.isSprint = status.runCurrentCoolTime > 0 ? false : true;
    }

    #endregion

    #region Attack

    void Reload()
    {
        if (stats.weapon == null)
            return;

        if (stats.weapon.maxAmmo < 0)
            return;

        if (stats.weapon.maxAmmo == stats.weapon.ammo)
            return;

        if (rDown && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            //장전 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = stats.weapon.reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }

        if (aDown && status.attackDelay < 0 && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold && stats.weapon.ammo == 0)
        {
            status.isReload = true;
            //장전 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = stats.weapon.reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }
    }

    void ReloadOut()
    {
        stats.weapon.Reload();
        status.isReload = false;
    }

    void Attack()
    {
        status.attackDelay -= Time.deltaTime;

        if (stats.weapon == null)
            return;

        if (stats.weapon.ammo == 0)
            return;

        status.isAttackReady = status.attackDelay <= 0;

        if (aDown && !status.isAttack && !status.isDodge && status.isAttackReady && !status.isSkill && !status.isSkillReady && !status.isSkillHold)
        {
            status.isAttack = true;

            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            WeaponController.Use(status.mousePos);
            // 초당 공격 횟수 = 플레이어 공속 * 무기 공속
            float attackRate = stats.weapon.attackSpeed * stats.attackSpeed;
            AudioManager.instance.SFXPlay("attack_sword");
            // 다음 공격까지 대기 시간 = 1 / 초당 공격 횟수
            status.attackDelay = (stats.weapon.preDelay + stats.weapon.rate + stats.weapon.postDelay) / attackRate;
            // 공격 준비 안됨
            status.isAttackReady = false;
            // 공격 시간(움직이기까지 대기 시간) = (선딜레이 * 공격 중인 시간) / 초당 공격 속도
            Invoke("AttackOut", (stats.weapon.preDelay + stats.weapon.rate) / attackRate);
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
        if (stats.skill == null)
            return;

        if (stats.skill.skillCoolTime > 0)
            return;

        if (stats.skill.skillLimit != SkillLimit.None && stats.weapon == null)
        {
            Debug.Log("무기 없음");
            return;
        }

        if (stats.skill.skillLimit == SkillLimit.Shot &&  stats.weapon.weaponType < 10)
        {
            Debug.Log("원거리 전용 스킬");
            return;
        }

        if (stats.skill.skillLimit == SkillLimit.Melee && 10 <= stats.weapon.weaponType)
        {
            Debug.Log("근거리 전용 스킬");
            return;
        }

        // 스킬 키 다운
        if (skDown && !status.isAttack && !status.isDodge && !status.isSkill)
        {
            skillController.SkillDown();
            print(stats.skillCoolTime);
        }

    }

    void ReadyOut()
    {
        if (stats.skill == null)
            return;

        // 스킬 준비 상태에서 공격 키 다운
        if (aDown && !status.isAttack && !status.isDodge && !status.isSkill && status.isSkillReady)
        {
            StartCoroutine(skillController.Immediate());
        }
    }

    void HoldOut()
    {
        if (stats.skill == null)
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
        if (iDown && nearObject != null && !status.isDodge && !status.isAttack && !status.isSkill && moveVec == Vector2.zero)
        {
            if (nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
            else if (nearObject.tag == "Npc")
            {
                nearObject.GetComponent<NPCbasic>().Conversation();
            }
            else if (nearObject.tag == "Door")
            {
                if (stats.key > 0) 
                {
                    stats.key--;
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
            if (stats.weapon != null)
            {
                WeaponController.UnEquipWeapon();
            }
            // 무기 장비
            WeaponController.EquipWeapon(selectItem.GetComponent<Weapon>());
        }
        else if (selectItem.selectItemClass == SelectItemClass.Equipments)
        {
            for(int i = 0;i<3;i++)
            {
                if(stats.equipments[i] ==null)
                {
                    EquipEquipment(selectItem.GetComponent<Equipment>(),i);
                    return;
                }
            }
        }
        else if (selectItem.selectItemClass == SelectItemClass.Skill)
        {
            if (stats.skill != null)
            {
                skillController.UnEquipSkill();
            }
            // 스킬 장착
            skillController.EquipSkill(selectItem.GetComponent<Skill>());
        }
        else if(selectItem.selectItemClass == SelectItemClass.Consumable || selectItem.selectItemClass==SelectItemClass.ThrowWeapon  )
        {
            //전에 가지고 있던 아이템 드랍
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //아이템 갱신
            stats.item = selectItem.GetComponent<ItemInfo>().selectItemName.ToString();
            playerItem = selectItem.gameObject;
            playerItem.SetActive(false);

            MapUIManager.instance.updateItemUI(selectItem.gameObject);
        }
    }

    void UseItem()
    {
        if (siDown && playerItem != null)
        {
            Debug.Log("UseSelectItem");
            //Throwing Items
            if (playerItem.GetComponent<SelectItem>().selectItemClass == SelectItemClass.ThrowWeapon)
            { WeaponController.UseItem(playerItem, status.mousePos); }
            //Consumable Item
            else 
            {
                switch (playerItem.GetComponent<ItemInfo>().selectItemName)
                {
                    case SelectItemName.HPPortion:
                        stats.HP += 10;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.SpeedPortion:
                        break;
                    case SelectItemName.SkillPortion:
                        break;
                    // 밑에 아이템들은 획득 즉시로 바꾸었으면 좋겠습니다.
                    case SelectItemName.Insam:
                        stats.HP += 20;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.Sansam:
                        stats.HP += 30;
                        MapUIManager.instance.UpdateHealthUI();
                        break;
                    case SelectItemName.SmallArmor:
                        stats.tempHP += 10;
                        break;
                    case SelectItemName.LargeArmor:
                        stats.tempHP += 20;
                        break;
                    case SelectItemName.NormalArmor:
                        stats.tempHP += 30;
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
            stats.item = "";

        }

    }
    
    public void EquipEquipment(Equipment equipment, int index)
    {
        stats.equipments[index] = equipment.GetComponent<Equipment>();
        stats.equipments[index].Equip();
        stats.equipments[index].gameObject.SetActive(false);

        MapUIManager.instance.UpdateEquipmentUI();
    }

    public void UnEquipEquipment(int index)
    {
        if(stats.equipments[index] == null)
            return;
        stats.equipments[index].gameObject.transform.position = gameObject.transform.position;
        stats.equipments[index].gameObject.SetActive(true);

        // 장비 능력치 해제
        stats.equipments[index].UnEquip();

        // 장비 해제
        stats.equipments[index] = null;

        MapUIManager.instance.UpdateEquipmentUI();
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
        if (scene.name != "Main")
        {
            stats.level = DataManager.instance.userData.playerLevel;
            stats.exp = DataManager.instance.userData.playerExp;
            stats.point = DataManager.instance.userData.playerPoint;

            stats.HP = DataManager.instance.userData.playerHP;
            stats.tempHP = DataManager.instance.userData.playerTempHP;

            //Debug.Log("Scene reloaded: " + scene.name);
            //Scene reload 후에도 전에 얻은 아이템 유지
            string playerItemName = DataManager.instance.userData.playerItem;
            int playerWeapon = DataManager.instance.userData.playerWeapon;
            int playerSkill = DataManager.instance.userData.playerSkill;
            int playerMaxEquipment = DataManager.instance.userData.playerMaxEquipment;
            int[] playerEquipment = DataManager.instance.userData.playerEquipments;

            for(int i = 0;i<stats.playerStat.Length;i++)
            {
                stats.playerStat[i] = DataManager.instance.userData.playerStat[i];
            }
            statApply();

            stats.coin = DataManager.instance.userData.playerCoin;
            stats.key = DataManager.instance.userData.playerKey;

            //아이템
            if(playerItemName != "")
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
            // 무기
            if (playerWeapon != 0)
            {
                WeaponController.EquipWeapon(Instantiate(DataManager.instance.gameData.weaponList[playerWeapon]).GetComponent<Weapon>());
            }
            // 스킬
            if (playerSkill != 0)
            {
                skillController.EquipSkill(Instantiate(DataManager.instance.gameData.skillList[playerSkill]).GetComponent<Skill>());
            }
            // 방어구
            
            for(int i = 0;i< playerMaxEquipment; i++)
            {
                if(playerEquipment[i] != 0)
                    EquipEquipment(Instantiate(DataManager.instance.gameData.equipmentList[playerEquipment[i]]).GetComponent<Equipment>(),i);
            }
            
            
        }

    }

    // 수정할 것
    // 지금 곱셉으로 적용돼서 이상함
    public void statApply()
    {
        Player.instance.stats.HPMax += Player.instance.stats.playerStat[0] * 25;
        Player.instance.stats.addAttackPower += Player.instance.stats.playerStat[1] * 0.20f;
        Player.instance.stats.addAttackSpeed += Player.instance.stats.playerStat[2] * 0.20f;
        Player.instance.stats.addCriticalChance += Player.instance.stats.playerStat[3] * 0.1f;
        Player.instance.stats.addCriticalDamage += Player.instance.stats.playerStat[4] * 0.05f;
        Player.instance.stats.addSkillPower += Player.instance.stats.playerStat[5] * 10f;
        Player.instance.stats.addSkillCoolTime -= Player.instance.stats.playerStat[6] * 0.10f;
        Player.instance.stats.addMoveSpeed += Player.instance.stats.playerStat[7] * 0.1f;

        MapUIManager.instance.UpdateHealthUI();
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

            Damaged(other.GetComponent<EnemyStats>().attackPower);
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
                stats.coin++;
                MapUIManager.instance.UpdateCoinUI();
            }

            if (item.itemClass == ItemClass.Key)
            {
                Destroy(other.gameObject); //키 오브젝트 삭제
                stats.key++;
                MapUIManager.instance.UpdateKeyUI();
            }

            MapUIManager.instance.UpdateMinimapUI(false);
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SelectItem" || other.tag == "Door" || other.tag == "ShabbyWall" || other.tag == "Npc")
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
        damage = damage * stats.defensivePower;

        //Debug.Log("Player Damaged" + damage);
        stats.HP -= damage;

        MapUIManager.instance.UpdateHealthUI();

        
        if(stats.HP >= stats.HPMax)
        {
            stats.HP = stats.HPMax;
        }

        if (stats.HP < 0)
        {
            Dead();
        }

    }

    public void KnockBack(GameObject agent)
    {
        //튕겨나감
        float distance = 10 * (1 - stats.defensivePower);
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
        // 가지고 있는 버프인지 체크한다.
        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();
        foreach (StatusEffect buff in stats.activeEffects)
        {
            // 가지고 있는 버프라면 갱신한다.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }
        
        // 가지고 있는 버프가 아니라면 새로 추가한다.
        GameObject Buff = Instantiate(effect);
        statusEffect = Buff.GetComponent<StatusEffect>();
        statusEffect.SetTarget(gameObject);

        statusEffect.ApplyEffect();
        stats.activeEffects.Add(statusEffect);
        
        StartCoroutine(RemoveEffectAfterDuration(statusEffect));
    }

    private IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            effect.duration -= 0.1f;
            if(effect.duration <= 0)
            {
                break;
            }
        }
        effect.RemoveEffect();
        stats.activeEffects.Remove(effect);

        Destroy(effect.gameObject);
    }

    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in stats.activeEffects)
        {
            effect.RemoveEffect();
        }
        stats.activeEffects.Clear();
    }

    #endregion


}
