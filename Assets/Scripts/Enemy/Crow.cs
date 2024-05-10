using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : EnemyBasic
{

    private void Start()
    {
        StartNamedCoroutine("LRShot", LRShot());
    }
    private void OnEnable()
    {
        RestartAllCoroutines();
    }

    public IEnumerator LRShot()
    {
        for (int i = 0; i < 2; i++)
        {
            targetDirVec = (enemyTarget.transform.position - transform.position).normalized;

            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.1f);

            for (int j = 0; j < 100; j++)
            { rigid.AddForce(targetDirVec * Mathf.Pow(-1, i) * 2 * Mathf.Pow(3, i)); }
            //{ rigid.AddForce(targetDirVec * Mathf.Pow(-1,i) * stats.speed*Mathf.Pow(3,i)); }

            yield return new WaitForSeconds(0.1f);
            rigid.velocity = Vector2.zero;

            shot();
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(LRShot());
    }
}
