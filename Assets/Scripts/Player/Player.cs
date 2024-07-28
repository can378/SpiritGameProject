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

    WeaponController weaponController;
    SkillController skillController;

    [field: SerializeField] public Skill[] skillList { get; private set; }
    [field: SerializeField] public Weapon[] weaponList {get; private set;}
    [field: SerializeField] GameObject[] meleeEffectList;
    [field: SerializeField] public Equipment[] equipmentList { get; private set; }

    protected override void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        status = GetComponent<PlayerStatus>();
        stats = playerStats = GetComponent<PlayerStats>();
        
        weaponController = gameObject.AddComponent<Player.WeaponController>();
        skillController = gameObject.AddComponent<Player.SkillController>();
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

        HealPoise();
        Turn();
        //Run();
        Dodge();
        Move();
        //UseItem();
        Interaction();

        if (status.isAttackable)
        {
            Reload();
            ReloadOut();
            Attack();
            SkillUp();
            SkillDown();
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

    #region Reload

    void Reload()
    {
        if (playerStats.weapon == 0)
            return;

        if (weaponList[playerStats.weapon].maxAmmo < 0)
            return;

        if (weaponList[playerStats.weapon].maxAmmo == weaponList[playerStats.weapon].ammo)
            return;

        if (rDown && !isFlinch && !status.isDodge && !status.isReload && !isAttack && !status.isSkill && !status.isSkillHold)
        {
            status.isReload = true;
            status.reloadDelay = 0f;
        }

        if (aDown && !isFlinch && status.attackDelay < 0 && weaponList[playerStats.weapon].ammo == 0 && !status.isDodge && !status.isReload && !isAttack && !status.isSkill && !status.isSkillHold)
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

        if(status.reloadDelay >= weaponList[playerStats.weapon].reloadTime)
        {
            weaponList[playerStats.weapon].Reload();
            status.isReload = false;
        }
    }

    #endregion

    #region Attack

    [System.Serializable]
    class WeaponController : MonoBehaviour
    {
        Player player;
        // ���� ����
        Coroutine attackCoroutine;
        int enchant;
        GameObject HitDetectionGameObject;
        int projectileIndex;
        void Awake()
        {
            player = GetComponent<Player>();
        }

        // ���⸦ ȹ��
        public bool EquipWeapon(int weaponID)
        {
            // ���� ����
            player.playerStats.weapon = weaponID;
            // ��� �ɷ�ġ ����
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(true);
            player.weaponList[player.playerStats.weapon].Equip(this.gameObject.GetComponent<Player>());

            if (player.weaponList[player.playerStats.weapon].weaponType < 10)
            {
                HitDetectionGameObject = player.meleeEffectList[player.weaponList[player.playerStats.weapon].weaponType];
            }
            else if (10 <= player.weaponList[player.playerStats.weapon].weaponType)
            {
                projectileIndex = player.weaponList[player.playerStats.weapon].projectileIndex;
            }

            // ��� UI ����
            //MapUIManager.instance.UpdateWeaponUI();
            return true;
        }

        public void UnEquipWeapon()
        {
            HitDetectionGameObject = null;
            projectileIndex = -1;

            // ���� ��ġ�� ��� ���´�.
            Instantiate(GameData.instance.weaponList[player.playerStats.weapon], gameObject.transform.position, gameObject.transform.localRotation);

            // ���� �ɷ�ġ ����
            player.weaponList[player.playerStats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(false);

            // ���� ����
            player.playerStats.weapon = 0;
            //MapUIManager.instance.UpdateWeaponUI();
        }

        public void SetEnchant(int enchantID)
        {
            enchant = enchantID;
        }

        public void Use(Vector3 clickPos)
        {
            player.weaponList[player.playerStats.weapon].ConsumeAmmo();
            if (player.weaponList[player.playerStats.weapon].weaponType < 10)
            {
                // �÷��̾� �ִϸ��̼� ����
                attackCoroutine = StartCoroutine("Swing");
            }
            else if (10 <= player.weaponList[player.playerStats.weapon].weaponType)
            {
                // �÷��̾� �ִϸ��̼� ����
                if (player.weaponList[player.playerStats.weapon].weaponType == 13)
                    attackCoroutine = StartCoroutine("Throw", clickPos);
                else
                    attackCoroutine = StartCoroutine("Shot");
            }

            //Debug.Log(playerStats.weapon.attackSpeed);
            //Debug.Log(playerStats.attackSpeed);

        }

        IEnumerator Swing()
        {
            // ���� ����
            player.isAttack = true;

            float attackAngle = player.status.mouseAngle;

            //����
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].preDelay / player.playerStats.attackSpeed);

            // ���� ����Ʈ ũ�� ����
            HitDetectionGameObject.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize, player.weaponList[player.playerStats.weapon].attackSize, 1);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetEnchant(enchant);

            // ����Ʈ ��ġ ����
            HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
            hitDetection.SetHitDetection(false, -1, player.weaponList[player.playerStats.weapon].isMultiHit, player.weaponList[player.playerStats.weapon].DPS, player.playerStats.attackPower, player.weaponList[player.playerStats.weapon].knockBack, player.playerStats.criticalChance, player.playerStats.criticalDamage, player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = this.gameObject;

            // ���� ���� 
            HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);

            // ���� ����Ʈ ����
            HitDetectionGameObject.SetActive(true);

            // ���� �ð�
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].rate / player.playerStats.attackSpeed);

            // ���� ����Ʈ ����
            HitDetectionGameObject.SetActive(false);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().index = 0;

            // ���� ���� ����
            player.isAttack = false;
        }

        IEnumerator Shot()
        {
            player.isAttack = true;

            float attackAngle = player.status.mouseAngle;
            Vector2 attackDir = player.status.mouseDir;

            // ����
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].preDelay / player.playerStats.attackSpeed);

            // ���� ����ü ����
            GameObject instantProjectile = ObjectPoolManager.instance.Get(projectileIndex);
            instantProjectile.transform.position = transform.position;
            instantProjectile.transform.rotation = transform.rotation;
            instantProjectile.GetComponent<Enchant>().SetEnchant(enchant);

            //����ü ����
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

            //bulletRigid.velocity = shotPos.up * 25;
            // ����ü ����
            hitDetection.SetHitDetection(true, player.weaponList[player.playerStats.weapon].penetrations, player.weaponList[player.playerStats.weapon].isMultiHit, player.weaponList[player.playerStats.weapon].DPS, player.playerStats.attackPower, player.weaponList[player.playerStats.weapon].knockBack, player.playerStats.criticalChance, player.playerStats.criticalDamage, player.weaponList[player.playerStats.weapon].statusEffect);
            hitDetection.user = this.gameObject;
            instantProjectile.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);  // ���� ����
            instantProjectile.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize, player.weaponList[player.playerStats.weapon].attackSize, 1);
            bulletRigid.velocity = attackDir * 10 * player.weaponList[player.playerStats.weapon].projectileSpeed;  // �ӵ� ����
            hitDetection.SetProjectileTime(player.weaponList[player.playerStats.weapon].projectileTime);

            // ���� ���� ����
            player.isAttack = false;
        }

        public void AttackCancle()
        {
            player.isAttack = false;
            player.status.attackDelay = 0;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
                if (HitDetectionGameObject != null)
                    HitDetectionGameObject.SetActive(false);
            }
        }
    }

    void Attack()
    {
        status.attackDelay -= Time.deltaTime;

        if (playerStats.weapon == 0)
            return;

        if (weaponList[playerStats.weapon].ammo == 0)
            return;

        isAttackReady = status.attackDelay <= 0;

        if (aDown && !isFlinch && !isAttack && !status.isReload && !status.isDodge && isAttackReady && !status.isSkill && !status.isSkillHold )
        {
            weaponController.Use(status.mousePos);

            AudioManager.instance.SFXPlay("attack_sword");

            // ���� ���ݱ��� ��� �ð� = 1 / �ʴ� ���� Ƚ��
            status.attackDelay = weaponList[playerStats.weapon].SPA / playerStats.attackSpeed;
        }
    }

    #endregion

    #region Skill

    [System.Serializable]
    class SkillController : MonoBehaviour
    {
        Player player;

        Coroutine skillCoroutine;

        void Awake()
        {
            player = GetComponent<Player>();
        }

        // ��ų ȹ��
        public bool EquipSkill(int skillID)
        {
            // �̹� ������ ��ų�̶��
            if (player.skillList[skillID].gameObject.activeSelf == true)
                return false;

            player.playerStats.skill[player.status.skillIndex] = skillID;
            player.skillList[skillID].gameObject.SetActive(true);
            return true;
        }

        // ��ų ����
        public void UnEquipSkill()
        {
            Instantiate(DataManager.instance.gameData.skillList[player.playerStats.skill[player.status.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
            player.skillList[player.playerStats.skill[player.status.skillIndex]].gameObject.SetActive(false);
            player.playerStats.skill[player.status.skillIndex] = 0;
        }

        // ��ųŰ �Է�
        public void SkillDown()
        {
            skillCoroutine = StartCoroutine(Enter());
        }

        public void SkillUp()
        {
            skillCoroutine = StartCoroutine(Exit());
        }

        IEnumerator Enter()
        {
            print("Enter");
            // Ȧ�� ��
            player.status.isSkillHold = true;

            if (player.skillList[player.playerStats.skill[player.status.skillIndex]].skillType == 0)
            {
                player.status.isSkill = true;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.status.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.status.skillIndex]].Enter(gameObject);

            if (player.skillList[player.playerStats.skill[player.status.skillIndex]].skillType == 0)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.status.skillIndex]].postDelay);
                player.status.isSkill = false;
            }

            skillCoroutine = StartCoroutine(Stay());

            yield return null;          // �� ������ �ڷ�ƾ ������ �ȵ� yield return�� ������ �ڷ�ƾ���� ��� ���ϴ� ��?
        }

        IEnumerator Stay()
        {
            print("Stay");
            float timer = player.skillList[player.playerStats.skill[player.status.skillIndex]].maxHoldTime;

            while (player.status.isSkillHold)
            {
                yield return new WaitForSeconds(0.1f);
                timer -= 0.1f;
                if (timer <= 0)
                {
                    skillCoroutine = StartCoroutine(Exit());
                    break;
                }
            }

            yield return null;          // �� ������ �ڷ�ƾ ������ �ȵ�
        }

        IEnumerator Exit()
        {
            player.status.isSkillHold = false;

            if (player.skillList[player.playerStats.skill[player.status.skillIndex]].skillType == 2)
            {
                player.status.isSkill = true;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.status.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.status.skillIndex]].Exit();

            skillCoroutine = null;

            if (player.skillList[player.playerStats.skill[player.status.skillIndex]].skillType == 2)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.status.skillIndex]].postDelay);
                player.status.isSkill = false;
            }

            yield return null;      // �� ������ �ڷ�ƾ ������ �ȵ�

            skillCoroutine = null;

        }

        public void SkillCancle()
        {
            print("Cancle");
            player.status.isSkillHold = false;
            player.status.isSkill = false;
            if (skillCoroutine != null) 
            {
                StopCoroutine(skillCoroutine);
                skillCoroutine = null;
                if (player.playerStats.skill[player.status.skillIndex] != 0)
                    player.skillList[player.playerStats.skill[player.status.skillIndex]].Cancle();
            }

        }

    }

    void SkillDown()
    {
        if (playerStats.skill[status.skillIndex] == 0)
            return;

        if (skillList[playerStats.skill[status.skillIndex]].skillCoolTime > 0)
            return;

        // ��ų Ű �ٿ�
        if (skDown && !isFlinch && !isAttack && !status.isDodge && !status.isSkill && !status.isSkillHold )
        {
            //��ų�� ������ �ִ� ���¿��� ������ ���Ⱑ ������ ���� ���� ��
            if (playerStats.weapon == 0 && 
                skillList[playerStats.skill[status.skillIndex]].skillLimit.Length != 0 && 
            Array.IndexOf(skillList[playerStats.skill[status.skillIndex]].skillLimit, weaponList[playerStats.weapon].weaponType) == -1)
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
        if ((!skDown)&& !isFlinch && !isAttack && !status.isDodge && !status.isSkill && status.isSkillHold)
        {
            skillController.SkillUp();
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
        print("Load Data");
        if (scene.name != "Main")
        {
            //playerStats.level = DataManager.instance.userData.playerLevel;
            //playerStats.exp = DataManager.instance.userData.playerExp;
            //playerStats.point = DataManager.instance.userData.playerPoint;

            playerStats.HP = DataManager.instance.userData.playerHP;
            playerStats.tempHP = DataManager.instance.userData.playerTempHP;

            //Debug.Log("Scene reloaded: " + scene.name);
            //Scene reload �Ŀ��� ���� ���� ������ ����
            //int playerItemName = DataManager.instance.userData.playerItem;

            // ����
            int playerWeapon = DataManager.instance.userData.playerWeapon;
            if (playerWeapon != 0)
            {
                weaponController.EquipWeapon(playerWeapon);
            }

            // ��ų
            playerStats.maxSkillSlot = DataManager.instance.userData.playerMaxSkillSlot;
            int[] playerSkill = DataManager.instance.userData.playerSkill;
            for (int i = 0; i < playerSkill.Length; i++)
            {
                if (playerSkill[i] != 0)
                    skillController.EquipSkill(playerSkill[i]);
            }

            // ��
            playerStats.maxEquipment = DataManager.instance.userData.playerMaxEquipments;
            int[] playerEquipment = DataManager.instance.userData.playerEquipments;
            for (int i = 0; i < playerEquipment.Length; i++)
            {
                if (playerEquipment[i] != 0)
                    EquipEquipment(playerEquipment[i]);
            }

            // ���� ����
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
                print("get key");
                other.gameObject.SetActive(false); //Ű ������Ʈ ����
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

    public void SetEnchant(int enchantID)
    {
        weaponController.SetEnchant(enchantID);
    }

    public override void AttackCancle()
    {
        base.AttackCancle();
        status.attackDelay = 0;
        skillController.SkillCancle();
        weaponController.AttackCancle();
    }

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

    #endregion


}
