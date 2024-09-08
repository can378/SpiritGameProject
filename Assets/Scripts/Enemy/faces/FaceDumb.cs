using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDumb : BossFace
{

    //무지=막 돌아다님
    private const float randomMoveCount =5f;
    private float nowCount = 0;
    private float randomX, randomY;

    public Bounds floorBound;
    public GameObject attackArea;

    protected override void Update()
    {
        base.Update();
        attackArea.transform.localPosition = new Vector3(0, 0, 0);
    }

    protected override void MovePattern()
    {
        //enemyStatus.moveVec = new Vector2(-1, 0);
        //Debug.Log(enemyStatus.isFlinch+" "+ enemyStatus.isAttack+" "+ enemyStatus.isRun);
        
        if (nowCount >= 0)
        {
            nowCount-=Time.deltaTime;
        }
        else 
        { 
            enemyStatus.moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;  
            //print("moveVec================"+ enemyStatus.moveVec);
            nowCount = randomMoveCount; 
        }
       
    }

    /*
    IEnumerator MadRush()
    {

        randomX = Random.Range(floorBound.min.x, floorBound.max.x);
        randomY = Random.Range(floorBound.min.y, floorBound.max.y);
        madRushVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;


        float time = 0;
        rigid.velocity = Vector2.zero;


        while (time < 2.0f)
        {
            if (isHitWall == true)
            {
                print("hit wall!");
                isHitWall = false;

                //벽에 부딫힘
                randomX = Random.Range(floorBound.min.x, floorBound.max.x);
                randomY = Random.Range(floorBound.min.y, floorBound.max.y);
                madRushVec = (new Vector3(randomX, randomY, 0) - transform.position).normalized;
                yield return new WaitForSeconds(0.1f);
            }

            rigid.AddForce(madRushVec * GetComponent<EnemyStats>().defaultMoveSpeed * 600);
            yield return new WaitForSeconds(0.05f);
            time += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;

    }
    */
}
