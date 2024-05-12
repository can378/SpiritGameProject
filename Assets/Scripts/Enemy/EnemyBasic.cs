using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class EnemyBasic : ObjectBasic
{
    public bool isChase;        // 적 추적 : 적에게 가까이 다가감
    public bool isRun;          // 도망 중 : 공격 할 수 없으며 적에게서 멀어짐

    //[HideInInspector]
    public Transform enemyTarget;
    [HideInInspector]
    public EnemyStats enemyStats;
    [HideInInspector]
    public Vector2 targetDirVec;
    //[HideInInspector]
    public float targetDis;
    [HideInInspector]
    public float timeValue=0;
    
    //Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();


    protected override void Awake()
    {
        base.Awake();
        enemyTarget = GameObject.FindWithTag("Player").gameObject.transform;
        stats = enemyStats = GetComponent<EnemyStats>();
    }

    protected virtual void Update()
    {
        Attack();
        Move();
    }

    /*
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    */

    #region  Attack

    void Attack()
    {
        if (!enemyTarget)
            return;
        
        targetDis = Vector2.Distance(this.transform.position,enemyTarget.position);

        if ( !isRun && !isFlinch && !isAttack && isAttackReady && (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0))
        {
            moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    protected virtual void AttackPattern()
    {
        isAttack = false;
        isAttackReady = false;
    }

    #endregion Attack

    #region Move

    void Move()
    {
        // 경직과 공격 중에는 직접 이동 불가
        if (isFlinch)
        {
            return;
        }
        else if(isAttack)
        {
            rigid.velocity = moveVec * stats.moveSpeed;
            return;
        }

        MovePattern();


        if (isRun)
        {
            moveVec = -(enemyTarget.transform.position - transform.position).normalized * 0.5f;
        }
        else if (isChase)
        {
            moveVec = (enemyTarget.transform.position - transform.position).normalized;
        }

        rigid.velocity = moveVec * stats.moveSpeed;

    }

    protected virtual void MovePattern()
    {
        // 적이 공격 사정거리 내에 있을 시
        if (targetDis <= enemyStats.maxAttackRange)
        {
            isChase = false;
        }
        else
        {
            isChase = true;
        }
    }

    #endregion Move

    #region Effect

    protected virtual void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "PlayerAttack" || collision.tag == "AllAttack")
        {
            BeAttacked(collision.gameObject.GetComponent<HitDetection>());
        }
    }

    public override void Dead()
    {
        base.Dead();

        //drop coin
        int dropCoinNum = 3;
        Vector3 coinDropPoint = transform.position;
        GameManager.instance.dropCoin(dropCoinNum, coinDropPoint);

        //enemy disappear
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    #endregion Effect

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
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
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
