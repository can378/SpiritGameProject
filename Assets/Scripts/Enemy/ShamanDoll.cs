using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanDoll : EnemyBasic
{
    [SerializeField] int defaulCurseCoolTime;
    [SerializeField] float curseCoolTime = 0;

    protected override void Update()
    {
        base.Update();
        curseCoolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        if(0f < curseCoolTime && targetDis <= 20f)
        {
            Run();
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
        yield return new WaitForSeconds(1f);

        enemyTarget.gameObject.GetComponent<ObjectBasic>().Damaged(5f);
        yield return new WaitForSeconds(1f);

        isAttack = false;
        isAttackReady = true;
        curseCoolTime = defaulCurseCoolTime;
    }

    /*

    private void OnEnable()
    {
        StartNamedCoroutine("shamanDoll", shamanDoll());
    }

    // 무당 인형
    // 기본적으로 도망다니며 플레이어에게 지속적인 피해

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
