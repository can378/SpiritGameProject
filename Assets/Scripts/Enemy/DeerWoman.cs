using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerWoman : EnemyBasic
{
    void Start()
    {
        StartCoroutine(deerWoman());
    }

    private void OnEnable()
    {
        StartCoroutine(deerWoman());
    }

    private void OnDisable()
    {
        StopCoroutine(deerWoman());
    }


    IEnumerator deerWoman()
    {
        //shot arrow
        GameObject bullet = ObjectPoolManager.instance.Get(0);
        bullet.transform.position = transform.position;


        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();

        targetDirVec = (enemyTarget.position - transform.position).normalized;
        rigidBullet.AddForce(targetDirVec.normalized * 3, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);

        StartCoroutine(deerWoman());

    }
}