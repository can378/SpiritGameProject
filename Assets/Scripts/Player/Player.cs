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
    // player ���� �ɷ�ġ
    public static Player instance { get; private set; }
    // player ���� ����
    public  PlayerStatus status { get; private set; }
    public PlayerStats stats {get; private set; }

    public float hAxis;
    public float vAxis;

    #region Key Input

    bool rDown;            //������
    bool dDown;           //ȸ��
    bool aDown;            //����
    bool siDown;           // ���� ������
    bool iDown;             //��ȣ�ۿ�

    bool skDown;
    bool skUp;

    #endregion

    public LayerMask layerMask;//���� �Ұ��� ���̾� ����
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
        // �������� ����� �����ϸ� Null, �������� ���ع� Return
        RaycastHit2D hit;

        // �⺻ �ӵ� = �÷��̾� �̵��ӵ� * �÷��̾� ����Ʈ �̵��ӵ�
        playerPosition = transform.position;
        Vector2 end= 
            playerPosition + 
            new Vector2(playerPosition.x * stats.moveSpeed, playerPosition.y * stats.moveSpeed);
        

        // ������ �߻� (����, ��, ���̾��ũ)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);


        // ������ �������� �������� �ʰ� ó��
        if (hit.transform == null) {  return true;   }
        return false;
    }

    void Move()     //�̵�
    {
        
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if (status.isAttack || status.isReload || status.isSkill || status.isFlinch)       // ����
        {
            moveVec = Vector2.zero;
        }
        if (status.isDodge)             // ȸ�ǽ� ���� �ӵ� ����
        {
            moveVec = dodgeVec;
        }
        else
        {
            // �⺻ �ӵ� = �÷��̾� �̵��ӵ� * �÷��̾� ����Ʈ �̵��ӵ�
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
 
    void Dodge()    // ȸ��
    {
        if(moveVec == Vector2.zero)
            return;
        
        if (dDown && !status.isFlinch && !status.isAttack && !status.isSkill  && !status.isDodge && !status.isSkillHold)
        {
            sprite.color = Color.cyan;
            dodgeVec = moveVec;
            // ȸ�� �ӵ� = �÷��̾� �̵��ӵ� * ȸ�Ǽӵ�
            float dodgeSpeed = stats.moveSpeed * stats.dodgeSpeed;
            rigid.velocity = moveVec * dodgeSpeed;
            status.isDodge = true;

            Invoke("DodgeOut", stats.dodgeTime);

        }
    }

    void DodgeOut() // ȸ�� ����������
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
            //������ �ɸ��� �ð� = ���� ���� �ð� / �÷��̾� ���� �ӵ�
            float reloadTime = weaponController.weaponList[stats.weapon].reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }

        if (aDown && !status.isFlinch && status.attackDelay < 0 && weaponController.weaponList[stats.weapon].ammo == 0 && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            //���� �ð� = ���� ���� �ð� / �÷��̾� ���� �ӵ�
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

            // ���� ����
            // ���� ���콺 ��ġ�� �ƴ�
            // Ŭ�� �� ��ġ��
            weaponController.Use(status.mousePos);

            AudioManager.instance.SFXPlay("attack_sword");

            // ���� ���ݱ��� ��� �ð� = 1 / �ʴ� ���� Ƚ��
            status.attackDelay = weaponController.weaponList[stats.weapon].SPA / stats.attackSpeed;

            // ���� �غ� �ȵ�
            status.isAttackReady = false;

            // ���� �ð�(�����̱���� ��� �ð�) = (�������� * ���� ���� �ð�) / �ʴ� ���� �ӵ�
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

        // ��ų Ű �ٿ�
        if (skDown && !status.isFlinch && !status.isAttack && !status.isDodge && !status.isSkill)
        {
            //��ų�� ������ �ִ� ���¿��� ������ ���Ⱑ ������ ���� ���� ��
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

        //��ų hold ���¿��� ��ų Ű up
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
            // ���� ���
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
            // ��ų ����
            skillController.EquipSkill(selectItem.GetComponent<Skill>().skillID);
        }
        else if(selectItem.selectItemClass == SelectItemClass.Consumable || selectItem.selectItemClass==SelectItemClass.ThrowWeapon  )
        {
            //���� ������ �ִ� ������ ���
            if (playerItem != null)
            { playerItem.SetActive(true); playerItem.transform.position = transform.position; }
            
            //������ ����
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
                    // �ؿ� �����۵��� ȹ�� ��÷� �ٲپ����� ���ڽ��ϴ�.
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

        // ��� �ɷ�ġ ����
        stats.equipments[index].UnEquip(this.gameObject.GetComponent<Player>());

        // ��� ����
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
            //Scene reload �Ŀ��� ���� ���� ������ ����
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

            //������
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
            // ����
            if (playerWeapon != 0)
            {
                weaponController.EquipWeapon(Instantiate(DataManager.instance.gameData.weaponList[playerWeapon]).GetComponent<Weapon>().equipmentId);
            }
            // ��ų
            if (playerSkill != 0)
            {
                skillController.EquipSkill(Instantiate(DataManager.instance.gameData.skillList[playerSkill]).GetComponent<Skill>().skillID);
            }
            // ��
            
            for(int i = 0;i< playerEquipment.Length; i++)
            {
                if(playerEquipment[i] != 0)
                    EquipEquipment(Instantiate(DataManager.instance.gameData.equipmentList[playerEquipment[i]]).GetComponent<Equipment>(),i);
            }
            
            
        }

    }

    // ������ ��
    // ���� �������� ����ż� �̻���
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
        //���ݹ���
        if (other.tag == "EnemyAttack")
        {
            if(status.isInvincible)
                return;
            // ������ ���� ���ҽ�
            // ���ظ� �԰�
            // �ڷ� �з�����
            // ��� ������ �ȴ�.
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
                Destroy(other.gameObject); //���� ������Ʈ ����
                stats.coin++;
                MapUIManager.instance.UpdateCoinUI();
            }

            if (item.itemClass == ItemClass.Key)
            {
                Destroy(other.gameObject); //Ű ������Ʈ ����
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

    // ���� ����
    #region Effect

    //������ �ǰ�
    public void EnemyAttack(GameObject attacker)
    {
        HitDetection hitDetection = attacker.GetComponent<HitDetection>();
        Damaged(hitDetection.damage);
        Flinch(0.3f);
        KnockBack(attacker.gameObject, hitDetection.knockBack);
        Invincible(0.3f);
    }

    // ����
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

    // �ڷ� �з���
    public void KnockBack(GameObject agent, float distance = 10)
    {
        if (status.isInvincible)
        {
            return;
        }

        Vector2 dir = (transform.position - agent.transform.position).normalized;

        rigid.AddForce(dir * (distance * (1 - stats.defensivePower)), ForceMode2D.Impulse);
    }

    // ������(������ �� ����)
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

    // ����(����, �ڷ� �з���, ���� ����)
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
        //���� ����
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
        // ������ �ִ� �������� üũ�Ѵ�.
        StatusEffect statusEffect = effect.GetComponent<StatusEffect>();
        foreach (StatusEffect buff in stats.activeEffects)
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
