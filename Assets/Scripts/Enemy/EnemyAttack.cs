using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyAttack
{ 
    rushHit, multiShot, hitAndRun, 
    rangeAttack, waveAttack,
    pop,
    chase,jump ,None,
    wander
};

//���� ġ��

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


    /*
    private void OnEnable()
    {
        StartNamedCoroutine("EnemyPatternStart", EnemyPatternStart());
    }
    */


    IEnumerator EnemyPatternStart()
    {
        switch (enemyAttack)
        {
            case enemyAttack.rushHit: StartCoroutine(rushHit(true)); break;
            case enemyAttack.multiShot:StartCoroutine(multiShot(EnemyGunMuzzle.Count, EnemyGunMuzzle,true)); break;
            case enemyAttack.hitAndRun: StartCoroutine(hitAndRun(true)); break;
            case enemyAttack.rangeAttack: StartCoroutine(rangeAttack(roundAttackRange,true)); break;
            case enemyAttack.waveAttack: StartCoroutine(waveAttack(donutAttackRange, donutInside,true)); break;
            case enemyAttack.pop: StartCoroutine(pop(true)); break;
            case enemyAttack.chase:StartCoroutine(chasing());break;
            case enemyAttack.jump: StartCoroutine(jump(true)); break;
            case enemyAttack.wander:StartCoroutine(Wander(true));break;
            default: break;
        }
        yield return null;
    }
    



   

   

}
