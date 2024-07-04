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
    public PlayerStatus status { get; private set; }                        // player ���� �ɷ�ġ
    public PlayerStats playerStats {get; private set; }                     // player ���� �ɷ�ġ
    public UserData userData { get; private set; }

    [HideInInspector] public float hAxis;
    [HideInInspector] public float vAxis;

    #region Key Input

    public bool rDown { get; private set; }                                 // ������
    public bool dDown { get; private set; }                                 // ȸ��
    public bool aDown { get; private set; }                                 // ����
    public bool siDown { get; private set; }                                // ���� ������ H
    public bool iDown { get; private set; }                                 // ��ȣ�ۿ�

    public float skcDown { get; private set; }                              // ��ų ����
    public bool skDown { get; private set; }                                // ��ų Ű �ٿ� ��

    #endregion

    public LayerMask layerMask;             //���� �Ұ��� ���̾� ����
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
        // �������� ����� �����ϸ� Null, �������� ���ع� Return
        RaycastHit2D hit;

        // �⺻ �ӵ� = �÷��̾� �̵��ӵ� * �÷��̾� ����Ʈ �̵��ӵ�
        playerPosition = transform.position;
        Vector2 end= 
            playerPosition + 
            new Vector2(playerPosition.x * playerStats.moveSpeed, playerPosition.y * playerStats.moveSpeed);
        

        // ������ �߻� (����, ��, ���̾��ũ)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);
        Debug.DrawRay(playerPosition, end, Color.blue);


        // ������ �������� �������� �ʰ� ó��
        if (hit.transform == null) {  return true;   }
        return false;
    }

    void Move()     //�̵�
    {
        if( isFlinch )
            return;

        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (isAttack || status.isSkill)       // ����
        {
            moveVec = Vector2.zero;
        }
        
        if (status.isDodge)             // ȸ�ǽ� ���� �ӵ� ����
        {
            rigid.velocity  = dodgeVec * playerStats.moveSpeed * (1 + playerStats.dodgeSpeed);
        }
        else
        {
            // �⺻ �ӵ� = �÷��̾� �̵��ӵ� * �÷��̾� ����Ʈ �̵��ӵ�
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
 
    void Dodge()    // ȸ��
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

    void DodgeOut() // ȸ�� ����������
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
            // ���� �غ� �ȵ�
            isAttackReady = false;
            isAttack = true;

            weaponController.Use(status.mousePos);

            AudioManager.instance.SFXPlay("attack_sword");

            // ���� ���ݱ��� ��� �ð� = 1 / �ʴ� ���� Ƚ��
            status.attackDelay = weaponController.weaponList[playerStats.weapon].SPA / playerStats.attackSpeed;

            // ���� �ð�(�����̱���� ��� �ð�) = (�������� * ���� ���� �ð�) / �ʴ� ���� �ӵ�
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

        // ��ų Ű �ٿ�
        if (skDown && !isFlinch && !isAttack && !status.isDodge && !status.isSkill && !status.isSkillHold )
        {
            //��ų�� ������ �ִ� ���¿��� ������ ���Ⱑ ������ ���� ���� ��
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

        //��ų hold ���¿��� ��ų Ű up
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

        //����////////////////////////////////////////////////////////////////////
        if (selectItem.selectItemClass == SelectItemClass.Weapon)
        {
            if (playerStats.weapon != 0)
            {
                weaponController.UnEquipWeapon();
            }
            // ���� ���
            gainItem = weaponController.EquipWeapon(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        //����/////////////////////////////////////////////////////////////////////
        else if (selectItem.selectItemClass == SelectItemClass.Equipments)
        {
            gainItem = EquipEquipment(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        //��ų/////////////////////////////////////////////////////////////////////
        else if (selectItem.selectItemClass == SelectItemClass.Skill)
        {

            if (playerStats.skill[status.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // ��ų ����
            gainItem = skillController.EquipSkill(selectItem.GetComponent<SelectItem>().selectItemID);

        }
        //�Ϲ� ������/////////////////////////////////////////////////////////////////
        else if(selectItem.selectItemClass == SelectItemClass.Consumable && selectItem.GetComponent<HPPortion>()!=null)
        {
            /*
            //���� ������ �ִ� ������ ���
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //������ ����
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


    // ������ ����� index
    public bool EquipEquipment(int equipmentId)
    {
        bool equipOK = false;

        for(int i = 0 ; i < playerStats.equipments.Length ; i++)
        {
            // �ߺ� ���� �Ұ�
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

    // ���� ������ ��� �� ������ index
    public bool UnEquipEquipment(int index)
    {
        if (playerStats.equipments[index] == 0)
            return false;

        // ���� ��ġ�� ��� ���´�.
        Instantiate(GameData.instance.equipmentList[playerStats.equipments[index]], gameObject.transform.position, gameObject.transform.localRotation);

        // ���� �ɷ�ġ ����
        equipmentList[playerStats.equipments[index]].UnEquip(this.gameObject.GetComponent<Player>());
        equipmentList[playerStats.equipments[index]].gameObject.SetActive(false);

        // ���� ����
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
            //Scene reload �Ŀ��� ���� ���� ������ ����
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
            //������
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

            // ����
            if (playerWeapon != 0)
            {
                weaponController.EquipWeapon(playerWeapon);
            }
            // ��ų
            if (playerSkill != 0)
            {
                skillController.EquipSkill(playerSkill);
            }
            // ��
            
            for(int i = 0;i< playerEquipment.Length; i++)
            {
                if(playerEquipment[i] != 0)
                    EquipEquipment(playerEquipment[i]);
            }
            
            
        }

    }

    // ������ ��
    // ���� �������� ����ż� �̻���
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
        //���ݹ���
        if (other.tag == "EnemyAttack" || other.tag == "AllAttack")
        {
            // ������ ���� ���ҽ�
            // ���ظ� �԰�
            // �ڷ� �з�����
            // ��� ������ �ȴ�.
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
                Destroy(other.gameObject); //���� ������Ʈ ����
                playerStats.coin++;
                MapUIManager.instance.UpdateCoinUI();
            }
            else if (item.itemClass == ItemClass.Key)
            {
                Destroy(other.gameObject); //Ű ������Ʈ ����
                playerStats.key++;
                MapUIManager.instance.UpdateKeyUI();
            }
            else if(item.itemClass == ItemClass.Heal)
            {
                Destroy(other.gameObject); //���� ������Ʈ ����
                playerStats.HP += 20f;
            }
            else if(item.itemClass == ItemClass.ExtraHealth)
            {
                Destroy(other.gameObject); //Ű ������Ʈ ����
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

    // ���� ����
    #region Effect

    /*
    //������ �ǰ�
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
    
    // ����
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

    // �ڷ� �з���
    public void KnockBack(GameObject agent, float distance = 10)
    {
        Vector2 dir = (transform.position - agent.transform.position).normalized;

        rigid.AddForce(dir * (distance * (1 - playerStats.defensivePower)), ForceMode2D.Impulse);
    }

    // ������(������ �� ����)
    public IEnumerator Flinch(float time = 0)
    {
        status.isFlinch = true;

        yield return new WaitForSeconds(time);

        status.isFlinch = false;
    }

    // ����(�� ���� ����)
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
        //���� ����
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
        // ������ �ִ� �������� üũ�Ѵ�.
        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();
        foreach (StatusEffect buff in playerStats.activeEffects)
        {
            // ������ �ִ� ������� �����Ѵ�.
            if (buff.buffId == statusEffect.buffId)
            {
                buff.ResetEffect();
                return;
            }
        }
        
        // ������ �ִ� ������ �ƴ϶�� ���� �߰��Ѵ�.
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
