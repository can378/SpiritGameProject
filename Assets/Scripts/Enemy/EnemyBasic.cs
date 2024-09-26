using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;



public class EnemyBasic : ObjectBasic
{
    [HideInInspector]
    public EnemyStats enemyStats;       // 적 스탯
    [HideInInspector]
    public EnemyStatus enemyStatus;     // 적 행동 상태

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
                hitEffect.GetComponent<HitDetection>().user = this.gameObject;
        }

        defaultLayer = this.gameObject.layer;
    }

    protected virtual void Start()
    {
        enemyStatus.enemyTarget = FindObj.instance.Player.transform;
            
    }

    protected virtual void Update()
    {
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

        if (!enemyStatus.isRun && !enemyStatus.isFlinch && !enemyStatus.isAttack && enemyStatus.isAttackReady && (enemyStatus.targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0))
        {
            enemyStatus.moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    // 도망 중이 아닐 때
    // 적 공격 패턴(기본 패턴 : 공격 안함)
    protected virtual void AttackPattern()
    {
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = false;
    }

    protected virtual void Detect()
    {
        // 타겟이 있을 때
        if (enemyStatus.enemyTarget != null)
        {
            // 타겟 유지 거리가 양수 이고 타겟이 유지거리보다 멀리 있다면 타겟 해제
            if (0 <= enemyStats.detectionKeepDis && enemyStats.detectionKeepDis < enemyStatus.targetDis)
            {
                enemyStatus.enemyTarget = null; // 플레이어가 거리 조절하며 적들 죽일 수 있게, 이거 안하면 적들이 너무 몰려와서 난이도가 너무 높아짐.
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
        // 경직과 공격 중에는 직접 이동 불가
        if (enemyStatus.isFlinch)
        {
            return;
        }
        else if (enemyStatus.isAttack)
        {
            rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;
            return;
        }
        else if (enemyStatus.isRun)
        {
            Run();
            return;
        }

        MovePattern();

        rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    }

    // 이동 패턴(기본 패턴 : 타겟이 없으면 무작위 이동, 타겟이 있으면 사정거리 까지 추적)
    protected virtual void MovePattern()
    {
        if (!enemyStatus.enemyTarget)
        {
            RandomMove();
        }
        // 적이 공격 사정거리 밖에 있을 시
        else if (enemyStatus.targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    // 무작위 이동
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

    // 적 추적
    protected void Chase()
    {
        if (!enemyStatus.enemyTarget)
        {
            return;
        }

       enemyStatus.moveVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
    }

    
    // 일시적으로 도망치기
    protected void Run()
    {
        if (enemyStatus.enemyTarget)
        {
            rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
        }
    }

    // 일정 시간 동안 도망치기
    protected IEnumerator RunAway(float time)
    {
        enemyStatus.isRun = true;
        yield return new WaitForSeconds(time);
        enemyStatus.isRun = false;
    }

    public void CheckMovement()
    {
        Vector2 velocity = rigid.velocity;

        if (velocity.magnitude == 0)
        {
            //Debug.Log("멈춤");
            enemyAnim.horizontalMove = 0;
            enemyAnim.verticalMove = 0;
        }
        else
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x > 0)
                {
                    //Debug.Log("오른쪽으로 움직임");
                    enemyAnim.horizontalMove = 1;
                }
                else
                {
                    //Debug.Log("왼쪽으로 움직임");
                    enemyAnim.horizontalMove = -1;
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    //Debug.Log("위쪽으로 움직임");
                    enemyAnim.verticalMove = 1;
                }
                else
                {
                    //Debug.Log("아래쪽으로 움직임");
                    enemyAnim.verticalMove = -1;
                }
            }
        }
    }

    #endregion Move

    #region Effect

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" || collision.tag == "AllAttack")
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>());
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
        int dropCoinNum = 3;
        Vector3 coinDropPoint = transform.position;
        GameManager.instance.dropCoin(dropCoinNum, coinDropPoint);
        
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
        base.AttackCancle();
        if(enemyStatus.attackCoroutine != null) 
        {
            Debug.Log(name + "코루틴 종료");
            StopCoroutine(enemyStatus.attackCoroutine);
        }
    }

    public override void InitStatus()
    {
        enemyStatus.isRun = false;
        enemyStatus.isTouchPlayer = false;
        base.InitStatus();
        if(enemyStatus.attackCoroutine != null) 
        {
            Debug.Log(name + "코루틴 종료");
            StopCoroutine(enemyStatus.attackCoroutine);
        }
    }

    void OnEnable()
    {
        // 보스라면 UI에 정보를 띄운다.
        if(enemyStatus.isBoss)
            MapUIManager.instance.SetBossProgress(this.GetComponent<EnemyBasic>());
    }

    void OnDisable()
    {
        // 비활성화시 상태 초기화
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
        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(enemyStatus.targetDirVec.normalized * 7, ForceMode2D.Impulse);
    }

    public void shotWhat(string name)
    {
        GameObject bullet = ObjectPoolManager.instance.Get2(name);
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
