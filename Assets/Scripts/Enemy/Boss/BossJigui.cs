using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossJigui : EnemyBasic
{
    public GameObject fire;
    public GameObject eyeSight;

    protected override void Start()
    {
        base.Start();
        enemyStatus.isTouchPlayer = false;
    }
    protected override void AttackPattern()
    {
        eyeSight.SetActive(true);
        StartCoroutine(fireBall());
    }

    private void OnDisable()
    {
        eyeSight.SetActive(false);
    }

    protected override void MovePattern()
    {
        if (!enemyStatus.enemyTarget)
        {
            RandomMove();
            print("enemyTarget is null");
        }
        else 
        {
            if (eyeSight.GetComponent<EyeSight>().isPlayerSeeEnemy == false)
            {
                Chase();
            }
            
            if (enemyStatus.targetDis <= 3f)
            {
                enemyStatus.isTouchPlayer = true;
            }
            else if (enemyStatus.targetDis >= 10f && enemyStatus.isTouchPlayer == true)
            {
                enemyStatus.isTouchPlayer = false;
            }

            if (enemyStatus.isTouchPlayer == true)
            { Run(); }

        }
        
    }


    IEnumerator fireBall()
    {

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //Throw fire balls
        for (int i = 0; i < 20; i++)
        {
            GameObject fireBall = ObjectPoolManager.instance.Get("fireBall");
            fireBall.transform.position = transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(3f);

        //Fire Area attack
        fire.SetActive(true);
        fire.transform.position = transform.position;
        fire.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(2f);
        fire.SetActive(false);
        //fire에 플레이어가 닿으면 불붙게하는 너프 적용!!!!!!!!!!!!!


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(5f);
        enemyStatus.isAttackReady = true;
    }



    


}
