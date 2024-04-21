using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using System;

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

    public WeaponController weaponController;
    public SkillController skillController;

    public UserData userData { get; private set; }

    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        status = GetComponent<PlayerStatus>();
        stats = GetComponent<PlayerStats>();
        
        weaponController = GetComponent<WeaponController>();
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
            Run();
            Dodge();
            Move();  
        }

        UseItem();
        
        if (status.isAttackable)
        {
            Reload();
            Attack();
            SkillDown();
            SkillUp();
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

        if (status.isAttack || status.isReload || status.isSkill || status.isFlinch)       // 정지
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
        if(moveVec == Vector2.zero)
            return;
        
        if (dDown && !status.isFlinch && !status.isAttack && !status.isSkill  && !status.isDodge && !status.isSkillHold)
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

    void Run()
    {
        if(status.isAttack || status.isFlinch || status.isSkillHold || !status.isAttackReady )
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
        if (stats.weapon == 0)
            return;

        if (weaponController.weaponList[stats.weapon].maxAmmo < 0)
            return;

        if (weaponController.weaponList[stats.weapon].maxAmmo == weaponController.weaponList[stats.weapon].ammo)
            return;

        if (rDown && !status.isFlinch && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            //장전에 걸리는 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = weaponController.weaponList[stats.weapon].reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }

        if (aDown && !status.isFlinch && status.attackDelay < 0 && weaponController.weaponList[stats.weapon].ammo == 0 && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            //장전 시간 = 무기 장전 시간 / 플레이어 공격 속도
            float reloadTime = weaponController.weaponList[stats.weapon].reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }
    }

    void ReloadOut()
    {
        weaponController.weaponList[stats.weapon].Reload();
        status.isReload = false;
    }

    void Attack()
    {
        status.attackDelay -= Time.deltaTime;

        if (stats.weapon == 0)
            return;

        if (weaponController.weaponList[stats.weapon].ammo == 0)
            return;

        status.isAttackReady = status.attackDelay <= 0;

        if (aDown && !status.isFlinch && !status.isAttack && !status.isDodge && status.isAttackReady && !status.isSkill && !status.isSkillHold)
        {
            status.isAttack = true;

            // 공격 방향
            // 현재 마우스 위치가 아닌
            // 클릭 한 위치로
            weaponController.Use(status.mousePos);

            AudioManager.instance.SFXPlay("attack_sword");

            // 다음 공격까지 대기 시간 = 1 / 초당 공격 횟수
            status.attackDelay = weaponController.weaponList[stats.weapon].SPA / stats.attackSpeed;

            // 공격 준비 안됨
            status.isAttackReady = false;

            // 공격 시간(움직이기까지 대기 시간) = (선딜레이 * 공격 중인 시간) / 초당 공격 속도
            Invoke("AttackOut", (weaponController.weaponList[stats.weapon].preDelay + weaponController.weaponList[stats.weapon].rate) / stats.attackSpeed);
        }
    }

    void AttackOut()
    {
        status.isAttack = false;
    }
    
    #endregion

    #region Skill

    void SkillDown()
    {
        if (stats.skill[status.skillIndex] == 0)
            return;

        if (skillController.skillList[stats.skill[status.skillIndex]].skillCoolTime > 0)
            return;

        // 스킬 키 다운
        if (skDown && !status.isFlinch && !status.isAttack && !status.isDodge && !status.isSkill)
        {
            //스킬이 제한이 있는 상태에서 적절한 무기가 가지고 있지 않을 때
            if (skillController.skillList[stats.skill[status.skillIndex]].skillLimit.Length != 0 && 
            Array.IndexOf(skillController.skillList[stats.skill[status.skillIndex]].skillLimit, weaponController.weaponList[stats.weapon].weaponType) == -1)
            {
                return;
            }
            skillController.SkillDown();
        }

    }

    void SkillUp()
    {
        if (stats.skill[status.skillIndex] == 0)
            return;

        //스킬 hold 상태에서 스킬 키 up
        if ((status.isFlinch || skUp) && !status.isAttack && !status.isDodge && !status.isSkill && status.isSkillHold)
        {
            StartCoroutine(skillController.Exit());
        }
    }

    #endregion

    void Interaction()
    {
        if(nearObject == null)
            return;

        if (iDown && !status.isFlinch && !status.isDodge && !status.isAttack && !status.isSkill)
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
            if (stats.weapon != 0)
            {
                weaponController.UnEquipWeapon();
            }
            // 무기 장비
            weaponController.EquipWeapon(selectItem.GetComponent<Weapon>().equipmentId);
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
            if (stats.skill[status.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // 스킬 장착
            skillController.EquipSkill(selectItem.GetComponent<Skill>().skillID);
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

        Destroy(selectItem.gameObject);
    }

    void UseItem()
    {
        if (siDown && playerItem != null)
        {
            Debug.Log("UseSelectItem");
            //Throwing Items
            if (playerItem.GetComponent<SelectItem>().selectItemClass == SelectItemClass.ThrowWeapon)
            { weaponController.UseItem(playerItem, status.mousePos); }
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
        stats.equipments[index].Equip(this.gameObject.GetComponent<Player>());

        stats.equipments[index].transform.parent = this.transform;
        stats.equipments[index].gameObject.SetActive(false);
        
        MapUIManager.instance.UpdateEquipmentUI();
    }

    public void UnEquipEquipment(int index)
    {
        if(stats.equipments[index] == null)
            return;
        stats.equipments[index].gameObject.transform.position = gameObject.transform.position;
        stats.equipments[index].transform.parent = null;
        stats.equipments[index].gameObject.SetActive(true);

        // 장비 능력치 해제
        stats.equipments[index].UnEquip(this.gameObject.GetComponent<Player>());

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
                weaponController.EquipWeapon(Instantiate(DataManager.instance.gameData.weaponList[playerWeapon]).GetComponent<Weapon>().equipmentId);
            }
            // 스킬
            if (playerSkill != 0)
            {
                skillController.EquipSkill(Instantiate(DataManager.instance.gameData.skillList[playerSkill]).GetComponent<Skill>().skillID);
            }
            // 방어구
            
            for(int i = 0;i< playerEquipment.Length; i++)
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
        if (other.tag == "EnemyAttack")
        {
            if(status.isInvincible)
                return;
            // 적에게 공격 당할시
            // 피해를 입고
            // 뒤로 밀려나며
            // 잠시 무적이 된다.
            EnemyAttack(other.gameObject);

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

    //적에게 피격
    public void EnemyAttack(GameObject attacker)
    {
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();
        Damaged(hitDetection.damage);
        Flinch(0.3f);
        KnockBack(attacker.gameObject, hitDetection.knockBack);
        Invincible(0.3f);
    }

    // 피해
    public void Damaged(float damage)
    {
        if (status.isInvincible)
        {
            return;
        }

        stats.HP -= damage * (1f - stats.defensivePower);

        MapUIManager.instance.UpdateHealthUI();
        
        if(stats.HP >= stats.HPMax)
        {
            stats.HP = stats.HPMax;
        }
        else if(stats.HP < 0)
        {
            Dead();
        }
    }

    // 뒤로 밀려남
    public void KnockBack(GameObject agent, float distance = 10)
    {
        if (status.isInvincible)
        {
            return;
        }

        Vector2 dir = (transform.position - agent.transform.position).normalized;

        rigid.AddForce(dir * (distance * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    // 경직됨(움직일 수 없음)
    public void Flinch(float time)
    {
        if (status.isInvincible)
        {
            return;
        }

        status.isFlinch = true;
        Invoke("FlinchOut",time);
    }

    void FlinchOut()
    {
        status.isFlinch = false;
    }

    // 무적(피해, 뒤로 밀려남, 경직 무시)
    public void Invincible(float time)
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
