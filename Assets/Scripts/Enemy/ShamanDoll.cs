using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanDoll : EnemyBasic
{
    //[SerializeField] int defaulCurseCoolTime;
    //private bool moveReady;

    //Vector2 playerPos1;
    //Vector2 playerPos2;
    //Vector2 playerPath;
    //Vector2 perpendicularDir;

    protected override void Start()
    {
        base.Start();
        //moveReady = true;
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void MovePattern()
    {
        // isRun 상태면 도망가는 함수있어서 그걸 쓰면 될듯
        // if(moveReady)
        // {
        //     StartCoroutine(runaway());
        // }
        
    }

    protected override void AttackPattern()
    {
        enemyStatus.attackCoroutine = StartCoroutine(Curse());
    }

    IEnumerator Curse()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(2f);

        enemyAnim.animator.SetBool("isAttack", true);

        //print("shamon doll hurts hershelf");
        yield return new WaitForSeconds(0.6f);
        enemyStatus.enemyTarget.gameObject.GetComponent<ObjectBasic>().ApplyBuff(8);
        enemyStatus.enemyTarget.gameObject.GetComponent<ObjectBasic>().ApplyBuff(8);
        enemyStatus.enemyTarget.gameObject.GetComponent<ObjectBasic>().ApplyBuff(8);
        enemyStatus.enemyTarget.gameObject.GetComponent<ObjectBasic>().ApplyBuff(8);
        enemyStatus.enemyTarget.gameObject.GetComponent<ObjectBasic>().ApplyBuff(8);
        yield return new WaitForSeconds(2f);

        enemyAnim.animator.SetBool("isAttack", false);

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

        if(enemyStatus.enemyTarget) RunAway(enemyStatus.enemyTarget.transform, 5.0f);
    }

    public override void AttackCancle()
    {
        base.AttackCancle();
        if(enemyStatus.enemyTarget) RunAway(enemyStatus.enemyTarget.transform, 5.0f);
    }

    /*
    IEnumerator runaway() 
    {
        //moveReady = false;

        //print("shamon doll move");
        playerPos1 = enemyStatus.enemyTarget.transform.position;
        yield return new WaitForSeconds(0.2f);
        playerPos2 = enemyStatus.enemyTarget.transform.position;

        playerPath = playerPos1 - playerPos2;
        perpendicularDir = new Vector2(playerPath.y, -playerPath.x).normalized;
        rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);

        enemyStatus.targetDirVec = (enemyStatus.enemyTarget.transform.position - transform.position).normalized;
        rigid.AddForce(-enemyStatus.targetDirVec * GetComponent<EnemyStats>().moveSpeed * 100f);

        //moveReady = true;
    }
    */

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
