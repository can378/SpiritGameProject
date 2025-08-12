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
    public PlayerStatus playerStatus { get; private set; }
    public PlayerStats playerStats {get; private set; }
    public UserData userData { get; private set; }
    public PlayerAnim playerAnim { get; private set; }


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

    Vector2 playerPosition;
    Vector2 dodgeVec;

    WeaponController weaponController;
    SkillController skillController;

    [field: SerializeField] public Skill[] skillList { get; private set; }
    [field: SerializeField] public Weapon[] weaponList {get; private set;}
    [field: SerializeField] public Equipment[] equipmentList { get; private set; }

    protected override void Awake()
    {
        instance = this;
        base.Awake();

        status = playerStatus = GetComponent<PlayerStatus>();
        stats = playerStats = GetComponent<PlayerStats>();


        weaponController = gameObject.AddComponent<Player.WeaponController>();
        skillController = gameObject.AddComponent<Player.SkillController>();

        playerAnim = animGameObject.GetComponent<PlayerAnim>();
    }
    void Start()
    {
        userData = DataManager.instance.userData;
    }
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        //sprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
            GetInput();

        /*
        if (isMoveable())
        {
            Run();
            Dodge();
            Move();  
        }
        */

        SEProgress();
        HealPoise();
        
        Turn();
        //Run();
        Dodge();
        //UseItem();
        Interaction();

        if (playerStatus.isAttackable)
        {
            Reload();
            ReloadOut();
            Attack();
            SkillUp();
            SkillDown();
            SkillChange();
            //skill cool time
            if(playerStats.skill[playerStatus.skillIndex] != 0)
            {
                MapUIManager.instance.UpdateSkillCoolTime
                (skillList[playerStats.skill[playerStatus.skillIndex]].skillDefalutCoolTime,
                skillList[playerStats.skill[playerStatus.skillIndex]].skillCoolTime);
            }
           
        }
        
        string layerName = LayerMask.LayerToName(gameObject.layer);
        //Debug.Log("My layer name is: " + layerName);
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Reload");
        dDown = Input.GetButtonDown("Dodge");
        aDown = Input.GetButton("Attack");
        iDown = Input.GetButtonDown("Interaction"); //f down
        siDown = Input.GetButtonDown("SelectItem"); //h down

        skcDown = Input.GetAxisRaw("Mouse ScrollWheel");
        skDown = Input.GetButton("Skill");          //e Down
        
        /*
        if (hAxis > 0) { playerAnim.rightPressed = true; }
        else if (hAxis < 0) { playerAnim.leftPressed = true; }
        else
        { 
            playerAnim.rightPressed = false; 
            playerAnim.leftPressed = false; 
        }

        if (vAxis > 0) { playerAnim.upPressed = true; }
        else if (vAxis < 0) { playerAnim.downPressed = true; }
        else
        { 
            playerAnim.upPressed = false; 
            playerAnim.downPressed = false; 
        }
        */
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
            new Vector2(playerPosition.x * playerStats.MoveSpeed.Value, playerPosition.y * playerStats.MoveSpeed.Value);
        

        // ������ �߻� (����, ��, ���̾��ũ)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);
        Debug.DrawRay(playerPosition, end, Color.blue);


        // ������ �������� �������� �ʰ� ó��
        if (hit.transform == null) {  return true;   }
        return false;
    }

    void Move()     //�̵�
    {
        if(0 < playerStatus.isFlinch)
            return;

        playerStatus.moveVec = new Vector2(hAxis, vAxis).normalized;

        if (playerStatus.isAttack || playerStatus.isSkill)       // ����
        {
            playerStatus.moveVec = Vector2.zero;
        }
        
        if (playerStatus.isDodge)             // ȸ�ǽ� ���� �ӵ� ����
        {
            rigid.velocity  = dodgeVec.normalized * playerStats.MoveSpeed.Value * (1 + playerStats.dodgeSpeed) * status.moveSpeedMultiplier;
        }
        else
        {
            // �⺻ �ӵ� = �÷��̾� �̵��ӵ� * �÷��̾� ����Ʈ �̵��ӵ� * �߰� �̵� �ӵ�
            rigid.velocity = playerStatus.moveVec * playerStats.MoveSpeed.Value * status.moveSpeedMultiplier;
        }
    }

    void Turn()
    {
        playerStatus.mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerStatus.mouseDir = (Vector2)(playerStatus.mousePos - CenterPivot.position).normalized;

        playerStatus.mouseAngle = Mathf.Atan2(playerStatus.mousePos.y - CenterPivot.position.y, playerStatus.mousePos.x - CenterPivot.position.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(mouseAngle - 90, Vector3.forward);

    }
 
    void Dodge()    // ȸ��
    {
        playerAnim.animator.SetBool("isDodge", playerStatus.isDodge);
        
        if (playerStatus.moveVec == Vector2.zero)
            return;
        
        if (dDown && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isDodge && !playerStatus.isSkillHold)
        {
            dodgeVec = playerStatus.moveVec;
            playerStatus.isDodge = true;
            playerStatus.isReload = false;
            //playerStats.HP += 100;
            //playerStats.MoveSpeed.AddValue = 20;

            Invoke("DodgeOut", playerStats.dodgeTime);

        }
    }

    void DodgeOut() // ȸ�� ����������
    {
        playerStatus.isDodge = false;
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


        playerAnim.animator.SetBool("isReload", playerStatus.isReload);
        playerAnim.animator.SetFloat("ReloadTime", playerStatus.reloadDelay / weaponList[playerStats.weapon].reloadTime);

        if (weaponList[playerStats.weapon].maxAmmo < 0)
            return;

        if (weaponList[playerStats.weapon].maxAmmo == weaponList[playerStats.weapon].ammo)
            return;

        if (rDown && (0 >= playerStatus.isFlinch) && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
            playerAnim.animator.SetInteger("AttackType", (int)weaponList[playerStats.weapon].weaponType);
        }

        if (aDown && (0 >= playerStatus.isFlinch) && playerStatus.attackDelay < 0 && weaponList[playerStats.weapon].ammo == 0 && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
            playerAnim.animator.SetInteger("AttackType", (int)weaponList[playerStats.weapon].weaponType);
        }
    }

    void ReloadOut()
    {
        if (!playerStatus.isReload)
            return;

        playerStatus.reloadDelay += Time.deltaTime * (playerStats.attackSpeed);

        if(playerStatus.reloadDelay >= weaponList[playerStats.weapon].reloadTime)
        {
            weaponList[playerStats.weapon].Reload();
            playerStatus.isReload = false;
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
        [field: SerializeField] public List<SE_TYPE> SEType { get; set; } = new List<SE_TYPE>();

        [field: SerializeField] public List<COMMON_TYPE> CommonType { get; set; } = new List<COMMON_TYPE>();

        [field: SerializeField] public List<PROJECTILE_TYPE> ProjectileType { get; set; } = new List<PROJECTILE_TYPE>();
        [SerializeField] GameObject HitDetectionGameObject;
        GameObject projectile;

        public float ComboTimer = 0;
        public int AttackCombo = 0;

        void Awake()
        {
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (0 < AttackCombo)
            {
                ComboTimer += Time.deltaTime;
                if (2 < ComboTimer)
                {
                    AttackCombo = 0;
                    ComboTimer = 0;
                }
            }

        }

        // ���⸦ ȹ��
        public bool EquipWeapon(int weaponID)
        {
            // ���� ����
            player.playerStats.weapon = weaponID;
            // ��� �ɷ�ġ ����
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(true);
            player.weaponList[player.playerStats.weapon].Equip(this.gameObject.GetComponent<Player>());

            if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.MELEE)
            {
                HitDetectionGameObject = player.hitEffects[(int)player.weaponList[player.playerStats.weapon].SwingEffectType];
            }
            else if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.RANGE)
            {
                projectile = player.weaponList[player.playerStats.weapon].projectile;
            }

            // ��� UI ����
            //MapUIManager.instance.UpdateWeaponUI();
            return true;
        }

        public void UnEquipWeapon()
        {
            HitDetectionGameObject = null;
            projectile = null;

            // ���� ��ġ�� ��� ���´�.
            Instantiate(GameData.instance.weaponList[player.playerStats.weapon], player.CenterPivot.transform.position, gameObject.transform.localRotation);

            // ���� �ɷ�ġ ����
            player.weaponList[player.playerStats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(false);

            // ���� ����
            player.playerStats.weapon = 0;
            //MapUIManager.instance.UpdateWeaponUI();
        }

        public void Use()
        {
            
            player.weaponList[player.playerStats.weapon].ConsumeAmmo();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[player.weaponList[player.playerStats.weapon].weaponType.ToSafeString()];
            AttackCombo = animationInfo.Animation.Count <= AttackCombo + 1 ? 0 : (AttackCombo + 1);
            ComboTimer = 0;

            if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.MELEE)
            {
                // �÷��̾� �ִϸ��̼� ����
                attackCoroutine = StartCoroutine(Swing(player.playerStatus.mouseDir, player.playerStatus.mouseAngle));
            }
            else if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.RANGE)
            {
                attackCoroutine = StartCoroutine(Shot(player.playerStatus.mouseDir, player.playerStatus.mouseAngle));
            }

            

            //Debug.Log(playerStats.weapon.attackSpeed);
            //Debug.Log(playerStats.attackSpeed);

        }

        IEnumerator Swing(Vector2 _AttackDir, float _AttackAngle)
        {

            Weapon CurWeapon = player.weaponList[player.playerStats.weapon];
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[player.weaponList[player.playerStats.weapon].weaponType.ToSafeString()];

            // ���� ����
            player.playerStatus.isAttack = true;

            // �ִϸ��̼� ����
            player.playerAnim.ChangeDirection(_AttackDir);
            player.playerAnim.ChangeWeaponSprite(CurWeapon.weaponType, CurWeapon.selectItemID);
            player.playerAnim.animator.Rebind();
            player.playerAnim.animator.SetBool("isAttack", true);
            player.playerAnim.animator.SetInteger("AttackType", (int)player.weaponList[player.playerStats.weapon].weaponType);
            player.playerAnim.animator.SetInteger("AttackCombo", AttackCombo);
            player.playerAnim.animator.SetFloat("AttackSpeed", player.playerStats.attackSpeed);


            AudioManager.instance.WeaponAttackAudioPlay(player.weaponList[player.playerStats.weapon].weaponType);

            //����
            yield return new WaitForSeconds(animationInfo.PreDelay / player.playerStats.attackSpeed);

            // ���� ���⸸ �켱 ����
            int SwingDir = animationInfo.Animation[AttackCombo].SwingDir;
            if (player.weaponList[player.playerStats.weapon].weaponType == WEAPON_TYPE.SWORD ||
            player.weaponList[player.playerStats.weapon].weaponType == WEAPON_TYPE.SHORT_SWORD ||
            player.weaponList[player.playerStats.weapon].weaponType == WEAPON_TYPE.LONG_SWORD ||
            player.weaponList[player.playerStats.weapon].weaponType == WEAPON_TYPE.SCYTHE)
            {
                SwingDir = 0 <= _AttackDir.x ? SwingDir : -SwingDir;
            }

            // ���� ����Ʈ ũ�� ����
            HitDetectionGameObject.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize * SwingDir, player.weaponList[player.playerStats.weapon].attackSize, 1);
        
        
            // ����Ʈ ��ġ ����
            HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
            hitDetection.SetHit_Ratio(
            0, 1, player.playerStats.AttackPower,
            player.weaponList[player.playerStats.weapon].knockBack, 
            player.playerStats.CriticalChance, 
            player.playerStats.CriticalDamage);
            hitDetection.user = player;
            hitDetection.SetMultiHit(player.weaponList[player.playerStats.weapon].isMultiHit, player.weaponList[player.playerStats.weapon].DPS);

            // ��æƮ ����
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetSE(SEType.Count == 0 ? SE_TYPE.NONE : SEType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetCommon(CommonType.Count == 0 ? COMMON_TYPE.NONE : CommonType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetProjectile(ProjectileType.Count == 0 ? PROJECTILE_TYPE.NONE : ProjectileType[0]);

            // ���� ����
            HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(_AttackAngle - 90, Vector3.forward);

            // ��ƼŬ
            {
                // ���� �ӵ��� ���� ����Ʈ ����
                ParticleSystem.MainModule particleMain = HitDetectionGameObject.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startLifetime = animationInfo.Rate / player.playerStats.attackSpeed;

            }
            
            // ���� ����Ʈ ����
            HitDetectionGameObject.SetActive(true);

            // ���� �ð�
            yield return new WaitForSeconds(animationInfo.Rate / player.playerStats.attackSpeed);

            // ���� ����Ʈ ����
            HitDetectionGameObject.SetActive(false);

            // ���� ���� ����
            player.playerAnim.animator.SetBool("isAttack", false);
            player.playerStatus.isAttack = false;
        }

        IEnumerator Shot(Vector2 _AttackDir, float _AttackAngle)
        {
            Weapon CurWeapon = player.weaponList[player.playerStats.weapon];

            player.playerStatus.isAttack = true;

            // �ִϸ��̼� ����
            player.playerAnim.ChangeDirection(_AttackDir);
            Vector3 ShotPos = player.hitEffects[(int)player.weaponList[player.playerStats.weapon].ShotPosType].transform.position;
            Vector3 ShotDir = (player.playerStatus.mousePos - ShotPos).normalized;
            float ShotAngle = Mathf.Atan2(player.playerStatus.mousePos.y - ShotPos.y, player.playerStatus.mousePos.x - ShotPos.x) * Mathf.Rad2Deg;

            player.playerAnim.ChangeWeaponSprite(CurWeapon.weaponType, CurWeapon.selectItemID);
            player.playerAnim.animator.Rebind();
            player.playerAnim.animator.SetBool("isAttack", true);
            player.playerAnim.animator.SetInteger("AttackType", (int)player.weaponList[player.playerStats.weapon].weaponType);
            player.playerAnim.animator.SetInteger("AttackCombo", AttackCombo);
            player.playerAnim.animator.SetFloat("AttackSpeed", player.playerStats.attackSpeed);
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[player.weaponList[player.playerStats.weapon].weaponType.ToSafeString()];

            // ����
            yield return new WaitForSeconds(animationInfo.PreDelay / player.playerStats.attackSpeed);
            AudioManager.instance.WeaponAttackAudioPlay(player.weaponList[player.playerStats.weapon].weaponType);

            // ���� ����ü ����

            GameObject instantProjectile = ObjectPoolManager.instance.Get(projectile, ShotPos);

            //����ü ����
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

            // ����ü ����
            hitDetection.SetProjectile_Ratio(player.weaponList[player.playerStats.weapon].penetrations
                ,0
                ,1
                , player.playerStats.AttackPower
                , player.weaponList[player.playerStats.weapon].knockBack
                , player.playerStats.CriticalChance
                , player.playerStats.CriticalDamage);
            hitDetection.SetMultiHit(player.weaponList[player.playerStats.weapon].isMultiHit, player.weaponList[player.playerStats.weapon].DPS);
            hitDetection.user = player;


            // ��æƮ
            instantProjectile.GetComponentInChildren<Enchant>().SetSE(SEType.Count == 0 ? SE_TYPE.NONE : SEType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetCommon(CommonType.Count == 0 ? COMMON_TYPE.NONE : CommonType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetProjectile(ProjectileType.Count == 0 ? PROJECTILE_TYPE.NONE : ProjectileType[0]);

            // ���� ����
            instantProjectile.transform.rotation = Quaternion.AngleAxis(ShotAngle - 90, Vector3.forward);

            // ũ�� ����
            instantProjectile.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize, player.weaponList[player.playerStats.weapon].attackSize, 1);

            // �ӵ� ����
            bulletRigid.velocity = ShotDir * 10 * player.weaponList[player.playerStats.weapon].projectileSpeed;

            // �����Ÿ� ����
            hitDetection.SetDisableTime(player.weaponList[player.playerStats.weapon].projectileTime);

            // ���� ���� ����
            player.playerAnim.animator.SetBool("isAttack", false);
            player.playerStatus.isAttack = false;
        }


        // Ȱ ���� ����
        IEnumerator Bow_Ready()
        {

            yield return null;
        }

        // Ȱ ���� ���� ��
        IEnumerator Bow_Draw()
        {
            yield return null;
        }

        // ȭ�� ���
        IEnumerator Bow_Shot()
        {
            yield return null;
        }

        public void AttackCancle()
        {
            player.playerStatus.isAttack = false;
            player.playerStatus.attackDelay = 0;
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
        playerStatus.attackDelay -= Time.deltaTime;

        if (playerStats.weapon == 0)
            return;

        if (weaponList[playerStats.weapon].ammo == 0)
            return;

        playerStatus.isAttackReady = playerStatus.attackDelay <= 0;

        if (aDown && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isReload && !playerStatus.isDodge && playerStatus.isAttackReady && !playerStatus.isSkill && !playerStatus.isSkillHold )
        {
            WeaponAnimationInfo animationInfo = playerAnim.AttackAnimationData[weaponList[playerStats.weapon].weaponType.ToSafeString()];

            float SPA = animationInfo.PostDelay + animationInfo.Rate + animationInfo.PreDelay;

            weaponController.Use();


            // ���� ���ݱ��� ��� �ð� = 1 / �ʴ� ���� Ƚ��
            playerStatus.attackDelay = SPA / playerStats.attackSpeed;
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

            player.playerStats.skill[player.playerStatus.skillIndex] = skillID;
            player.skillList[skillID].gameObject.SetActive(true);
            return true;
        }

        // ��ų ����
        public void UnEquipSkill()
        {
            Instantiate(DataManager.instance.gameData.skillList[player.playerStats.skill[player.playerStatus.skillIndex]], player.CenterPivot.transform.position, gameObject.transform.localRotation);
            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].gameObject.SetActive(false);
            player.playerStats.skill[player.playerStatus.skillIndex] = 0;
        }

        // ��ųŰ �Է�
        public void SkillDown()
        {
            skillCoroutine = StartCoroutine(Enter());
        }

        public void SkillUp()
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = StartCoroutine(Exit());
        }

        IEnumerator Enter()
        {
            //print("Enter");
            // Ȧ�� ��
            player.playerStatus.isSkillHold = true;

            // ��ų Ű �ٿ� ��� ����
            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 0)
            {
                player.playerStatus.isSkill = true;
                yield return null;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Enter(player);

            // ��ų Ű �ٿ� ��� ����
            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 0)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].postDelay);
                yield return null;
                player.playerStatus.isSkill = false;
            }

            skillCoroutine = StartCoroutine(Stay());

            yield return null;          // �� ������ �ڷ�ƾ ������ �ȵ� yield return�� ������ �ڷ�ƾ���� ��� ���ϴ� ��?
        }

        IEnumerator Stay()
        {
            //print("Stay");
            float timer = player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].maxHoldTime;

            while (player.playerStatus.isSkillHold && timer > 0)
                {
                    yield return null;          // �� ������ �ڷ�ƾ ������ �ȵ�
                    timer -= Time.deltaTime;
                    player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].HoldCoolDown();      // Ȧ�� ���� ���� ��Ÿ���� �پ���� ����
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
            player.playerStatus.isSkillHold = false;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 2)
            {
                player.playerStatus.isSkill = true;
                yield return null;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Exit();

            skillCoroutine = null;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 2)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].postDelay);
                yield return null;
                player.playerStatus.isSkill = false;
            }

            yield return null;      // �� ������ �ڷ�ƾ ������ �ȵ�

            skillCoroutine = null;

        }

        public void SkillCancle()
        {
            //print("Cancle");
            player.playerStatus.isSkillHold = false;
            player.playerStatus.isSkill = false;
            if (skillCoroutine != null) 
            {
                StopCoroutine(skillCoroutine);
                skillCoroutine = null;
                if (player.playerStats.skill[player.playerStatus.skillIndex] != 0)
                    player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Cancle();
            }
        }

    }

    void SkillDown()
    {
        if (playerStats.skill[playerStatus.skillIndex] == 0)
            return;

        if (skillList[playerStats.skill[playerStatus.skillIndex]].skillCoolTime > 0)
            return;

        // ��ų Ű �ٿ�
        if (skDown && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && !playerStatus.isSkillHold )
        {
            // ��ų�� ������ �ִ� ���¿��� ������ ���Ⱑ ������ ���� ���� ��
            // �ش� ���⸸ ����� �����ϴ�.

            // ���� ������ ���� ��
            if (skillList[playerStats.skill[playerStatus.skillIndex]].skillLimit.Length != 0)
            {
                // ���Ⱑ ���ų�
                // ���� ���⸦ ������ ���� �ʰų�
                int SkillUseOk = Array.IndexOf(skillList[playerStats.skill[playerStatus.skillIndex]].skillLimit, weaponList[playerStats.weapon].weaponType);
                print(SkillUseOk);
                if (playerStats.weapon == 0 || SkillUseOk == -1)
                {
                    return;
                }

            }
            skillController.SkillDown();
        }

    }
    
    void SkillUp()
    {
        if (playerStats.skill[playerStatus.skillIndex] == 0)
            return;

        //��ų hold ���¿��� ��ų Ű up
        if ((!skDown)&& (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && playerStatus.isSkillHold)
        {
            playerStatus.isReload = false;
            skillController.SkillUp();
            
        }
    }
    
    void SkillChange()
    {
        playerStatus.skillChangeDelay -= Time.deltaTime;

        if (skcDown != 0f && (0 >= playerStatus.isFlinch) && !playerStatus.isSkill && !playerStatus.isSkillHold && playerStatus.skillChangeDelay <= 0f)
        {
            playerStatus.skillChangeDelay = 0.1f;
            if(skcDown > 0f)
            {
                playerStatus.skillIndex = playerStatus.skillIndex + 1 > playerStats.maxSkillSlot - 1 ? playerStats.maxSkillSlot - 1 : playerStatus.skillIndex + 1;
            }
            else if(skcDown < 0f)
            {
                playerStatus.skillIndex = 0 > playerStatus.skillIndex - 1 ? 0 : playerStatus.skillIndex - 1;
            }
        }
    }

    #endregion

    #region Interaction

    void Interaction()
    {
        if(playerStatus.nearObject == null)
            return;

        if (iDown && (0 >= playerStatus.isFlinch) && !playerStatus.isDodge && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.nearObject.GetComponent<Interactable>().Interact();
        }

    }

    #endregion Interaction

    #region Item

    public void GainSelectItem(SelectItem selectItem)
    {
        //SelectItem selectItem = playerStatus.nearObject.GetComponent<SelectItem>();
        
        // ������ ȹ�� ����
        bool gainItem = false;

        // ���� =======================================================
        // ���� ���� ���� ���� �� ���� ����
        if (selectItem.selectItemType == SelectItemType.Weapon)
        {
            if (playerStats.weapon != 0)
            {
                weaponController.UnEquipWeapon();
            }
            // ���� ���
            gainItem = weaponController.EquipWeapon(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        // ���� =======================================================
        // ����ִ� ��� �������� ����
        // ����ִ� ��� ������ ���ٸ� ���� ����
        else if (selectItem.selectItemType == SelectItemType.Equipments)
        {
            gainItem = EquipEquipment(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        // ��ų =======================================================
        // ���� ��� ���� ��ų ���� �� ��ų ����
        else if (selectItem.selectItemType == SelectItemType.Skill)
        {

            if (playerStats.skill[playerStatus.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // ��ų ����
            gainItem = skillController.EquipSkill(selectItem.GetComponent<SelectItem>().selectItemID);

        }
        // �Ϲ� ������ =======================================================
        // ȹ�� ��� ��� ��
        else if (selectItem.selectItemType == SelectItemType.Consumable)
        {
            //UseItem
            selectItem.GetComponent<Consumable>().UseItem(this);
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
            { weaponController.UseItem(playerItem, playerStatus.mousePos); }
            //Consumable Item
            else 
            {
                playerItem.GetComponent<Consumable>().UseItem(this);
                Destroy(playerItem);
            }
           
            
            //"no item" playerStatus
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
        Instantiate(GameData.instance.equipmentList[playerStats.equipments[index]], CenterPivot.transform.position, gameObject.transform.localRotation);

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

            Player.instance.playerStats.HPMax                       += Player.instance.playerStats.playerStat[(int)StatID.HP] * StatIV[(int)StatID.HP];
            Player.instance.playerStats.AttackPower.IncreasedValue        += Player.instance.playerStats.playerStat[(int)StatID.AP] * StatIV[(int)StatID.AP];
            Player.instance.playerStats.increasedAttackSpeed        += Player.instance.playerStats.playerStat[(int)StatID.AS] * StatIV[(int)StatID.AS];
            Player.instance.playerStats.CriticalChance.AddValue           += Player.instance.playerStats.playerStat[(int)StatID.CC] * StatIV[(int)StatID.CC];
            Player.instance.playerStats.CriticalDamage.AddValue          += Player.instance.playerStats.playerStat[(int)StatID.CD] * StatIV[(int)StatID.CD];
            Player.instance.playerStats.SkillPower.AddValue               += Player.instance.playerStats.playerStat[(int)StatID.SP] * StatIV[(int)StatID.SP];
            Player.instance.playerStats.addSkillCoolTime            += Player.instance.playerStats.playerStat[(int)StatID.SCT] * StatIV[(int)StatID.SCT];
            Player.instance.playerStats.MoveSpeed.IncreasedValue          += Player.instance.playerStats.playerStat[(int)StatID.MS] * StatIV[(int)StatID.MS];

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

    #endregion

    #region StatLV

    // ü��, ���ݷ�, ���ݼӵ�, ġ��Ÿ Ȯ�� ,ġ��Ÿ ���ط�, ����, ���� ���ð�, �̵��ӵ�
    public enum StatID {HP, AP, AS, CC, CD, SP, SCT, MS};
    // ���� ������
    [HideInInspector]
    public readonly float[] StatIV = { 25, 0.2f, 0.2f, 0.1f, 0.05f, 10f, -0.1f, 0.1f };

    // ������ ���� ��Ų��.
    public void StatLevelUp(StatID _StatID)
    {
        Player.instance.playerStats.playerStat[(int)_StatID]++;
        switch(_StatID)
        {
            case StatID.HP:
            Player.instance.playerStats.HPMax                       += StatIV[(int)_StatID];
            break;
            case StatID.AP:
            Player.instance.playerStats.AttackPower.IncreasedValue        += StatIV[(int)_StatID];
            break;
            case StatID.AS:
            Player.instance.playerStats.increasedAttackSpeed        += StatIV[(int)_StatID];
            break;
            case StatID.CC:
            Player.instance.playerStats.CriticalChance.AddValue           += StatIV[(int)_StatID];
            break;
            case StatID.CD:
            Player.instance.playerStats.CriticalDamage.AddValue           += StatIV[(int)_StatID];
            break;
            case StatID.SP:
            Player.instance.playerStats.SkillPower.AddValue               += StatIV[(int)_StatID];
            break;
            case StatID.SCT:
            Player.instance.playerStats.addSkillCoolTime            = StatIV[(int)_StatID];
            break;
            case StatID.MS:
            Player.instance.playerStats.MoveSpeed.IncreasedValue          += StatIV[(int)_StatID];
            break;

        }
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
            BeAttacked(other.gameObject.GetComponent<HitDetection>(), other.ClosestPoint(CenterPivot.position));

        }
        else if (other.tag == "EnterDungeon")
        {
            AudioManager.instance.Bgm_normal(userData.nowChapter);

            if (userData.nowChapter < 3)
            {
                
                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("Map");
            }
            else if (userData.nowChapter == 3)
            {
                
                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("FinalMap"); 
            }
            else if(userData.nowChapter==4) 
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
            }
            else if (item.itemClass == ItemClass.Key)
            {
                print("get key");
                other.gameObject.SetActive(false); //Ű ������Ʈ ����
                playerStats.key++;
            }
            /*
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
            */
            //MapUIManager.instance.UpdateMinimapUI(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // �ش� tag�� ���� ����� ��ȣ�ۿ��� ����
        if (other.tag == "SelectItem" || other.tag == "Door" || 
        other.tag == "Npc" || other.tag=="reward" || other.tag == "SellingItem")
        {
            if(playerStatus.nearObject == null || Vector2.Distance(CenterPivot.transform.position, other.transform.position) < Vector2.Distance(CenterPivot.transform.position, playerStatus.nearObject.transform.position))
            {
                playerStatus.nearObject = other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        playerStatus.nearObject = null;
    }

    #endregion

    // ���� ����
    #region Effect

    public void AddEnchant_SE(SE_TYPE _Type)
    {
        weaponController.SEType.Insert(0, _Type);
    }

    public void AddEnchant_Common(COMMON_TYPE _Type)
    {
        weaponController.CommonType.Insert(0, _Type);
    }

    public void AddEnchant_Projectile(PROJECTILE_TYPE _Type)
    {
        weaponController.ProjectileType.Insert(0, _Type);
    }

    public void RemoveEnchant_SE(SE_TYPE _Type)
    {
        weaponController.SEType.Remove(_Type);
    }

    public void RemoveEnchant_Common(COMMON_TYPE _Type)
    {
        weaponController.CommonType.Remove(_Type);
    }

    public void RemoveEnchant_Projectile(PROJECTILE_TYPE _Type)
    {
        weaponController.ProjectileType.Remove(_Type);
    }

    public override void FlinchCancle()
    {
        base.FlinchCancle();
        playerStatus.attackDelay = 0;
        playerStatus.isReload = false;
        skillController.SkillCancle();
        weaponController.AttackCancle();
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
