using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanDoll : EnemyBasic
{
    // 무당 인형
    // 기본적으로 도망다니며 플레이어에게 지속적인 피해

    [SerializeField] int defaulCurseCoolTime;
    [SerializeField] float curseCoolTime = 0;
    private bool moveReady;

    Vector2 playerPos1;
    Vector2 playerPos2;
    Vector2 playerPath;
    Vector2 perpendicularDir;

    private void Start()
    {
        moveReady = true;
    }
    protected override void Update()
    {
        base.Update();
        curseCoolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        if(moveReady)
        {
            StartCoroutine(runaway());
        }
        
    }

    protected override void AttackPattern()
    {
        if(curseCoolTime <= 0f)
        {
            StartCoroutine(Curse());
        }
    }

    IEnumerator Curse()
    {
        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.2f);

        //print("shamon doll hurts hershelf");
        enemyTarget.gameObject.GetComponent<ObjectBasic>().Damaged(5f);
        yield return new WaitForSeconds(0.2f);

        isAttack = false;
        isAttackReady = true;
        curseCoolTime = defaulCurseCoolTime;
    }

    IEnumerator runaway() 
    {
        moveReady = false;

        //print("shamon doll move");
        playerPos1 = enemyTarget.transform.position;
        yield return new WaitForSeconds(0.2f);
        playerPos2 = enemyTarget.transform.position;

        playerPath = playerPos1 - playerPos2;
        perpendicularDir = new Vector2(playerPath.y, -playerPath.x).normalized;
        rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);

        targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
        rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().moveSpeed * 100f);

        moveReady = true;
    }

    /*

    private void OnEnable()
    {
        StartNamedCoroutine("shamanDoll", shamanDoll());
    }

    

    IEnumerator shamanDoll()
    {

        Vector2 playerPos1 = enemyTarget.transform.position;
        yield return new WaitForSeconds(2f);
        Vector2 playerPos2 = enemyTarget.transform.position;

        Vector2 playerPath=playerPos1-playerPos2;
        Vector2 perpendicularDir = new Vector2(playerPath.y, -playerPath.x).normalized;
        rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);

        
        targetDirVec = enemyTarget.transform.position - transform.position;
        rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().moveSpeed*10f);
        yield return new WaitForSeconds(2f);
        

        //enemyTarget.GetComponent<PlayerStats>().HP -= GetComponent<EnemyStats>().attackPower;
        enemyTarget.GetComponent<Player>().BeAttacked(this.gameObject.GetComponent<HitDetection>());
        print("doll is killing hershelf");

        StartCoroutine(shamanDoll());
        
    }
    */
}
