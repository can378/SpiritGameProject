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
    public EnemyStats enemyStats;       // ?  ?€?―
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
        enemyStatus.enemyTarget = FindObj.instance.Player.transform;
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
        if (!enemyStatus.enemyTarget)
            return;

        enemyStatus.targetDis = Vector2.Distance(this.transform.position, enemyStatus.enemyTarget.position);
        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.position - transform.position).normalized;

        //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

        if (this.gameObject.activeSelf && !enemyStatus.isRun && (0 >= enemyStatus.isFlinch) && !enemyStatus.isAttack && enemyStatus.isAttackReady && (enemyStatus.targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0))
        {
            enemyStatus.moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    // ?λ§? μ€μ΄ ?? ?
    // ?  κ³΅κ²© ?¨?΄(κΈ°λ³Έ ?¨?΄ : κ³΅κ²© ??¨)
    protected virtual void AttackPattern()
    {
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = false;
    }

    protected virtual void Detect()
    {
        // ???κ²μ΄ ?? ?
        if (enemyStatus.enemyTarget != null)
        {
            // ???κ²? ? μ§? κ±°λ¦¬κ°? ?? ?΄κ³? ???κ²μ΄ ? μ§?κ±°λ¦¬λ³΄λ€ λ©?λ¦? ??€λ©? ???κ²? ?΄? 
            if (0 <= enemyStats.detectionKeepDis && enemyStats.detectionKeepDis < enemyStatus.targetDis)
            {
                enemyStatus.enemyTarget = null; // ?? ?΄?΄κ°? κ±°λ¦¬ μ‘°μ ?λ©? ? ?€ μ£½μΌ ? ?κ²?, ?΄κ±? ??λ©? ? ?€?΄ ?λ¬? λͺ°λ €???? ??΄?κ°? ?λ¬? ??μ§?.
                return;
            }
            return;
        }

        var target = Physics2D.OverlapCircle(this.transform.position, enemyStats.detectionDis, targetLayer);

        if (target == null)
        {
            return;
        }

        enemyStatus.enemyTarget = target.transform;
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

    // ?΄? ?¨?΄(κΈ°λ³Έ ?¨?΄ : ???κ²μ΄ ??Όλ©? λ¬΄μ? ?΄?, ???κ²μ΄ ??Όλ©? ?¬? κ±°λ¦¬ κΉμ?? μΆμ )
    protected virtual void MovePattern()
    {
        if (!enemyStatus.enemyTarget)
        {
            RandomMove();
        }
        // ? ?΄ κ³΅κ²© ?¬? κ±°λ¦¬ λ°μ ?? ?
        else if (enemyStatus.targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    // λ¬΄μ? ?΄?
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

    // ?  μΆμ 
    protected void Chase()
    {
        if (!enemyStatus.enemyTarget)
        {
            return;
        }

       enemyStatus.moveVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
    }

    
    // ?Ό?? ?Όλ‘? ?λ§μΉκΈ?
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

    // ?Ό?  ?κ°? ?? ?λ§μΉκΈ?
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
            //Debug.Log("λ©μΆ€");
            enemyAnim.horizontalMove = 0;
            enemyAnim.verticalMove = 0;
        }
        else
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x > 0)
                {
                    //Debug.Log("?€λ₯Έμͺ½?Όλ‘? ???μ§μ");
                    enemyAnim.horizontalMove = 1;
                }
                else
                {
                    //Debug.Log("?Όμͺ½μΌλ‘? ???μ§μ");
                    enemyAnim.horizontalMove = -1;
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    //Debug.Log("?μͺ½μΌλ‘? ???μ§μ");
                    enemyAnim.verticalMove = 1;
                }
                else
                {
                    //Debug.Log("??μͺ½μΌλ‘? ???μ§μ");
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
            BeAttacked(collision.gameObject.GetComponent<HitDetection>(), collision.ClosestPoint(transform.position));
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
        // enemyStats.DropCoinΐ» ΌφΑ€ΗΟΏ© Ά³²Ω΄Β ΔΪΐΞ Όφ Ί―°ζ °‘΄Ι
        Vector3 coinDropPoint = transform.position;
        GameManager.instance.dropCoin(enemyStats.DropCoin, coinDropPoint);
        
        base.Dead();
    }

    #endregion Effect

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, enemyStats.detectionDis);
        //Gizmos.DrawWireSphere(this.transform.position, enemyStats.detectionKeepDis);
    }

    public override void AttackCancle()
    {
        if (enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
        }
        base.AttackCancle();

    }

    public override void InitStatus()
    {
        enemyStatus.isRun = false;
        enemyStatus.isTouchPlayer = false;
        if (enemyStatus.attackCoroutine != null)
        {
            StopCoroutine(enemyStatus.attackCoroutine);
        }
        base.InitStatus();

    }

    void OnEnable()
    {
        // λ³΄μ€?Όλ©? UI? ? λ³΄λ?? ??΄?€.
        if(enemyStatus.isBoss)
            MapUIManager.instance.SetBossProgress(this.GetComponent<EnemyBasic>());
    }

    void OnDisable()
    {
        // λΉν?±?? ?? μ΄κΈ°?
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
