using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBasic : ObjectBasic
{
    //[HideInInspector]
    public Transform enemyTarget;
    [HideInInspector]
    public EnemyStats enemyStats;
    [HideInInspector]
    public Vector2 targetDirVec;
    [HideInInspector]
    public float targetDis;
    [HideInInspector]
    public float timeValue=0;
    

    Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();


    protected override void Awake()
    {
        base.Awake();
        enemyTarget = GameObject.FindWithTag("Player").gameObject.transform;
        stats = enemyStats = GetComponent<EnemyStats>();

        GetComponent<EnemyStats>().isEnemyFear = false;
        GetComponent<EnemyStats>().isEnemyStun = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "PlayerAttack" || collision.tag == "AllAttack")
        {
            BeAttacked(collision.gameObject);
        }
    }

    public override void Dead()
    {
        base.Dead();

        //drop coin
        int dropCoinNum = 10;
        GameManager.instance.dropCoin(dropCoinNum, transform.position);

        //enemy disappear
        this.gameObject.SetActive(false);
    }

    public void Chase()
    {
        Vector2 direction = (enemyTarget.position - transform.position).normalized;
        //rigid.velocity = direction * enemyStats.moveSpeed;
        transform.Translate(direction * stats.defaultMoveSpeed * Time.deltaTime);
    }

    public void shot()
    {
        GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
        bullet.transform.position = transform.position;
        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized * 2, ForceMode2D.Impulse);
    }

    #region coroutine Manager

    public void StartNamedCoroutine(string coroutineName, IEnumerator routine)
    {
        if (!runningCoroutines.ContainsKey(coroutineName))
        {
            Coroutine newCoroutine = StartCoroutine(routine);
            runningCoroutines.Add(coroutineName, newCoroutine);
        }
        else
        {
            Debug.LogWarning("Coroutine with name " + coroutineName + " is already running.");
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
    #endregion

    public void runAway() 
    {
        print("enemy runaway");

    }
}
