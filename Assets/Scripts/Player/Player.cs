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

    public bool rDown { get; private set; }                                 // 재장전
    public bool dDown { get; private set; }                                 // 회피
    public bool aDown { get; private set; }                                 // 공격
    public bool siDown { get; private set; }                                // 선택 아이템 H
    public bool iDown { get; private set; }                                 // 상호작용

    public float skcDown { get; private set; }                              // 스킬 변경
    public bool skDown { get; private set; }                                // 스킬 키 다운 중

    #endregion

    public LayerMask layerMask;             //접근 불가한 레이어 설정

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
        defaultLayer = this.gameObject.layer;
    }
    void Update()
    {
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

        HealPoise();
        Turn();
        //Run();
        Dodge();
        Move();
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
        if( playerStatus.isFlinch )
            return;

        playerStatus.moveVec = new Vector2(hAxis, vAxis).normalized;

        if (playerStatus.isAttack || playerStatus.isSkill)       // 정지
        {
            playerStatus.moveVec = Vector2.zero;
        }
        
        if (playerStatus.isDodge)             // 회피시 현재 속도 유지
        {
            rigid.velocity  = dodgeVec * playerStats.moveSpeed * (1 + playerStats.dodgeSpeed);
        }
        else
        {
            // 기본 속도 = 플레이어 이동속도 * 플레이어 디폴트 이동속도
            rigid.velocity = playerStatus.moveVec * playerStats.moveSpeed;
        }
    }

    void Turn()
    {
        playerStatus.mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerStatus.mouseDir = (Vector2)(playerStatus.mousePos - transform.position).normalized;

        playerStatus.mouseAngle = Mathf.Atan2(playerStatus.mousePos.y - transform.position.y, playerStatus.mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(mouseAngle - 90, Vector3.forward);

    }
 
    void Dodge()    // 회피
    {
        if(playerStatus.moveVec == Vector2.zero)
            return;
        
        if (dDown && !playerStatus.isFlinch && !playerStatus.isAttack && !playerStatus.isSkill  && !playerStatus.isDodge && !playerStatus.isSkillHold)
        {
            dodgeVec = playerStatus.moveVec;
            playerStatus.isDodge = true;
            playerStatus.isReload = false;
            playerStats.HP += 100;
            playerStats.addMoveSpeed = 20;

            Invoke("DodgeOut", playerStats.dodgeTime);

        }
    }

    void DodgeOut() // 회피 빠져나가기
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

        if (weaponList[playerStats.weapon].maxAmmo < 0)
            return;

        if (weaponList[playerStats.weapon].maxAmmo == weaponList[playerStats.weapon].ammo)
            return;

        if (rDown && !playerStatus.isFlinch && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
        }

        if (aDown && !playerStatus.isFlinch && playerStatus.attackDelay < 0 && weaponList[playerStats.weapon].ammo == 0 && !playerStatus.isDodge && !playerStatus.isReload && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {
            playerStatus.isReload = true;
            playerStatus.reloadDelay = 0f;
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
        // 공격 정보
        Coroutine attackCoroutine;
        [field: SerializeField] public List<SE_TYPE> SEType { get; set; } = new List<SE_TYPE>();

        [field: SerializeField] public List<COMMON_TYPE> CommonType { get; set; } = new List<COMMON_TYPE>();

        [field: SerializeField] public List<PROJECTILE_TYPE> ProjectileType { get; set; } = new List<PROJECTILE_TYPE>();
        [SerializeField] GameObject HitDetectionGameObject;
        int projectileIndex;
        void Awake()
        {
            player = GetComponent<Player>();
        }

        // 무기를 획득
        public bool EquipWeapon(int weaponID)
        {
            // 무기 소유
            player.playerStats.weapon = weaponID;
            // 장비 능력치 적용
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(true);
            player.weaponList[player.playerStats.weapon].Equip(this.gameObject.GetComponent<Player>());

            if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.MELEE)
            {
                HitDetectionGameObject = player.hitEffects[(int)player.weaponList[player.playerStats.weapon].weaponType];
            }
            else if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.RANGE)
            {
                projectileIndex = player.weaponList[player.playerStats.weapon].projectileIndex;
            }

            // 장비 UI 적용
            //MapUIManager.instance.UpdateWeaponUI();
            return true;
        }

        public void UnEquipWeapon()
        {
            HitDetectionGameObject = null;
            projectileIndex = -1;

            // 현재 위치에 장비를 놓는다.
            Instantiate(GameData.instance.weaponList[player.playerStats.weapon], gameObject.transform.position, gameObject.transform.localRotation);

            // 무기 능력치 해제
            player.weaponList[player.playerStats.weapon].UnEquip(this.gameObject.GetComponent<Player>());
            player.weaponList[player.playerStats.weapon].gameObject.SetActive(false);

            // 무기 해제
            player.playerStats.weapon = 0;
            //MapUIManager.instance.UpdateWeaponUI();
        }

        public void Use(Vector3 clickPos)
        {
            player.weaponList[player.playerStats.weapon].ConsumeAmmo();
            if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.MELEE)
            {
                // 플레이어 애니메이션 실행
                attackCoroutine = StartCoroutine("Swing");
            }
            else if ((int)player.weaponList[player.playerStats.weapon].weaponType < (int)WEAPON_TYPE.RANGE)
            {
                attackCoroutine = StartCoroutine("Shot");
            }

            //Debug.Log(playerStats.weapon.attackSpeed);
            //Debug.Log(playerStats.attackSpeed);

        }

        IEnumerator Swing()
        {
            // 공격 상태
            player.playerStatus.isAttack = true;

            AudioManager.instance.WeaponAttackAudioPlay(player.weaponList[player.playerStats.weapon].weaponType);

            //선딜
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].preDelay / player.playerStats.attackSpeed);

            // 무기 이펙트 크기 설정
            HitDetectionGameObject.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize, player.weaponList[player.playerStats.weapon].attackSize, 1);
        
        
            // 이펙트 수치 설정
            HitDetection hitDetection = HitDetectionGameObject.GetComponentInChildren<HitDetection>();
            hitDetection.SetHitDetection(false, 
            -1, 
            player.weaponList[player.playerStats.weapon].isMultiHit, 
            player.weaponList[player.playerStats.weapon].DPS, 
            player.playerStats.attackPower, 
            player.weaponList[player.playerStats.weapon].knockBack, 
            player.playerStats.criticalChance, 
            player.playerStats.criticalDamage);
            hitDetection.user = this.gameObject;

            // 인챈트 설정
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetSE(SEType.Count == 0 ? SE_TYPE.NONE : SEType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetCommon(CommonType.Count == 0 ? COMMON_TYPE.NONE : CommonType[0]);
            HitDetectionGameObject.GetComponentInChildren<Enchant>().SetProjectile(ProjectileType.Count == 0 ? PROJECTILE_TYPE.NONE : ProjectileType[0]);

            // 무기 방향 
            HitDetectionGameObject.transform.rotation = Quaternion.AngleAxis(player.playerStatus.mouseAngle - 90, Vector3.forward);

            // 파티클
            {
                // 공격 속도에 따른 이펙트 가속
                ParticleSystem.MainModule particleMain = HitDetectionGameObject.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startLifetime = player.weaponList[player.playerStats.weapon].rate / player.playerStats.attackSpeed;

            }
            
            // 무기 이펙트 실행
            HitDetectionGameObject.SetActive(true);

            // 공격 시간
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].rate / player.playerStats.attackSpeed);

            // 무기 이펙트 해제
            HitDetectionGameObject.SetActive(false);

            // 공격 상태 해제
            player.playerStatus.isAttack = false;
        }

        IEnumerator Shot()
        {
            player.playerStatus.isAttack = true;

            float attackAngle = player.playerStatus.mouseAngle;
            Vector2 attackDir = player.playerStatus.mouseDir;

            // 선딜
            yield return new WaitForSeconds(player.weaponList[player.playerStats.weapon].preDelay / player.playerStats.attackSpeed);
            AudioManager.instance.WeaponAttackAudioPlay(player.weaponList[player.playerStats.weapon].weaponType);

            // 무기 투사체 적용
            GameObject instantProjectile = ObjectPoolManager.instance.Get(projectileIndex);
            instantProjectile.transform.position = transform.position;
            instantProjectile.transform.rotation = transform.rotation;

            //투사체 설정
            Rigidbody2D bulletRigid = instantProjectile.GetComponent<Rigidbody2D>();
            HitDetection hitDetection = instantProjectile.GetComponent<HitDetection>();

            // 투사체 설정
            hitDetection.SetHitDetection(true, player.weaponList[player.playerStats.weapon].penetrations, player.weaponList[player.playerStats.weapon].isMultiHit, player.weaponList[player.playerStats.weapon].DPS, player.playerStats.attackPower, player.weaponList[player.playerStats.weapon].knockBack, player.playerStats.criticalChance, player.playerStats.criticalDamage);
            hitDetection.user = this.gameObject;

            // 인챈트
            instantProjectile.GetComponentInChildren<Enchant>().SetSE(SEType.Count == 0 ? SE_TYPE.NONE : SEType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetCommon(CommonType.Count == 0 ? COMMON_TYPE.NONE : CommonType[0]);
            instantProjectile.GetComponentInChildren<Enchant>().SetProjectile(ProjectileType.Count == 0 ? PROJECTILE_TYPE.NONE : ProjectileType[0]);

            // 방향 설정
            instantProjectile.transform.rotation = Quaternion.AngleAxis(attackAngle - 90, Vector3.forward);

            // 크기 설정
            instantProjectile.transform.localScale = new Vector3(player.weaponList[player.playerStats.weapon].attackSize, player.weaponList[player.playerStats.weapon].attackSize, 1);

            // 속도 설정
            bulletRigid.velocity = attackDir * 10 * player.weaponList[player.playerStats.weapon].projectileSpeed;

            // 사정거리 설정
            hitDetection.SetProjectileTime(player.weaponList[player.playerStats.weapon].projectileTime);

            // 공격 상태 해제
            player.playerStatus.isAttack = false;
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

        if (aDown && !playerStatus.isFlinch && !playerStatus.isAttack && !playerStatus.isReload && !playerStatus.isDodge && playerStatus.isAttackReady && !playerStatus.isSkill && !playerStatus.isSkillHold )
        {
            weaponController.Use(playerStatus.mousePos);

            // 다음 공격까지 대기 시간 = 1 / 초당 공격 횟수
            playerStatus.attackDelay = weaponList[playerStats.weapon].SPA / playerStats.attackSpeed;
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

        // 스킬 획득
        public bool EquipSkill(int skillID)
        {
            // 이미 보유한 스킬이라면
            if (player.skillList[skillID].gameObject.activeSelf == true)
                return false;

            player.playerStats.skill[player.playerStatus.skillIndex] = skillID;
            player.skillList[skillID].gameObject.SetActive(true);
            return true;
        }

        // 스킬 해제
        public void UnEquipSkill()
        {
            Instantiate(DataManager.instance.gameData.skillList[player.playerStats.skill[player.playerStatus.skillIndex]], gameObject.transform.position, gameObject.transform.localRotation);
            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].gameObject.SetActive(false);
            player.playerStats.skill[player.playerStatus.skillIndex] = 0;
        }

        // 스킬키 입력
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
            //print("Enter");
            // 홀드 중
            player.playerStatus.isSkillHold = true;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 0)
            {
                player.playerStatus.isSkill = true;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Enter(gameObject);

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 0)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].postDelay);
                player.playerStatus.isSkill = false;
            }

            skillCoroutine = StartCoroutine(Stay());

            yield return null;          // 안 넣으면 코루틴 저장이 안됨 yield return이 없으면 코루틴으로 취급 안하는 듯?
        }

        IEnumerator Stay()
        {
            //print("Stay");
            float timer = player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].maxHoldTime;

            while (player.playerStatus.isSkillHold)
            {
                yield return new WaitForSeconds(0.1f);
                timer -= 0.1f;
                player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].HoldCoolDown();      // 홀드 중일 때는 쿨타임이 줄어들지 않음
                if (timer <= 0)
                {
                    skillCoroutine = StartCoroutine(Exit());
                    break;
                }
            }

            yield return null;          // 안 넣으면 코루틴 저장이 안됨
        }

        IEnumerator Exit()
        {
            player.playerStatus.isSkillHold = false;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 2)
            {
                player.playerStatus.isSkill = true;
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].preDelay);
            }

            player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].Exit();

            skillCoroutine = null;

            if (player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].skillType == 2)
            {
                yield return new WaitForSeconds(player.skillList[player.playerStats.skill[player.playerStatus.skillIndex]].postDelay);
                player.playerStatus.isSkill = false;
            }

            yield return null;      // 안 넣으면 코루틴 저장이 안됨

            skillCoroutine = null;

        }

        public void SkillCancle()
        {
            print("Cancle");
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

        // 스킬 키 다운
        if (skDown && !playerStatus.isFlinch && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && !playerStatus.isSkillHold )
        {
            //스킬이 제한이 있는 상태에서 적절한 무기가 가지고 있지 않을 때
            if (playerStats.weapon == 0 && 
                skillList[playerStats.skill[playerStatus.skillIndex]].skillLimit.Length != 0 && 
            Array.IndexOf(skillList[playerStats.skill[playerStatus.skillIndex]].skillLimit, (int)weaponList[playerStats.weapon].weaponType) == -1)
            {
                return;
            }
            skillController.SkillDown();
        }

    }
    
    void SkillUp()
    {
        if (playerStats.skill[playerStatus.skillIndex] == 0)
            return;

        //스킬 hold 상태에서 스킬 키 up
        if ((!skDown)&& !playerStatus.isFlinch && !playerStatus.isAttack && !playerStatus.isDodge && !playerStatus.isSkill && playerStatus.isSkillHold)
        {
            skillController.SkillUp();
            playerStatus.isReload = false;
        }
    }
    
    void SkillChange()
    {
        playerStatus.skillChangeDelay -= Time.deltaTime;

        if (skcDown != 0f && !playerStatus.isFlinch && !playerStatus.isSkill && !playerStatus.isSkillHold && playerStatus.skillChangeDelay <= 0f)
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

        if (iDown && !playerStatus.isFlinch && !playerStatus.isDodge && !playerStatus.isAttack && !playerStatus.isSkill && !playerStatus.isSkillHold)
        {

            if (playerStatus.nearObject.tag == "SelectItem")
            {
                GainSelectItem();
            }
            else if (playerStatus.nearObject.tag == "Npc")
            {
                playerStatus.nearObject.GetComponentInParent<NPCbasic>().Conversation();
            }
            else if (playerStatus.nearObject.tag == "Door")
            {
                if (playerStats.key > 0)
                {
                    playerStats.key--;
                    playerStatus.nearObject.GetComponent<Door>().DoorInteraction();
                }

            }
            else if (playerStatus.nearObject.tag == "ShabbyWall")
            {
                //open with bomb
                //nearObject.GetComponent<Wall>().WallInteraction();
            }
            else if (playerStatus.nearObject.tag == "reward")
            {
                print("reward interaction");
                playerStatus.nearObject.GetComponent<treasureBox>().Interaction();
            
            }
        }

    }

    #endregion Interaction

    #region Item

    void GainSelectItem()
    {
        SelectItem selectItem = playerStatus.nearObject.GetComponent<SelectItem>();
        
        // 아이템 획득 여부
        bool gainItem = false;

        // 무기 =======================================================
        // 현재 장착 무기 해제 후 무기 장착
        if (selectItem.selectItemType == SelectItemType.Weapon)
        {
            if (playerStats.weapon != 0)
            {
                weaponController.UnEquipWeapon();
            }
            // 무기 장비
            gainItem = weaponController.EquipWeapon(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        // 갑옷 =======================================================
        // 비어있는 장비 슬롯으로 장착
        // 비어있는 장비 슬롯이 없다면 장착 실패
        else if (selectItem.selectItemType == SelectItemType.Equipments)
        {
            gainItem = EquipEquipment(selectItem.GetComponent<SelectItem>().selectItemID);
        }
        // 스킬 =======================================================
        // 현재 사용 중인 스킬 해제 후 스킬 장착
        else if (selectItem.selectItemType == SelectItemType.Skill)
        {

            if (playerStats.skill[playerStatus.skillIndex] != 0)
            {
                skillController.UnEquipSkill();
            }
            // 스킬 장착
            gainItem = skillController.EquipSkill(selectItem.GetComponent<SelectItem>().selectItemID);

        }
        // 일반 아이템 =======================================================
        // 획득 즉시 사용 됨
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
        print("Load Data");
        if (scene.name != "Main")
        {
            //playerStats.level = DataManager.instance.userData.playerLevel;
            //playerStats.exp = DataManager.instance.userData.playerExp;
            //playerStats.point = DataManager.instance.userData.playerPoint;

            playerStats.HP = DataManager.instance.userData.playerHP;
            playerStats.tempHP = DataManager.instance.userData.playerTempHP;

            //Debug.Log("Scene reloaded: " + scene.name);
            //Scene reload 후에도 전에 얻은 아이템 유지
            //int playerItemName = DataManager.instance.userData.playerItem;

            // 무기
            int playerWeapon = DataManager.instance.userData.playerWeapon;
            if (playerWeapon != 0)
            {
                weaponController.EquipWeapon(playerWeapon);
            }

            // 스킬
            playerStats.maxSkillSlot = DataManager.instance.userData.playerMaxSkillSlot;
            int[] playerSkill = DataManager.instance.userData.playerSkill;
            for (int i = 0; i < playerSkill.Length; i++)
            {
                if (playerSkill[i] != 0)
                    skillController.EquipSkill(playerSkill[i]);
            }

            // 방어구
            playerStats.maxEquipment = DataManager.instance.userData.playerMaxEquipments;
            int[] playerEquipment = DataManager.instance.userData.playerEquipments;
            for (int i = 0; i < playerEquipment.Length; i++)
            {
                if (playerEquipment[i] != 0)
                    EquipEquipment(playerEquipment[i]);
            }

            // 스탯 적용
            for(int i = 0;i<playerStats.playerStat.Length;i++)
            {
                playerStats.playerStat[i] = DataManager.instance.userData.playerStat[i];
            }

            Player.instance.playerStats.HPMax                       += Player.instance.playerStats.playerStat[(int)StatID.HP] * StatIV[(int)StatID.HP];
            Player.instance.playerStats.increasedAttackPower        += Player.instance.playerStats.playerStat[(int)StatID.AP] * StatIV[(int)StatID.AP];
            Player.instance.playerStats.increasedAttackSpeed        += Player.instance.playerStats.playerStat[(int)StatID.AS] * StatIV[(int)StatID.AS];
            Player.instance.playerStats.addCriticalChance           += Player.instance.playerStats.playerStat[(int)StatID.CC] * StatIV[(int)StatID.CC];
            Player.instance.playerStats.addCriticalDamage           += Player.instance.playerStats.playerStat[(int)StatID.CD] * StatIV[(int)StatID.CD];
            Player.instance.playerStats.addSkillPower               += Player.instance.playerStats.playerStat[(int)StatID.SP] * StatIV[(int)StatID.SP];
            Player.instance.playerStats.addSkillCoolTime            += Player.instance.playerStats.playerStat[(int)StatID.SCT] * StatIV[(int)StatID.SCT];
            Player.instance.playerStats.increasedMoveSpeed          += Player.instance.playerStats.playerStat[(int)StatID.MS] * StatIV[(int)StatID.MS];

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

        }

    }

    #endregion

    #region StatLV

    // 체력, 공격력, 공격속도, 치명타 확률 ,치명타 피해량, 도력, 도술 대기시간, 이동속도
    public enum StatID {HP, AP, AS, CC, CD, SP, SCT, MS};
    // 스탯 증가량
    [HideInInspector]
    public readonly float[] StatIV = { 25, 0.2f, 0.2f, 0.1f, 0.05f, 10f, -0.1f, 0.1f };

    // 스탯을 증가 시킨다.
    public void StatLevelUp(StatID _StatID)
    {
        Player.instance.playerStats.playerStat[(int)_StatID]++;
        switch(_StatID)
        {
            case StatID.HP:
            Player.instance.playerStats.HPMax                       += StatIV[(int)_StatID];
            break;
            case StatID.AP:
            Player.instance.playerStats.increasedAttackPower        += StatIV[(int)_StatID];
            break;
            case StatID.AS:
            Player.instance.playerStats.increasedAttackSpeed        += StatIV[(int)_StatID];
            break;
            case StatID.CC:
            Player.instance.playerStats.addCriticalChance           += StatIV[(int)_StatID];
            break;
            case StatID.CD:
            Player.instance.playerStats.addCriticalDamage           += StatIV[(int)_StatID];
            break;
            case StatID.SP:
            Player.instance.playerStats.addSkillPower               += StatIV[(int)_StatID];
            break;
            case StatID.SCT:
            Player.instance.playerStats.addSkillCoolTime            += StatIV[(int)_StatID];
            break;
            case StatID.MS:
            Player.instance.playerStats.increasedMoveSpeed          += StatIV[(int)_StatID];
            break;

        }
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
                Destroy(other.gameObject); //코인 오브젝트 삭제
                playerStats.coin++;
            }
            else if (item.itemClass == ItemClass.Key)
            {
                print("get key");
                other.gameObject.SetActive(false); //키 오브젝트 삭제
                playerStats.key++;
            }
            /*
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
            */
            //MapUIManager.instance.UpdateMinimapUI(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SelectItem" || other.tag == "Door" || other.tag == "ShabbyWall" || other.tag == "Npc" || other.tag=="reward")
        {
            if(playerStatus.nearObject == null || Vector2.Distance(transform.position, other.transform.position) < Vector2.Distance(transform.position, playerStatus.nearObject.transform.position))
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

    // 상태 관련
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

    public override void AttackCancle()
    {
        base.AttackCancle();
        playerStatus.attackDelay = 0;
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
