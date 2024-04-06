using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanDoll : EnemyBasic
{
    void Start()
    {
        StartCoroutine(shamanDoll());
    }

    private void OnEnable()
    {
        StartCoroutine(shamanDoll());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator shamanDoll()
    {

        Vector2 playerPos1 = enemyTarget.transform.position;
        yield return new WaitForSeconds(2f);
        Vector2 playerPos2 = enemyTarget.transform.position;

        Vector2 playerPath=playerPos1-playerPos2;
        Vector2 perpendicularDir = new Vector2(playerPath.y, -playerPath.x).normalized;
        rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);

        /*
        targetDirVec = enemyTarget.transform.position - transform.position;
        rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().moveSpeed*10f);
        yield return new WaitForSeconds(2f);
        */

        enemyTarget.GetComponent<PlayerStats>().HP -= GetComponent<EnemyStats>().attackPower;
        print("doll is killing hershelf");

        StartCoroutine(shamanDoll());
        
    }
}
