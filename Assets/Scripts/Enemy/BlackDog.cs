using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDog : EnemyBasic
{
    private bool isRunaway=false;

    protected override void MovePattern()
    {
        
    }

    protected override void AttackPattern()
    {

    }

    //플레이어 마우스가 근처에 있으면 원형으로 도망간다.
    // 플레이어 마우스가 근처에 없다면 슬금슬금 다가오다 한대 치고 도망간다.


    IEnumerator blackDog() 
    {

        Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDir = (enemyTarget.position - transform.position).normalized;
        Vector3 playerForward = (enemyTarget.position - mousePos);
        float angle = Vector3.Angle(playerForward, targetDir);


        if (Vector2.Distance(enemyTarget.position, transform.position) < 0.5f)
        { isRunaway = true; }



        if (angle < 70f)
        {

            Vector2 perpendicularDir = new Vector2(targetDir.y, -targetDir.x).normalized;
            rigid.AddForce(perpendicularDir * GetComponent<EnemyStats>().defaultMoveSpeed * 100);


        }
        else 
        {
            
            if (isRunaway) 
            {
                targetDirVec= (enemyTarget.position - transform.position).normalized;
                rigid.AddForce(-targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed);
                
                if (Vector2.Distance(enemyTarget.position, transform.position) > 12f)
                { isRunaway = false;}
            }
            else 
            {
                //Chase(); 
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(blackDog());
    
    }



}
