using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomStick : EnemyBasic
{
    public GameObject colObj;
    int[] debuff = {7};
    float radius = 50;
    float attackTime = 2;

    protected override void AttackPattern()
    {
        if (targetDis <= 1f)
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
        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        //attack
        //colObj.transform.position = transform.position;
        colObj.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        colObj.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 0, 0, 0, debuff);
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
        yield return new WaitForSeconds(2f);

        isAttack = false;
        yield return new WaitForSeconds(0.1f);
        isAttackReady = true;
    }


    IEnumerator headache() 
    {
        isAttack = true;
        isAttackReady = false;

        colObj.transform.localScale = new Vector3(0.1f * radius, 0.1f * radius, 1);
        colObj.GetComponent<HitDetection>().SetHitDetection(false,-1,true,2,0,0,0,0, debuff);
        colObj.GetComponent<SpriteRenderer>().color = Color.magenta;
        colObj.SetActive(true);

        while(targetDis <= 1f)
        {
            yield return null;
        }

        colObj.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        isAttackReady = true;

    }


}
