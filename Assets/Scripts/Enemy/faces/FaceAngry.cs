using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : BossFace
{
    //분노=머리랑 귀, 코 에서 불 나오고 랜덤으로 돌아다님

    public List<GameObject> fires;//아니 이게 부모 자식 관계인데 일일히 해줘야하는건지 이해가 안가는데 일단 안되서 이렇게 함;;;
    public GameObject fire;
    private const float randomMoveCount = 3f;
    private float nowCount = 0;

    protected override void Update()
    {
        base.Update();
        fires[0].transform.localPosition = new Vector3(-1.85f, -0.03f, 0);
        fires[1].transform.localPosition = new Vector3(-0.007f, 1.9f, 0);
        fires[2].transform.localPosition = new Vector3(1.72f, -0.001f, 0);

    }
    protected override void MovePattern()
    {
        //Chase();
        print("angry");
        //Debug.Log(enemyStatus.isFlinch+" "+ enemyStatus.isAttack+" "+ enemyStatus.isRun);
        /*
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
        */

        RandomMove();
    }

    protected override void faceAttack()
    {
        base.faceAttack();
        fire.SetActive(true);
       
    }
    protected override void init()
    {
        base.init();
        nowCount = randomMoveCount;
        enemyStatus.moveVec = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;
    }
    protected override void Finish()
    {
        base .Finish();
        fire.SetActive(false);
    }





















    // protected override void Move()
    // {
    //     // 경직 중에는 직접 이동 불가
    //     if (enemyStatus.isFlinch)
    //     {
    //         return;
    //     }
    //     else if (enemyStatus.isRun)
    //     {
    //         if (enemyStatus.enemyTarget)
    //         {
    //             rigid.velocity = -(enemyStatus.enemyTarget.position - transform.position).normalized * stats.moveSpeed;
    //         }
    //         return;
    //     }

    //     MovePattern();

    //     rigid.velocity = enemyStatus.moveVec * stats.moveSpeed;

    // }





    //이거는 왜 넣은것..?
    /*
    IEnumerator angry()
    {
        //enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        foreach(GameObject f in faces) 
        {
            f.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        foreach (GameObject f in faces)
        {
            f.SetActive(false);
        }



        //enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;

    }
    */
}
