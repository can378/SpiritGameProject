using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;



public class EnemyBasic : ObjectBasic
{
    [HideInInspector]
    public EnemyStats enemyStats;       // ?†Å ?ä§?ÉØ
    [HideInInspector]
    public EnemyStatus enemyStatus;     // ?†Å ?ñâ?èô ?ÉÅ?Éú
    [HideInInspector]
    public EnemyAnim enemyAnim;

    [SerializeField]
    LayerMask targetLayer;


    protected override void Awake()
    {
        base.Awake();       
        stats = enemyStats = GetComponent<EnemyStats>();
        status = enemyStatus = GetComponent<EnemyStatus>();
        foreach (GameObject hitEffect in hitEffects)
        {
            if (hitEffect.GetComponent<HitDetection>())
                hitEffect.GetComponent<HitDetection>().user = this;
        }

        enemyAnim = animGameObject.GetComponent<EnemyAnim>();
    }

    protected virtual void Start()
    {
        enemyStatus.EnemyTarget = FindObj.instance.Player.transform.GetComponent<Player>();
    }

    protected virtual void Update()
    {
        if (CameraManager.instance.isShowingBoss) 
        {
            return;
        }
        SEProgress();
        HealPoise();
        Attack();
        Move();
        Detect();
    }

    #region  Attack

    protected virtual void Attack()
    {
        if (!enemyStatus.isTarget)
            return;

        enemyStatus.targetDis = Vector2.Distance(CenterPivot.position, enemyStatus.EnemyTarget.CenterPivot.position);
        enemyStatus.targetDirVec = (enemyStatus.EnemyTarget.CenterPivot.position - CenterPivot.position).normalized;

        //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

        if (gameObject.activeSelf && 
            !enemyStatus.isRun && 
            (0 >= enemyStatus.isFlinch) && 
            !enemyStatus.isAttack && 
            enemyStatus.isAttackReady && 
            !enemyStatus.isWating &&
            (enemyStatus.targetDis <= enemyStats.maxAttackRange ||
            enemyStats.maxAttackRange < 0))
        {
            enemyStatus.moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    // ?èÑÎß? Ï§ëÏù¥ ?ïÑ?ãê ?ïå
    // ?†Å Í≥µÍ≤© ?å®?Ñ¥(Í∏∞Î≥∏ ?å®?Ñ¥ : Í≥µÍ≤© ?ïà?ï®)
    protected virtual void AttackPattern()
    {
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = false;
    }

    protected virtual void Detect()
    {
        if (enemyStatus.isTargetNoThisRoom)
        {
            enemyStatus.isTargetNoThisRoomTime += Time.deltaTime;
            enemyStatus.isTarget = false;
            if (1 <= enemyStatus.isTargetNoThisRoomTime)
                gameObject.SetActive(false);

            return;
        }

        // When there is a target
        if (enemyStatus.isTarget)
        {
            // ≈∏∞Ÿ¿Ã Ω√æﬂ π¸¿ß æ»ø° µÈæÓø‘æÓµµ, ≈∏∞Ÿ¿Ã Ω√æﬂ ¿Ø¡ˆ ∞≈∏Æ∫∏¥Ÿ ∏÷∏Æ ¿÷¥Ÿ∏È ≈∏∞Ÿ¿ª «ÿ¡¶.
            if (!enemyStatus.isTargetForced &&
                0 <= enemyStats.detectionKeepDis &&
                enemyStats.detectionKeepDis < enemyStatus.targetDis)
            {
                enemyStatus.isTarget = false;
                return;
            }
            return;
        }

        var target = Physics2D.OverlapCircle(CenterPivot.position, enemyStats.detectionDis, targetLayer);

        if (target == null)
        {
            return;
        }

        //enemyStatus.enemyTarget = target.transform;
        enemyStatus.isTarget = true;
        Waiting(0.5f);
    }

    #endregion Attack

    #region Move

    protected virtual void Move()
    {
        enemyStatus.moveVec.Normalize();
        if (0 < enemyStatus.isFlinch)
        {
            return;
        }
        else if (enemyStatus.isAttack)
        {
            rigid.velocity = enemyStatus.moveVec * stats.MoveSpeed.Value * status.moveSpeedMultiplier;
            return;
        }
        else if (enemyStatus.isRun)
        {
            Run();
            rigid.velocity = enemyStatus.moveVec * stats.MoveSpeed.Value * status.moveSpeedMultiplier;
            return;
        }
        else if (enemyStatus.isWating)
        {
            enemyStatus.moveVec = Vector2.zero;
            return;
        }

        MovePattern();

        rigid.velocity = enemyStatus.moveVec * stats.MoveSpeed.Value * status.moveSpeedMultiplier;

    }

    // ?ù¥?èô ?å®?Ñ¥(Í∏∞Î≥∏ ?å®?Ñ¥ : ???Í≤üÏù¥ ?óÜ?úºÎ©? Î¨¥Ïûë?úÑ ?ù¥?èô, ???Í≤üÏù¥ ?ûà?úºÎ©? ?Ç¨?†ïÍ±∞Î¶¨ ÍπåÏ?? Ï∂îÏ†Å)
    protected virtual void MovePattern()
    {
        if (!enemyStatus.isTarget)
        {
            RandomMove();
        }
        // ?†Å?ù¥ Í≥µÍ≤© ?Ç¨?†ïÍ±∞Î¶¨ Î∞ñÏóê ?ûà?ùÑ ?ãú
        else if (enemyStatus.targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    // Î¨¥Ïûë?úÑ ?ù¥?èô
    protected void RandomMove()
    {
        enemyStatus.randomMove -= Time.deltaTime;
        if (-1f < enemyStatus.randomMove && enemyStatus.randomMove < 0f)
        {
            enemyStatus.moveVec = Vector2.zero;
            enemyStatus.randomMove -= 1;
        }
        else if (enemyStatus.randomMove < -3f)
        {
            enemyStatus.moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
            enemyStatus.randomMove = Random.Range(2, 5);
        }
    }

    // ?†Å Ï∂îÏ†Å
    protected void Chase()
    {
        if (!enemyStatus.isTarget || enemyStatus.targetDis < 1f)
        {
            enemyStatus.moveVec = Vector2.zero;
            return;
        }


       enemyStatus.moveVec = (enemyStatus.EnemyTarget.CenterPivot.transform.position - CenterPivot.position).normalized;
    }

    
    // ?ùº?ãú?†Å?úºÎ°? ?èÑÎßùÏπòÍ∏?
    protected void Run()
    {
        if (enemyStatus.fearTarget)
        {
            status.moveVec = -(enemyStatus.fearTarget.position - transform.position).normalized;
        }
    }

    public void RunAway(Transform _FearTarget, float _Time)
    {
        if (_FearTarget == null)
        {
            Debug.LogWarning("no enemy target. cancel runaway");
            return;
        }
        enemyStatus.fearTarget = _FearTarget;

        if (enemyStatus.runCoroutine != null)
        {
            StopCoroutine(enemyStatus.runCoroutine);
        }

        enemyStatus.runCoroutine = StartCoroutine(RunCoroutine(_Time));
    }

    // ?ùº?†ï ?ãúÍ∞? ?èô?ïà ?èÑÎßùÏπòÍ∏?
    IEnumerator RunCoroutine(float time)
    {
        enemyStatus.isRun = true;
        yield return new WaitForSeconds(time);
        enemyStatus.isRun = false;
        enemyStatus.fearTarget = null;
        Waiting(1f);
    }

    public void Waiting(float _Time)
    {
        if (enemyStatus.watingCoroutine != null)
        {
            StopCoroutine(enemyStatus.watingCoroutine);
        }

        enemyStatus.watingCoroutine = StartCoroutine(WaitingCoroutine(_Time));
    }

    // ?ùº?†ï ?ãúÍ∞? ?èô?ïà ?èÑÎßùÏπòÍ∏?
    IEnumerator WaitingCoroutine(float time)
    {
        enemyStatus.isWating = true;
        yield return new WaitForSeconds(time);
        enemyStatus.isWating = false;
    }

    /*
    public void CheckMovement()
    {
        Vector2 velocity = rigid.velocity;

        if (velocity.magnitude == 0)
        {
            //Debug.Log("Î©àÏ∂§");
            enemyAnim.horizontalMove = 0;
            enemyAnim.verticalMove = 0;
        }
        else
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x > 0)
                {
                    //Debug.Log("?ò§Î•∏Ï™Ω?úºÎ°? ???ÏßÅÏûÑ");
                    enemyAnim.horizontalMove = 1;
                }
                else
                {
                    //Debug.Log("?ôºÏ™ΩÏúºÎ°? ???ÏßÅÏûÑ");
                    enemyAnim.horizontalMove = -1;
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    //Debug.Log("?úÑÏ™ΩÏúºÎ°? ???ÏßÅÏûÑ");
                    enemyAnim.verticalMove = 1;
                }
                else
                {
                    //Debug.Log("?ïÑ?ûòÏ™ΩÏúºÎ°? ???ÏßÅÏûÑ");
                    enemyAnim.verticalMove = -1;
                }
            }
        }
    }
    */
    #endregion Move

    #region Effect

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") || collision.CompareTag("AllAttack"))
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>(), collision.ClosestPoint(CenterPivot.position));
        }
        enemyStatus.isTouchPlayer = false;
        if (collision.tag == "Player")
        {
            enemyStatus.isTouchPlayer = true;
        }
    }

    public override void Dead()
    {
        //drop coin
        // enemyStats.DropCoin¿ª ºˆ¡§«œø© ∂≥≤Ÿ¥¬ ƒ⁄¿Œ ºˆ ∫Ø∞Ê ∞°¥…
        Vector3 coinDropPoint = transform.position;
        GameManager.instance.dropCoin(enemyStats.DropCoin, coinDropPoint);
        
        base.Dead();
    }

    #endregion Effect


    //enemy attacked
    public override bool Damaged(float damage, float critical = 0, float criticalDamage = 0)
    {
        bool criticalHit = base.Damaged(damage, critical, criticalDamage);

        // Detect the target (player) when the enemy is attacked.
        enemyStatus.isTarget = true;
        enemyStatus.isTargetForced= true;
        return criticalHit;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, enemyStats.detectionDis);
        //Gizmos.DrawWireSphere(this.transform.position, enemyStats.detectionKeepDis);
    }

    public override void FlinchCancle()
    {
        if (enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
        }
        base.FlinchCancle();

    }

    public override void InitStatus()
    {
        enemyStatus.isRun = false;
        enemyStatus.isTouchPlayer = false;
        enemyStatus.isTarget = false;
        enemyStatus.isTargetForced = false;
        if (enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
        }
        base.InitStatus();

    }

    void OnEnable()
    {
        // Î≥¥Ïä§?ùºÎ©? UI?óê ?†ïÎ≥¥Î?? ?ùÑ?ö¥?ã§.
        if(enemyStatus.isBoss)
            MapUIManager.instance.SetBossProgress(this.GetComponent<EnemyBasic>());
    }

    void OnDisable()
    {
        // ÎπÑÌôú?Ñ±?ôî?ãú ?ÉÅ?Éú Ï¥àÍ∏∞?ôî
        InitStatus();
    }

    /*
    public void Chase()
    {
        Vector2 direction = (enemyTarget.position - transform.position).normalized;
        //rigid.velocity = direction * enemyStats.moveSpeed;
        transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);
    }
    */

    public void shot()
    {
        GameObject bullet = ObjectPoolManager.instance.Get("Bullet");
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(enemyStatus.targetDirVec.normalized * 7, ForceMode2D.Impulse);
    }

    public void shotWhat(string name)
    {
        GameObject bullet = ObjectPoolManager.instance.Get(name);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(enemyStatus.targetDirVec.normalized * 2, ForceMode2D.Impulse);

    }

    /*
    public IEnumerator runAway()
    {
        print("enemy runaway");

        Vector2 playerPos1 = enemyTarget.transform.position;
        yield return new WaitForSeconds(2f);
        Vector2 playerPos2 = enemyTarget.transform.position;

        Vector2 playerPath = playerPos1 - playerPos2;
        Vector2 perpendicularDir = new Vector2(playerPath.y, -playerPath.x).normalized;
        rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);
        StartCoroutine(runAway());
    }
    */


    #region Coroutine Manager
    /*

    public void StartNamedCoroutine(string coroutineName, IEnumerator routine)
    {
        if (!runningCoroutines.ContainsKey(coroutineName))
        {
            Coroutine newCoroutine = StartCoroutine(routine);
            runningCoroutines.Add(coroutineName, newCoroutine);
        }
        else
        {
            Coroutine newCoroutine = StartCoroutine(routine);
            //Debug.LogWarning("Coroutine with name " + coroutineName + " is already running.");
        }
    }


    public void StopNamedCoroutine(string coroutineName)
    {
        if (runningCoroutines.ContainsKey(coroutineName))
        {
            StopCoroutine(runningCoroutines[coroutineName]);
            //runningCoroutines.Remove(coroutineName);
        }
        else
        {
            Debug.LogWarning("Coroutine with name " + coroutineName + " is not running.");
        }
    }


    public void StopAllCoroutinesAndGetNames()
    {
        //List<string> stoppedCoroutineNames = new List<string>();

        foreach (KeyValuePair<string, Coroutine> kvp in runningCoroutines)
        {
            //stoppedCoroutineNames.Add(kvp.Key);
            StopCoroutine(kvp.Value);
        }

        //runningCoroutines.Clear(); // Clear the dictionary since all coroutines are stopped

    }

    public void RestartAllCoroutines() 
    {
        foreach (KeyValuePair<string, Coroutine> kvp in runningCoroutines)
        {
            StartCoroutine(kvp.Key);
        }

    }

    */

    #endregion


}
