using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

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

        if (status.isAttack || status.isReload || status.isSkill)       // ����
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
        if (dDown && !status.isAttack && !status.isSkill && moveVec != Vector2.zero && !status.isDodge && !status.isSkillHold)
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
            //���� �ð� = ���� ���� �ð� / �÷��̾� ���� �ӵ�
            float reloadTime = stats.weapon.reloadTime / stats.attackSpeed;
            Invoke("ReloadOut", reloadTime);
        }

        if (aDown && status.attackDelay < 0 && !status.isDodge && !status.isReload && !status.isAttack && !status.isSkill && !status.isSkillHold && stats.weapon.ammo == 0)
        {
            status.isReload = true;
            //���� �ð� = ���� ���� �ð� / �÷��̾� ���� �ӵ�
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

            // ���� ����
            // ���� ���콺 ��ġ�� �ƴ�
            // Ŭ�� �� ��ġ��
            WeaponController.Use(status.mousePos);
            // �ʴ� ���� Ƚ�� = �÷��̾� ���� * ���� ����
            float attackRate = stats.weapon.attackSpeed * stats.attackSpeed;
            AudioManager.instance.SFXPlay("attack_sword");
            // ���� ���ݱ��� ��� �ð� = 1 / �ʴ� ���� Ƚ��
            status.attackDelay = (stats.weapon.preDelay + stats.weapon.rate + stats.weapon.postDelay) / attackRate;
            // ���� �غ� �ȵ�
            status.isAttackReady = false;
            // ���� �ð�(�����̱���� ��� �ð�) = (�������� * ���� ���� �ð�) / �ʴ� ���� �ӵ�
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
            Debug.Log("���� ����");
            return;
        }

        if (stats.skill.skillLimit == SkillLimit.Shot &&  stats.weapon.weaponType < 10)
        {
            Debug.Log("���Ÿ� ���� ��ų");
            return;
        }

        if (stats.skill.skillLimit == SkillLimit.Melee && 10 <= stats.weapon.weaponType)
        {
            Debug.Log("�ٰŸ� ���� ��ų");
            return;
        }

        // ��ų Ű �ٿ�
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

        // ��ų �غ� ���¿��� ���� Ű �ٿ�
        if (aDown && !status.isAttack && !status.isDodge && !status.isSkill && status.isSkillReady)
        {
            StartCoroutine(skillController.Immediate());
        }
    }

    void HoldOut()
    {
        if (stats.skill == null)
            return;

        //��ų hold ���¿��� ��ų Ű up
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
            // ���� ���
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
            // ��ų ����
            skillController.EquipSkill(selectItem.GetComponent<Skill>());
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

        // ��� �ɷ�ġ ����
        stats.equipments[index].UnEquip();

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
            int playerMaxEquipment = DataManager.instance.userData.playerMaxEquipment;
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
                WeaponController.EquipWeapon(Instantiate(DataManager.instance.gameData.weaponList[playerWeapon]).GetComponent<Weapon>());
            }
            // ��ų
            if (playerSkill != 0)
            {
                skillController.EquipSkill(Instantiate(DataManager.instance.gameData.skillList[playerSkill]).GetComponent<Skill>());
            }
            // ��
            
            for(int i = 0;i< playerMaxEquipment; i++)
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
        if (other.tag == "Enemy" || other.tag == "EnemyAttack")
        {
            // ������ ���� ���ҽ�
            // ���ظ� �԰�
            // �ڷ� �з�����
            // ��� ������ �ȴ�.

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
    public void Damaged(float damage)
    {
        if (status.isInvincible)
        {
            damage = 0;
            return;
        }

        //�޴� ���� = ���� �� ���� * �÷��̾� ���� ������
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
        //ƨ�ܳ���
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
