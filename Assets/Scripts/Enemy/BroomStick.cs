using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomStick : EnemyBasic
{
    public GameObject colObj;
    float radius = 50;
    float attackTime = 2;

    int FearIndex;

    protected override void MovePattern()
    {
        Chase();
    }

    protected override void AttackPattern()
    {
        if (enemyStatus.targetDis <= 2f)
        {
            StartCoroutine(headache());   
        }
        else
        {
            StartCoroutine(peripheralAttack());
        }
    }


    IEnumerator peripheralAttack()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        yield return new WaitForSeconds(0.5f);
        colObj.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        colObj.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 0);
        colObj.GetComponent<HitDetection>().SetSE(FearIndex);
        colObj.GetComponent<SpriteRenderer>().color = Color.white;
        colObj.SetActive(true);
        
        float r = 1;
        while (r < radius)
        {
            colObj.transform.localScale = new Vector3(0.1f * r, 0.1f * r, 1);
            r += 1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(attackTime);

        //no attack
        colObj.SetActive(false);
        yield return new WaitForSeconds(1f);

        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(0.1f);
        enemyStatus.isAttackReady = true;
    }


    IEnumerator headache() 
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        colObj.transform.localScale = new Vector3(0.1f * radius, 0.1f * radius, 1);
        colObj.GetComponent<HitDetection>().SetHitDetection(false,-1,true,2,0,0);
        colObj.GetComponent<HitDetection>().SetSE(FearIndex);
        colObj.GetComponent<SpriteRenderer>().color = Color.magenta;
        colObj.SetActive(true);

        while(enemyStatus.targetDis <= 2f)
        {
            yield return null;
        }

        colObj.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

    }


}
