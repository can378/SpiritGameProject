using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyAttack
{ rushHit, multiShot, hitAndRun, rangeAttack, waveAttack,pop,chase,jump };


public class EnemyAttack : EnemyPattern
{
    public enemyAttack enemyAttack;

    [Header("multiShot")]
    public List<GameObject> EnemyGunMuzzle;

    [Header("waveAttack")]
    public GameObject donutAttackRange;
    public GameObject donutInside;

    [Header("rangeAttack")] 
    public GameObject roundAttackRange;

    private void Start()
    {
        //if (this.transform.gameObject.activeSelf != false) { EnemyPatternStart(); }
    }

    private void OnEnable()
    {
        EnemyPatternStart();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void EnemyPatternStart()
    {
        switch (enemyAttack)
        {
            case enemyAttack.rushHit: StartCoroutine(rushHit()); break;
            case enemyAttack.multiShot:StartCoroutine(multiShot(EnemyGunMuzzle.Count, EnemyGunMuzzle)); break;
            case enemyAttack.hitAndRun: StartCoroutine(hitAndRun()); break;
            case enemyAttack.rangeAttack: StartCoroutine(rangeAttack(roundAttackRange)); break;
            case enemyAttack.waveAttack: StartCoroutine(waveAttack(donutAttackRange, donutInside)); break;
            case enemyAttack.pop: StartCoroutine(pop()); break;
            case enemyAttack.chase:StartCoroutine(chasing());break;
            case enemyAttack.jump: StartCoroutine(jump()); break;
            default: break;
        }
    }
    

}
