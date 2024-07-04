using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using System;

public class Player : ObjectBasic
{

    public static Player instance { get; private set; }         
    public PlayerStatus status { get; private set; }                        // player 현재 능력치
    public PlayerStats playerStats {get; private set; }                     // player 현재 능력치
    public UserData userData { get; private set; }

    [HideInInspector] public float hAxis;
    [HideInInspector] public float vAxis;

    #region Key Input

    public bool rDown { get; private set; }                                 // 재장전
    public bool dDown { get; private set; }                                 // 회피
    public bool aDown { get; private set; }                                 // 공격
    public bool siDown { get; private set; }                                // 선택 아이템 H
    public bool iDown { get; private set; }                                 // 상호작용

    public float skcDown { get; private set; }                              // 스킬 변경
    public bool skDown { get; private set; }                                // 스킬 키 다운 중

    #endregion

    public LayerMask layerMask;             //접근 불가한 레이어 설정
    public GameObject nearObject;
    //public GameObject playerItem;

    Vector2 playerPosition;
    Vector2 dodgeVec;

    public WeaponController weaponController;
    public SkillController skillController;
    public Equipment[] equipmentList;

    protected override void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        status = GetComponent<PlayerStatus>();
        stats = playerStats = GetComponent<PlayerStats>();
        
        weaponController = GetComponent<WeaponController>();
        skillController = GetComponent<SkillController>();
    }

    void Start()
    {
        userData = DataManager.instance.userData;
        defaultLayer = this.gameObject.layer;
    }

    void Update()
    {
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        GetInput();

        /*
        if (isMoveable())
        {
            Run();
            Dodge();
            Move();  
        }
        */

        Turn();
        //Run();
        Dodge();
        Move();
        //UseItem();
        Interaction();
        HalfDead();

        if (status.isAttackable)
        {
            Reload();
            ReloadOut();
            Attack();
            SkillDown();
            SkillUp();
            SkillChange();
        }
        
        string layerName = LayerMask.LayerToName(gameObject.layer);
        //Debug.Log("My layer name is: " + layerName);
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

        skcDown = Input.GetAxisRaw("Mouse ScrollWheel");
        skDown = Input.GetButton("Skill");          //e Down
        
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
            new Vector2(playerPosition.x * playerStats.moveSpeed, playerPosition.y * playerStats.moveSpeed);
        

        // 레이저 발사 (시작, 끝, 레이어마스크)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);
        Debug.DrawRay(playerPosition, end, Color.blue);


        // 벽으로 막혔을때 실행하지 않게 처리
        if (hit.transform == null) {  return true;   }
        return false;
    }

    void Move()     //이동
    {
        if( isFlinch )
            return;

        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack || status.isSkill)       // 정지
        {
            moveVec = Vector2.zero;
        }
        
        if (status.isDodge)             // 회피시 현재 속도 유지
        {
            rigid.velocity  = dodgeVec * playerStats.moveSpeed * (1 + playerStats.dodgeSpeed);
        }
        else
        {
            // 기본 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도
            rigid.velocity = moveVec * playerStats.moveSpeed;
        }
    }

    void Turn()
    {
        status.mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        status.mouseDir = (Vector2)(status.mousePos - transform.position).normalized;

        status.mouseAngle = Mathf.Atan2(status.mousePos.y - transform.position.y, status.mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(mouseAngle - 90, Vector3.forward);

    }
 
    void Dodge()    // 회피
    {
        if(moveVec == Vector2.zero)
            return;
        
        if (dDown && !isFlinch && !isAttack && !status.isSkill  && !status.isDodge && !status.isSkillHold)
        {
            dodgeVec = moveVec;
            status.isDodge = true;
            status.isReload = false;

            Invoke("DodgeOut", playerStats.dodgeTime);

        }
    }

    void DodgeOut() // 회피 빠져나가기
    {
        status.isDodge = false;
    }

    /*
    void Run()
    {
        if(isAttack || isFlinch || status.isSkillHold || status.isDodge)
        {
            status.isSprint = false;
            status.runCurrentCoolTime = playerStats.runCoolTime;
            return;
        }

        status.runCurrentCoolTime -= Time.deltaTime;
        status.isSprint = status.runCurrentCoolTime <= 0 ? true : false;
    }
    */

    #endregion

    #region Attack

    void Reload()
    {
        if (playerStats.weapon == 0)
            return;

        if (weaponController.weaponList[playerStats.weapon].maxAmmo < 0)
            return;

        if (weaponController.weaponList[playerStats.weapon].maxAmmo == weaponController.weaponList[playerStats.weapon].ammo)
            return;

        if (rDown && !isFlinch && !status.isDodge && !status.isReload && !isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            status.reloadDelay = 0f;
        }

        if (aDown && !isFlinch && status.attackDelay < 0 && weaponController.weaponList[playerStats.weapon].ammo == 0 && !status.isDodge && !status.isReload && !isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            status.reloadDelay = 0f;
        }
    }

    void ReloadOut()
    {
        if (!status.isReload)
            return;

        status.reloadDelay += Time.deltaTime * (playerStats.attackSpeed);

        if(status.reloadDelay >= weaponController.weaponList[playerStats.weapon].reloadTime)
        {
            weaponController.weaponList[playerStats.weapon].Reload();
            status.isReload = false;
        }
    }

    void Attack()
    {
        status.attackDelay -= Time.deltaTime;

        if (playerStats.weapon == 0)
            return;

        if (weaponController.weaponList[playerStats.weapon].ammo == 0)
            return;

        isAttackReady = status.attackDelay <= 0;

        if (aDown && !isFlinch && !isAttack && !status.isReload && !status.isDodge && isAttackReady && !status.isSkill && !status.isSkillHold )
        {
            // 공격 준비 안됨
            isAttackReady = false;
            isAttack = true;

            weaponController.Use(status.mousePos);

            AudioManager.instance.SFXPlay("attack_sword");

            // 다음 공격까지 대기 시간 = 1 / 초당 공격 횟수
            status.attackDelay = weaponController.weaponList[playerStats.weapon].SPA / playerStats.attackSpeed;

            // 공격 시간(움직이기까지 대기 시간) = (선딜레이 * 공격 중인 시간) / 초당 공격 속도
            Invoke("AttackOut", (weaponController.weaponList[playerStats.weapon].preDelay + weaponController.weaponList[playerStats.weapon].rate) / playerStats.attackSpeed);
        }
    }

    void AttackOut()
    {
        isAttack = false;
    }
    
    #endregion

    #region Skill

    void SkillDown()
    {
        if (playerStats.skill[status.skillIndex] == 0)
            return;

        if (skillController.skillList[playerStats.skill[status.skillIndex]].skillCoolTime > 0)
            return;

        // 스킬 키 다운
        if (skDown && !isFlinch && !isAttack && !status.isDodge && !status.isSkill && !status.isSkillHold )
        {
            //스킬이 제한이 있는 상태에서 적절한 무기가 가지고 있지 않을 때
            if (skillController.skillList[playerStats.skill[status.skillIndex]].skillLimit.Length != 0 && 
            Array.IndexOf(skillController.skillList[playerStats.skill[status.skillIndex]].skillLimit, weaponController.weaponList[playerStats.weapon].weaponType) == -1)
            {
                return;
            }
            skillController.SkillDown();
        }

    }

    void SkillUp()
    {
        if (playerStats.skill[status.skillIndex] == 0)
            return;

        //스킬 hold 상태에서 스킬 키 up
        if ((isFlinch || !skDown) && !isAttack && !status.isDodge && !status.isSkill && status.isSkillHold)
        {
            StartCoroutine(skillController.Exit());
            status.isReload = false;
        }
    }

    void SkillChange()
    {
        status.skillChangeDelay -= Time.deltaTime;

        if (skcDown != 0f && !isFlinch && !status.isSkill && !status.isSkillHold && status.skillChangeDelay <= 0f)
        {
            status.skillChangeDelay = 0.1f;
            if(skcDown > 0f)
            {
                status.skillIndex = status.skillIndex + 1 > playerStats.maxSkillSlot - 1 ? playerStats.maxSkillSlot - 1 : status.skillIndex + 1;
            }
            else if(skcDown < 0f)
            {
                status.skillIndex = 0 > status.skillIndex - 1 ? 0 : status.skillIndex - 1;
            }
        }
    }

    #endregion

    #region Interaction

    void Interaction()
    {
        if(nearObject == null)
            return;

        if (iDown && !isFlinch && !status.isDodge && !isAttack && !status.isSkill && !status.isSkillHold)
        {

            if (nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
            else if (nearObject.tag == "Npc")
            {
                nearObject.GetComponentInParent<NPCbasic>().Conversation();
            }
            else if (nearObject.tag == "Door")
            {
                if (playerStats.key > 0)
                {
                    playerStats.key--;
                    MapUIManager.instance.UpdateKeyUI();
                    nearObject.GetComponent<Door>().DoorInteraction();
                }

            }
            else if (nearObject.tag == "ShabbyWall")
            {
                //open with bomb
                //nearObject.GetComponent<Wall>().WallInteraction();
            }
            else if (nearObject.tag == "reward")
            {
                print("reward interaction");
                nearObject.GetComponent<treasureBox>().Interaction();
            
            }
        }

    }

    #endregion Interaction

    #region Item

    void GainSelectItem()
    {
        SelectItem selectItem = nearObject.GetComponent<SelectItem>();
        bool gainItem = false;

        //무기////////////////////////////////////////////////////////////////////
        if (selectItem.selectItemClass == SelectItemClass.Weapon)
        {
            if (playerStats.weapon != 0)
            {
                weaponController.UnEquipWeapon();
            }
            // 무기 장비
            gainItem = weaponController.EquipWeapon(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        //갑옷/////////////////////////////////////////////////////////////////////
        else if (selectItem.selectItemClass == SelectItemClass.Equipments)
        {
            gainItem = EquipEquipment(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        //스킬/////////////////////////////////////////////////////////////////////
        else if (selectItem.selectItemClass == SelectItemClass.Skill)
        {

            if (playerStats.skill[status.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // 스킬 장착
            gainItem = skillController.EquipSkill(selectItem.GetComponent<SelectItem>().selectItemID);

        }
        //일반 아이템/////////////////////////////////////////////////////////////////
        else if(selectItem.selectItemClass == SelectItemClass.Consumable && selectItem.GetComponent<HPPortion>()!=null)
        {
            /*
            //전에 가지고 있던 아이템 드랍
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //아이템 갱신
            playerStats.item = selectItem.GetComponent<SelectItem>().selectItemID;
            playerItem = selectItem.gameObject;
            playerItem.SetActive(false);

            MapUIManager.instance.updateItemUI(selectItem.gameObject);
            */

            //UseItem
            selectItem.GetComponent<HPPortion>().UseItem(FindObj.instance.Player.GetComponent<Player>());
            gainItem = true;
        }

        if(gainItem)
            Destroy(selectItem.gameObject);
    }

    /*
    void UseItem()
    {
        if (siDown && playerItem != null)
        {
            Debug.Log("UseSelectItem");
            //Throwing Items
            if (playerItem.GetComponent<Consumable>().throwItem)
            { weaponController.UseItem(playerItem, status.mousePos); }
            //Consumable Item
            else 
            {
                playerItem.GetComponent<Consumable>().UseItem(this);
                Destroy(playerItem);
            }
           
            
            //"no item" status
            MapUIManager.instance.updateItemUI(null);
            playerItem = null;
            playerStats.item = 0;
        }
    }
    */


    // 장착할 장비의 index
    public bool EquipEquipment(int equipmentId)
    {
        bool equipOK = false;

        for(int i = 0 ; i < playerStats.equipments.Length ; i++)
        {
            // 중복 장착 불가
            if (playerStats.equipments[i] == equipmentId && playerStats.equipments[i] != 0)
                break;
            if (playerStats.equipments[i] != 0)
                continue;

            playerStats.equipments[i] = equipmentId;
            equipOK = true;
            break;
        }

        if(!equipOK)
            return false;

        equipmentList[equipmentId].gameObject.SetActive(true);
        equipmentList[equipmentId].Equip(this.gameObject.GetComponent<Player>());

        return true;
    }

    // 현재 장착한 장비 중 해제할 index
    public bool UnEquipEquipment(int index)
    {
        if (playerStats.equipments[index] == 0)
            return false;

        // 현재 위치에 장비를 놓는다.
        Instantiate(GameData.instance.equipmentList[playerStats.equipments[index]], gameObject.transform.position, gameObject.transform.localRotation);

        // 무기 능력치 해제
        equipmentList[playerStats.equipments[index]].UnEquip(this.gameObject.GetComponent<Player>());
        equipmentList[playerStats.equipments[index]].gameObject.SetActive(false);

        // 무기 해제
        playerStats.equipments[index] = 0;
        //MapUIManager.instance.UpdateEquipmentUI();
        return true;
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
            //playerStats.level = DataManager.instance.userData.playerLevel;
            //playerStats.exp = DataManager.instance.userData.playerExp;
            //playerStats.point = DataManager.instance.userData.playerPoint;

            playerStats.HP = DataManager.instance.userData.playerHP;
            playerStats.tempHP = DataManager.instance.userData.playerTempHP;

            //Debug.Log("Scene reloaded: " + scene.name);
            //Scene reload 후에도 전에 얻은 아이템 유지
            int playerItemName = DataManager.instance.userData.playerItem;
            int playerWeapon = DataManager.instance.userData.playerWeapon;
            int playerSkill = DataManager.instance.userData.playerSkill;
            int[] playerEquipment = DataManager.instance.userData.playerEquipments;

            for(int i = 0;i<playerStats.playerStat.Length;i++)
            {
                playerStats.playerStat[i] = DataManager.instance.userData.playerStat[i];
            }
            statApply();

            playerStats.coin = DataManager.instance.userData.playerCoin;
            playerStats.key = DataManager.instance.userData.playerKey;
            
            /*
            //아이템
            if(playerItemName != 0)
            {
                foreach (GameObject obj in DataManager.instance.gameData.selectItemList)
                {
                    if (obj.GetComponent<Consumable>().selectItemID == playerItemName)
                    {
                        playerItem = Instantiate(obj);
                        MapUIManager.instance.updateItemUI(playerItem.gameObject);
                        playerItem.SetActive(false);
                        break;
                    }
                }
            }
            */

            // 무기
            if (playerWeapon != 0)
            {
                weaponController.EquipWeapon(playerWeapon);
            }
            // 스킬
            if (playerSkill != 0)
            {
                skillController.EquipSkill(playerSkill);
            }
            // 방어구
            
            for(int i = 0;i< playerEquipment.Length; i++)
            {
                if(playerEquipment[i] != 0)
                    EquipEquipment(playerEquipment[i]);
            }
            
            
        }

    }

    // 수정할 것
    // 지금 곱셉으로 적용돼서 이상함
    public void statApply()
    {
        Player.instance.playerStats.HPMax += Player.instance.playerStats.playerStat[0] * 25;
        Player.instance.playerStats.addAttackPower += Player.instance.playerStats.playerStat[1] * 0.20f;
        Player.instance.playerStats.addAttackSpeed += Player.instance.playerStats.playerStat[2] * 0.20f;
        Player.instance.playerStats.addCriticalChance += Player.instance.playerStats.playerStat[3] * 0.1f;
        Player.instance.playerStats.addCriticalDamage += Player.instance.playerStats.playerStat[4] * 0.05f;
        Player.instance.playerStats.addSkillPower += Player.instance.playerStats.playerStat[5] * 10f;
        Player.instance.playerStats.addSkillCoolTime -= Player.instance.playerStats.playerStat[6] * 0.10f;
        Player.instance.playerStats.addMoveSpeed += Player.instance.playerStats.playerStat[7] * 0.1f;

        MapUIManager.instance.UpdateHealthUI();
    }
    #endregion

    #region Trigger

    void OnTriggerEnter2D(Collider2D other)
    {
        //공격받음
        if (other.tag == "EnemyAttack" || other.tag == "AllAttack")
        {
            // 적에게 공격 당할시
            // 피해를 입고
            // 뒤로 밀려나며
            // 잠시 무적이 된다.
            BeAttacked(other.gameObject.GetComponent<HitDetection>());

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
                playerStats.coin++;
                MapUIManager.instance.UpdateCoinUI();
            }
            else if (item.itemClass == ItemClass.Key)
            {
                Destroy(other.gameObject); //키 오브젝트 삭제
                playerStats.key++;
                MapUIManager.instance.UpdateKeyUI();
            }
            else if(item.itemClass == ItemClass.Heal)
            {
                Destroy(other.gameObject); //코인 오브젝트 삭제
                playerStats.HP += 20f;
            }
            else if(item.itemClass == ItemClass.ExtraHealth)
            {
                Destroy(other.gameObject); //키 오브젝트 삭제
                playerStats.tempHP += 10f;
            }

            MapUIManager.instance.UpdateMinimapUI(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SelectItem" || other.tag == "Door" || other.tag == "ShabbyWall" || other.tag == "Npc" || other.tag=="reward")
        {
            if(nearObject == null || Vector2.Distance(transform.position, other.transform.position) < Vector2.Distance(transform.position, nearObject.transform.position))
            {
                nearObject = other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearObject = null;
    }

    #endregion

    // 상태 관련
    #region Effect

    /*
    //적에게 피격
    public void EnemyAttack(GameObject attacker)
    {
        if (status.isInvincible)
        {
            return;
        }

        HitDetection hitDetection = attacker.GetComponent<HitDetection>();


        Damaged(hitDetection.damage);

        KnockBack(attacker.gameObject, hitDetection.knockBack);

        if(FlinchCoroutine != null) StopCoroutine(FlinchCoroutine);
        FlinchCoroutine = StartCoroutine(Flinch(0.3f));

        Invincible(0.1f);

        if (hitDetection.statusEffect != null)
        {
            foreach (int statusEffectIndex in hitDetection.statusEffect)
            {
                ApplyBuff(GameData.instance.statusEffectList[statusEffectIndex]);
            }
        }
    }
    */
    
    // 피해
    public override void Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        base.Damaged(damage,critical,criticalDamage);
        MapUIManager.instance.UpdateHealthUI();
    }

    public override void Dead()
    {
        DataManager.instance.InitData();
        DataManager.instance.SaveUserData();
        MapUIManager.instance.diePanel.SetActive(true);
        base.Dead();
    }
    /*
    void DamagedOut()
    {
        sprite.color = Color.white;
    }

    // 뒤로 밀려남
    public void KnockBack(GameObject agent, float distance = 10)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;

        rigid.AddForce(dir * (distance * (1 - playerStats.defensivePower)), ForceMode2D.Impulse);
    }

    // 경직됨(움직일 수 없음)
    public IEnumerator Flinch(float time = 0)
    {
        status.isFlinch = true;

        yield return new WaitForSeconds(time);

        status.isFlinch = false;
    }

    // 무적(적 공격 무시)
    public void Invincible(float time = 0)
    {
        status.isInvincible = true;
        int layerNum = LayerMask.NameToLayer("Invincible");
        this.layerMask = layerNum;
        sprite.color = new Color(1, 1, 1, 0.4f);
        Invoke("InvincibleOut", time);
    }

    void InvincibleOut()
    {
        //무적 해제
        sprite.color = new Color(1, 1, 1, 1);
        this.layerMask = LayerMask.NameToLayer("Player");
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
        foreach (StatusEffect buff in playerStats.activeEffects)
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
        playerStats.activeEffects.Add(statusEffect);
        
        StartCoroutine(RemoveEffectAfterDuration(statusEffect));
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
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
        playerStats.activeEffects.Remove(effect);

        Destroy(effect.gameObject);
    }

    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in playerStats.activeEffects)
        {
            effect.RemoveEffect();
        }
        playerStats.activeEffects.Clear();
    }
    */

    #endregion


}
