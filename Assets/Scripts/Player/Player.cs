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
    public PlayerStats playerStats { get; private set; }
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

    [field: SerializeField] public SkillBase[] skillList { get; private set; }
    //[field: SerializeField] public Weapon[] weaponList { get; private set; }
    //[field: SerializeField] public Equipment[] equipmentList { get; private set; }

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

        Update_Buff();
        Update_Passive();
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
            if (playerStats.skill[playerStatus.skillIndex] != 0)
            {
                MapUIManager.instance.UpdateSkillCoolTime
                (skillList[playerStats.skill[playerStatus.skillIndex]].skillData.skillDefalutCoolTime,
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
        Vector2 end =
            playerPosition +
            new Vector2(playerPosition.x * playerStats.MoveSpeed.Value, playerPosition.y * playerStats.MoveSpeed.Value);


        // ������ �߻� (����, ��, ���̾��ũ)
        hit = Physics2D.Linecast(playerPosition, end, layerMask);
        Debug.DrawRay(playerPosition, end, Color.blue);


        // ������ �������� �������� �ʰ� ó��
        if (hit.transform == null) { return true; }
        return false;
    }

    void Move()     //�̵�
    {
        if (0 < playerStatus.isFlinch)
            return;

        playerStatus.moveVec = new Vector2(hAxis, vAxis).normalized;

        if (playerStatus.isAttack || playerStatus.isSkill)       // ����
        {
            playerStatus.moveVec = Vector2.zero;
        }

        if (playerStatus.isDodge)             // ȸ�ǽ� ���� �ӵ� ����
        {
            rigid.velocity = dodgeVec.normalized * playerStats.MoveSpeed.Value * (1 + playerStats.dodgeSpeed) * status.moveSpeedMultiplier;
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


        if (playerStats.weapon.weaponData == null)
            return;


        playerAnim.animator.SetBool("isReload", playerStatus.isReload);
        playerAnim.animator.SetFloat("ReloadTime", playerStatus.reloadDelay / playerStats.weapon.GetReloadTime());

        if (playerStats.weapon.GetMaxAmmo() < 0)
            return;

        if (playerStats.weapon.GetMaxAmmo() == playerStats.weapon.ammo)
            return;

        if (rDown && (0 >= playerStatus.isFlinch) && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
            playerAnim.animator.SetInteger("AttackType", (int)playerStats.weapon.weaponData.weaponType);
        }

        if (aDown && (0 >= playerStatus.isFlinch) && playerStatus.attackDelay < 0 && playerStats.weapon.ammo == 0 && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
            playerAnim.animator.SetInteger("AttackType", (int)playerStats.weapon.weaponData.weaponType);
        }
    }

    void ReloadOut()
    {
        if (!playerStatus.isReload)
            return;

        playerStatus.reloadDelay += Time.deltaTime * (playerStats.attackSpeed);

        if (playerStatus.reloadDelay >= playerStats.weapon.GetReloadTime())
        {
            playerStats.weapon.Reload();
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
        public bool EquipWeapon(Weapon _Weapon)
        {
            // ���� ��Ȱ��ȭ �� ����
            //_Weapon.gameObject.SetActive(false);

            // ���� �ɷ�ġ ����
            player.playerStats.weapon.SetWeaponData(_Weapon);
            player.playerStats.weapon.Equip(player);

            // ���� �нú� ����
            //player.playerStats.weapon.Equip(this.gameObject.GetComponent<Player>());

            if ((int)player.playerStats.weapon.weaponData.weaponType < (int)WEAPON_TYPE.MELEE)
            {
                HitDetectionGameObject = player.hitEffects[(int)player.playerStats.weapon.weaponData.SwingEffectType];
            }
            else if ((int)player.playerStats.weapon.weaponData.weaponType < (int)WEAPON_TYPE.RANGE)
            {
                projectile = player.playerStats.weapon.weaponData.projectile;
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
            player.playerStats.weapon.gameObject.transform.position = transform.position;
            Instantiate(DataManager.instance.gameData.weaponList[player.playerStats.weapon.weaponData.selectItemID], player.CenterPivot.transform.position, gameObject.transform.localRotation);

            // ���� �нú� ����
            player.playerStats.weapon.UnEquip(player);

            // ���� �ɷ�ġ ����
            player.playerStats.weapon.SetWeaponData(null);

            //player.playerStats.weapon.gameObject.SetActive(true);

            // ���� ����
            //player.playerStats.weapon = null;
            //MapUIManager.instance.UpdateWeaponUI();
        }

        public void Use()
        {

            player.playerStats.weapon.ConsumeAmmo();
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[player.playerStats.weapon.weaponData.weaponType.ToSafeString()];
            AttackCombo = animationInfo.Animation.Count <= AttackCombo + 1 ? 0 : (AttackCombo + 1);
            ComboTimer = 0;

            if ((int)player.playerStats.weapon.weaponData.weaponType < (int)WEAPON_TYPE.MELEE)
            {
                // �÷��̾� �ִϸ��̼� ����
                attackCoroutine = StartCoroutine(Swing(player.playerStatus.mouseDir, player.playerStatus.mouseAngle));
            }
            else if ((int)player.playerStats.weapon.weaponData.weaponType < (int)WEAPON_TYPE.RANGE)
            {
                attackCoroutine = StartCoroutine(Shot(player.playerStatus.mouseDir, player.playerStatus.mouseAngle));
            }



            //Debug.Log(playerStats.weapon.attackSpeed);
            //Debug.Log(playerStats.attackSpeed);

        }

        IEnumerator Swing(Vector2 _AttackDir, float _AttackAngle)
        {

            PlayerWeapon CurWeapon = player.playerStats.weapon;
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[CurWeapon.weaponData.weaponType.ToSafeString()];

            // ���� ����
            player.playerStatus.isAttack = true;

            // �ִϸ��̼� ����
            player.playerAnim.ChangeDirection(_AttackDir);
            player.playerAnim.ChangeWeaponSprite(CurWeapon.weaponData.weaponType, CurWeapon.weaponData.selectItemID);
            player.playerAnim.animator.Rebind();
            player.playerAnim.animator.SetBool("isAttack", true);
            player.playerAnim.animator.SetInteger("AttackType", (int)CurWeapon.weaponData.weaponType);
            player.playerAnim.animator.SetInteger("AttackCombo", AttackCombo);
            player.playerAnim.animator.SetFloat("AttackSpeed", player.playerStats.attackSpeed);


            AudioManager.instance.WeaponAttackAudioPlay(CurWeapon.weaponData.weaponType);

            //����
            yield return new WaitForSeconds(animationInfo.PreDelay / player.playerStats.attackSpeed);

            // ���� ���⸸ �켱 ����
            int SwingDir = animationInfo.Animation[AttackCombo].SwingDir;
            if (CurWeapon.weaponData.weaponType == WEAPON_TYPE.SWORD ||
            CurWeapon.weaponData.weaponType == WEAPON_TYPE.SHORT_SWORD ||
            CurWeapon.weaponData.weaponType == WEAPON_TYPE.LONG_SWORD ||
            CurWeapon.weaponData.weaponType == WEAPON_TYPE.SCYTHE)
            {
                SwingDir = 0 <= _AttackDir.x ? SwingDir : -SwingDir;
            }

            // ���� ����Ʈ ũ�� ����
            HitDetectionGameObject.transform.localScale = new Vector3(CurWeapon.GetAttackSize() * SwingDir, CurWeapon.GetAttackSize(), 1);


            // ����Ʈ ��ġ ����
            HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
            hitDetection.SetHit_Ratio(
            0, 1, player.playerStats.AttackPower,
            CurWeapon.GetKnockBack(),
            player.playerStats.CriticalChance,
            player.playerStats.CriticalDamage);
            hitDetection.user = player;
            hitDetection.SetMultiHit(CurWeapon.GetMultiHit(), CurWeapon.GetDPS());

            // ��æƮ ����
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetSE(player.playerStats.SEType.Count == 0 ? SE_TYPE.None : player.playerStats.SEType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetCommon(player.playerStats.CommonType.Count == 0 ? COMMON_TYPE.None : player.playerStats.CommonType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetProjectile(player.playerStats.ProjectileType.Count == 0 ? PROJECTILE_TYPE.None : player.playerStats.ProjectileType[0]);

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
            PlayerWeapon CurWeapon = player.playerStats.weapon;

            player.playerStatus.isAttack = true;

            // �ִϸ��̼� ����
            player.playerAnim.ChangeDirection(_AttackDir);
            Vector3 ShotPos = player.hitEffects[(int)CurWeapon.weaponData.ShotPosType].transform.position;
            Vector3 ShotDir = (player.playerStatus.mousePos - ShotPos).normalized;
            float ShotAngle = Mathf.Atan2(player.playerStatus.mousePos.y - ShotPos.y, player.playerStatus.mousePos.x - ShotPos.x) * Mathf.Rad2Deg;

            player.playerAnim.ChangeWeaponSprite(CurWeapon.weaponData.weaponType, CurWeapon.weaponData.selectItemID);
            player.playerAnim.animator.Rebind();
            player.playerAnim.animator.SetBool("isAttack", true);
            player.playerAnim.animator.SetInteger("AttackType", (int)CurWeapon.weaponData.weaponType);
            player.playerAnim.animator.SetInteger("AttackCombo", AttackCombo);
            player.playerAnim.animator.SetFloat("AttackSpeed", player.playerStats.attackSpeed);
            WeaponAnimationInfo animationInfo = player.playerAnim.AttackAnimationData[CurWeapon.weaponData.weaponType.ToSafeString()];

            // ����
            yield return new WaitForSeconds(animationInfo.PreDelay / player.playerStats.attackSpeed);
            AudioManager.instance.WeaponAttackAudioPlay(CurWeapon.weaponData.weaponType);

            // ���� ����ü ����

            GameObject instantProjectile = ObjectPoolManager.instance.Get(projectile, ShotPos);

            //����ü ����
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

            // ����ü ����
            hitDetection.SetProjectile_Ratio(CurWeapon.GetMaxPenetrations()
                , 0
                , 1
                , player.playerStats.AttackPower
                , CurWeapon.GetKnockBack()
                , player.playerStats.CriticalChance
                , player.playerStats.CriticalDamage);
            hitDetection.SetMultiHit(CurWeapon.GetMultiHit(), CurWeapon.GetDPS());
            hitDetection.user = player;


            // ��æƮ
            instantProjectile.GetComponentInChildren<Enchant>().SetSE(player.playerStats.SEType.Count == 0 ? SE_TYPE.None : player.playerStats.SEType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetCommon(player.playerStats.CommonType.Count == 0 ? COMMON_TYPE.None : player.playerStats.CommonType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetProjectile(player.playerStats.ProjectileType.Count == 0 ? PROJECTILE_TYPE.None : player.playerStats.ProjectileType[0]);

            // ���� ����
            instantProjectile.transform.rotation = Quaternion.AngleAxis(ShotAngle - 90, Vector3.forward);

            // ũ�� ����
            instantProjectile.transform.localScale = new Vector3(CurWeapon.GetAttackSize(), CurWeapon.GetAttackSize(), 1);

            // �ӵ� ����
            bulletRigid.velocity = ShotDir * 10 * CurWeapon.GetProjectileSpeed();

            // �����Ÿ� ����
            hitDetection.SetDisableTime(CurWeapon.GetProjectileTime());

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

        if (playerStats.weapon.weaponData == null)
            return;

        if (playerStats.weapon.ammo == 0)
            return;

        playerStatus.isAttackReady = playerStatus.attackDelay <= 0;

        if (aDown && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isReload && !playerStatus.isDodge && playerStatus.isAttackReady && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            WeaponAnimationInfo animationInfo = playerAnim.AttackAnimationData[playerStats.weapon.weaponData.weaponType.ToSafeString()];

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
            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.skillType == 0)
            {
                player.playerStatus.isSkill = true;
                yield return null;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Enter(player);

            // ��ų Ű �ٿ� ��� ����
            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.skillType == 0)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.postDelay);
                yield return null;
                player.playerStatus.isSkill = false;
            }

            skillCoroutine = StartCoroutine(Stay());

            yield return null;          // �� ������ �ڷ�ƾ ������ �ȵ� yield return�� ������ �ڷ�ƾ���� ��� ���ϴ� ��?
        }

        IEnumerator Stay()
        {
            //print("Stay");
            float timer = player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.maxHoldTime;

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

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.skillType == SKILL_TYPE.UP)
            {
                player.playerStatus.isSkill = true;
                yield return null;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Exit();

            skillCoroutine = null;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.skillType == SKILL_TYPE.UP)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillData.postDelay);
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
        if (skDown && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            // ��ų�� ������ �ִ� ���¿��� ������ ���Ⱑ ������ ���� ���� ��
            // �ش� ���⸸ ����� �����ϴ�.

            // ���� ������ ���� ��
            if (skillList[playerStats.skill[playerStatus.skillIndex]].skillData.skillLimit.Length != 0)
            {
                // ���Ⱑ ���ų�
                // ���� ���⸦ ������ ���� �ʰų�
                int SkillUseOk = Array.IndexOf(skillList[playerStats.skill[playerStatus.skillIndex]].skillData.skillLimit, playerStats.weapon.weaponData.weaponType);
                print(SkillUseOk);
                if (playerStats.weapon.weaponData == null || SkillUseOk == -1)
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
        if ((!skDown) && (0 >= playerStatus.isFlinch) && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && playerStatus.isSkillHold)
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
            if (skcDown > 0f)
            {
                playerStatus.skillIndex = playerStatus.skillIndex + 1 > playerStats.maxSkillSlot - 1 ? playerStats.maxSkillSlot - 1 : playerStatus.skillIndex + 1;
            }
            else if (skcDown < 0f)
            {
                playerStatus.skillIndex = 0 > playerStatus.skillIndex - 1 ? 0 : playerStatus.skillIndex - 1;
            }
        }
    }

    #endregion

    #region Interaction

    void Interaction()
    {
        if (playerStatus.nearObject == null)
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
        // ������ ȹ�� ����
        bool gainItem = false;

        // ���� =======================================================
        // ���� ���� ���� ���� �� ���� ����
        if (selectItem.itemData.selectItemType == SelectItemType.Weapon)
        {
            if (playerStats.weapon.weaponData != null)
            {
                weaponController.UnEquipWeapon();
            }
            // ���� ���
            gainItem = weaponController.EquipWeapon(selectItem.GetComponent<Weapon>());
        }
        // ���� =======================================================
        // ����ִ� ��� �������� ����
        // ����ִ� ��� ������ ���ٸ� ���� ����
        else if (selectItem.itemData.selectItemType == SelectItemType.Equipments)
        {
            gainItem = EquipEquipment(selectItem.GetComponent<Equipment>().equipmentData);
        }
        // ��ų =======================================================
        // ���� ��� ���� ��ų ���� �� ��ų ����
        else if (selectItem.itemData.selectItemType == SelectItemType.Skill)
        {

            if (playerStats.skill[playerStatus.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // ��ų ����
            gainItem = skillController.EquipSkill(selectItem.GetComponent<SelectItem>().itemData.selectItemID);

        }
        // �Ϲ� ������ =======================================================
        // ȹ�� ��� ��� ��
        else if (selectItem.itemData.selectItemType == SelectItemType.Consumable)
        {
            //UseItem
            selectItem.GetComponent<Consumable>().UseItem(this);
            gainItem = true;
        }

        if (gainItem)
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
    public bool EquipEquipment(EquipmentData _EData)
    {
        bool equipOK = false;

        for (int i = 0; i < playerStats.equipments.Length; i++)
        {
            // �ߺ� ���� �Ұ�
            if (playerStats.equipments[i] == _EData && playerStats.equipments[i] != null)
                break;
            // ���� ��
            if (playerStats.equipments[i] != null)
                continue;

            playerStats.equipments[i] = _EData;
            equipOK = true;
            break;
        }

        if (!equipOK)
            return false;

        // �� �нú� ����
        foreach (PassiveData passiveData in _EData.passives)
        {
            ApplyPassive(passiveData);
        }

        return true;
    }

    // ���� ������ ��� �� ������ index
    public bool UnEquipEquipment(int index)
    {
        if (playerStats.equipments[index] == null)
            return false;

        // ���� ��ġ�� ��� ���´�.
        GameObject EGO = Instantiate(DataManager.instance.gameData.equipmentList[playerStats.equipments[index].selectItemID], CenterPivot.transform.position, gameObject.transform.localRotation);
        EquipmentData EData= EGO.GetComponent<Equipment>().equipmentData;
        // ���� �ɷ�ġ ����
        //equipmentList[playerStats.equipments[index]].UnEquip(this.gameObject.GetComponent<Player>());
        //equipmentList[playerStats.equipments[index]].gameObject.SetActive(false);
        foreach (PassiveData passiveData in EData.passives)
        {
            RemovePassive(passiveData);
        }

        // ���� ����
        playerStats.equipments[index] = null;
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
                GameObject WeaponObject = Instantiate(DataManager.instance.gameData.weaponList[playerWeapon]);
                weaponController.EquipWeapon(WeaponObject.GetComponent<Weapon>());
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
                {
                    EquipmentData EData = DataManager.instance.gameData.weaponList[playerEquipment[i]].GetComponent<Equipment>().equipmentData;
                    EquipEquipment(EData);

                }
            }

            // ���� ����
            for (int i = 0; i < playerStats.playerStat.Length; i++)
            {
                playerStats.playerStat[i] = DataManager.instance.userData.playerStat[i];
            }

            Player.instance.playerStats.HPMax.AddValue += Player.instance.playerStats.playerStat[(int)StatID.HP] * StatIV[(int)StatID.HP];
            Player.instance.playerStats.AttackPower.IncreasedValue += Player.instance.playerStats.playerStat[(int)StatID.AP] * StatIV[(int)StatID.AP];
            Player.instance.playerStats.increasedAttackSpeed += Player.instance.playerStats.playerStat[(int)StatID.AS] * StatIV[(int)StatID.AS];
            Player.instance.playerStats.CriticalChance.AddValue += Player.instance.playerStats.playerStat[(int)StatID.CC] * StatIV[(int)StatID.CC];
            Player.instance.playerStats.CriticalDamage.AddValue += Player.instance.playerStats.playerStat[(int)StatID.CD] * StatIV[(int)StatID.CD];
            Player.instance.playerStats.SkillPower.AddValue += Player.instance.playerStats.playerStat[(int)StatID.SP] * StatIV[(int)StatID.SP];
            Player.instance.playerStats.SkillCoolTime.AddValue += Player.instance.playerStats.playerStat[(int)StatID.SCT] * StatIV[(int)StatID.SCT];
            Player.instance.playerStats.MoveSpeed.IncreasedValue += Player.instance.playerStats.playerStat[(int)StatID.MS] * StatIV[(int)StatID.MS];

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
    public enum StatID { HP, AP, AS, CC, CD, SP, SCT, MS };
    // ���� ������
    [HideInInspector]
    public readonly float[] StatIV = { 25, 0.2f, 0.2f, 0.1f, 0.05f, 10f, -0.1f, 0.1f };

    // ������ ���� ��Ų��.
    public void StatLevelUp(StatID _StatID)
    {
        Player.instance.playerStats.playerStat[(int)_StatID]++;
        switch (_StatID)
        {
            case StatID.HP:
                Player.instance.playerStats.HPMax.AddValue += StatIV[(int)_StatID];
                break;
            case StatID.AP:
                Player.instance.playerStats.AttackPower.IncreasedValue += StatIV[(int)_StatID];
                break;
            case StatID.AS:
                Player.instance.playerStats.increasedAttackSpeed += StatIV[(int)_StatID];
                break;
            case StatID.CC:
                Player.instance.playerStats.CriticalChance.AddValue += StatIV[(int)_StatID];
                break;
            case StatID.CD:
                Player.instance.playerStats.CriticalDamage.AddValue += StatIV[(int)_StatID];
                break;
            case StatID.SP:
                Player.instance.playerStats.SkillPower.AddValue += StatIV[(int)_StatID];
                break;
            case StatID.SCT:
                Player.instance.playerStats.SkillCoolTime.AddValue = StatIV[(int)_StatID];
                break;
            case StatID.MS:
                Player.instance.playerStats.MoveSpeed.IncreasedValue += StatIV[(int)_StatID];
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
            else if (userData.nowChapter == 4)
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
        other.tag == "Npc" || other.tag == "reward" || other.tag == "SellingItem")
        {
            if (playerStatus.nearObject == null || Vector2.Distance(CenterPivot.transform.position, other.transform.position) < Vector2.Distance(CenterPivot.transform.position, playerStatus.nearObject.transform.position))
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
