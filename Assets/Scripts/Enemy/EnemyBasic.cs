using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class EnemyBasic : ObjectBasic
{
    public bool isRun;                  // ���� �� : ���� �� �� ������ �����Լ� �־���
    public float randomMove = 0;

    public EnemyStats enemyStats;       // �� ����

    public LayerMask detectEnemy;
    public Transform enemyTarget;       // ���� Ÿ��
    public Vector2 targetDirVec;        // ���� ����
    // ���� -> rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90)
    public float targetDis;             // ������ �Ÿ�

    //Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();


    protected override void Awake()
    {
        base.Awake();
        enemyTarget = GameObject.FindWithTag("Player").gameObject.transform;//�̰� ������ �ȵǴ°ž�?
        stats = enemyStats = GetComponent<EnemyStats>();

    }

    void Start()
    {
        defaultLayer = this.gameObject.layer;
    }

    protected virtual void Update()
    {
        Attack();
        Move();
        Detect();
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

        targetDis = Vector2.Distance(this.transform.position, enemyTarget.position);
        targetDirVec = (enemyTarget.position - transform.position).normalized;

        //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

        if (!isRun && !isFlinch && !isAttack && isAttackReady && (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0))
        {
            moveVec = Vector2.zero;
            AttackPattern();
        }
    }

    // �� ���� ����(�⺻ ���� : ���� ����)
    protected virtual void AttackPattern()
    {
        isAttack = false;
        isAttackReady = false;
    }

    void Detect()
    {
        // Ÿ���� ���� ��
        if (enemyTarget != null)
        {
            // Ÿ�� ���� �Ÿ��� ��� �̰� Ÿ���� �����Ÿ����� �ָ� �ִٸ� Ÿ�� ����
            if (0 <= enemyStats.detectionKeepDis && enemyStats.detectionKeepDis < targetDis)
            {
                enemyTarget = null;
                return;
            }
            return;
        }

        var target = Physics2D.OverlapCircle(transform.position, enemyStats.detectionDis, detectEnemy);

        if (target == null)
        {
            return;
        }

        enemyTarget = target.transform;
    }

    #endregion Attack

    #region Move

    void Move()
    {
        // ������ ���� �߿��� ���� �̵� �Ұ�
        if (isFlinch)
        {
            return;
        }
        else if (isAttack)
        {
            rigid.velocity = moveVec * stats.moveSpeed;
            return;
        }
        else if (isRun)
        {
            if (enemyTarget)
            {
                rigid.velocity = -(enemyTarget.position - transform.position).normalized * stats.moveSpeed;
            }
            return;
        }

        MovePattern();

        rigid.velocity = moveVec * stats.moveSpeed;

    }

    // �̵� ����(�⺻ ���� : Ÿ���� ������ ������ �̵�, Ÿ���� ������ �����Ÿ� ���� ����)
    protected virtual void MovePattern()
    {
        if (!enemyTarget)
        {
            RandomMove();
        }
        // ���� ���� �����Ÿ� �ۿ� ���� ��
        else if (targetDis > enemyStats.maxAttackRange)
        {
            Chase();
        }
    }

    // ������ �̵�
    protected void RandomMove()
    {
        randomMove -= Time.deltaTime;
        if (-1f < randomMove && randomMove < 0f)
        {
            moveVec = Vector2.zero;
            randomMove -= 1;
        }
        else if (randomMove < -3f)
        {
            moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
            randomMove = Random.Range(2, 5);
        }
    }

    // �� ����
    protected void Chase()
    {
        if (!enemyTarget)
        {
            return;
        }

        moveVec = (enemyTarget.transform.position - transform.position).normalized;
    }

    // ����ġ�� ���� ����
    protected void Run()
    {
        if (!enemyTarget)
            return;

        moveVec = -(enemyTarget.transform.position - transform.position).normalized * 0.5f;
    }


    #endregion Move

    #region Effect

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    #endregion Effect

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, enemyStats.detectionDis);
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
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
    }

    public void shotWhat(string name)
    {
        GameObject bullet = ObjectPoolManager.instance.Get2(name);
        bullet.transform.position = transform.position;
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
