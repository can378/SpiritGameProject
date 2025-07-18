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
    public EnemyStats enemyStats;       // ?  ?¤?¯
    [HideInInspector]
    public EnemyStatus enemyStatus;     // ?  ?? ??
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
            (enemyStatus.targetDis <= enemyStats.maxAttackRange || 
            enemyStats.maxAttackRange < 0))
        {
            enemyStatus.moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    // ?ë§? ì¤ì´ ?? ?
    // ?  ê³µê²© ?¨?´(ê¸°ë³¸ ?¨?´ : ê³µê²© ??¨)
    protected virtual void AttackPattern()
    {
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = false;
    }

    protected virtual void Detect()
    {
        // When there is a target
        if (enemyStatus.isTarget)
        {
            // Å¸°ÙÀÌ ½Ã¾ß ¹üÀ§ ¾È¿¡ µé¾î¿Ô¾îµµ, Å¸°ÙÀÌ ½Ã¾ß À¯Áö °Å¸®º¸´Ù ¸Ö¸® ÀÖ´Ù¸é Å¸°ÙÀ» ÇØÁ¦.
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

        MovePattern();

        rigid.velocity = enemyStatus.moveVec * stats.MoveSpeed.Value * status.moveSpeedMultiplier;

    }

    // ?´? ?¨?´(ê¸°ë³¸ ?¨?´ : ???ê²ì´ ??¼ë©? ë¬´ì? ?´?, ???ê²ì´ ??¼ë©? ?¬? ê±°ë¦¬ ê¹ì?? ì¶ì )
    protected virtual void MovePattern()
    {
        if (!enemyStatus.isTarget)
        {
            RandomMove();
        }
        // ? ?´ ê³µê²© ?¬? ê±°ë¦¬ ë°ì ?? ?
        else if (enemyStatus.targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    // ë¬´ì? ?´?
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

    // ?  ì¶ì 
    protected void Chase()
    {
        if (!enemyStatus.isTarget)
        {
            return;
        }

       enemyStatus.moveVec = (enemyStatus.EnemyTarget.transform.position - CenterPivot.position).normalized;
    }

    
    // ?¼?? ?¼ë¡? ?ë§ì¹ê¸?
    protected void Run()
    {
        if (enemyStatus.fearTarget)
        {
            status.moveVec = -(enemyStatus.fearTarget.position - transform.position).normalized;
        }
    }

    public void RunAway(Transform _FearTarget, float _Time)
    {
        if(_FearTarget == null)
        {
            Debug.LogWarning("no enemy target. cancel runaway");
            return;
        }
        enemyStatus.fearTarget = _FearTarget;
        
        if(enemyStatus.runCoroutine != null)
        {
            StopCoroutine(enemyStatus.runCoroutine);
        }

        enemyStatus.runCoroutine = StartCoroutine(RunCoroutine(_Time));
    }

    // ?¼?  ?ê°? ?? ?ë§ì¹ê¸?
    IEnumerator RunCoroutine(float time)
    {
        enemyStatus.isRun = true;
        yield return new WaitForSeconds(time);
        enemyStatus.isRun = false;
        enemyStatus.fearTarget = null;
    }

    /*
    public void CheckMovement()
    {
        Vector2 velocity = rigid.velocity;

        if (velocity.magnitude == 0)
        {
            //Debug.Log("ë©ì¶¤");
            enemyAnim.horizontalMove = 0;
            enemyAnim.verticalMove = 0;
        }
        else
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x > 0)
                {
                    //Debug.Log("?¤ë¥¸ìª½?¼ë¡? ???ì§ì");
                    enemyAnim.horizontalMove = 1;
                }
                else
                {
                    //Debug.Log("?¼ìª½ì¼ë¡? ???ì§ì");
                    enemyAnim.horizontalMove = -1;
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    //Debug.Log("?ìª½ì¼ë¡? ???ì§ì");
                    enemyAnim.verticalMove = 1;
                }
                else
                {
                    //Debug.Log("??ìª½ì¼ë¡? ???ì§ì");
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
        enemyStatus.isTouchPlayer= false;
        if (collision.tag == "Player")
        {
            enemyStatus.isTouchPlayer = true;
        }
    }

    public override void Dead()
    {
        //drop coin
        // enemyStats.DropCoinÀ» ¼öÁ¤ÇÏ¿© ¶³²Ù´Â ÄÚÀÎ ¼ö º¯°æ °¡´É
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
        // ë³´ì¤?¼ë©? UI? ? ë³´ë?? ??´?¤.
        if(enemyStatus.isBoss)
            MapUIManager.instance.SetBossProgress(this.GetComponent<EnemyBasic>());
    }

    void OnDisable()
    {
        // ë¹í?±?? ?? ì´ê¸°?
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
