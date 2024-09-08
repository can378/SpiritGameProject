using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAngry : BossFace
{
    //�г�=�Ӹ��� ��, �� ���� �� ������ �������� ���ƴٴ�

    public List<GameObject> fires;//�ƴ� �̰� �θ� �ڽ� �����ε� ������ ������ϴ°��� ���ذ� �Ȱ��µ� �ϴ� �ȵǼ� �̷��� ��;;;
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
    //     // ���� �߿��� ���� �̵� �Ұ�
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





    //�̰Ŵ� �� ������..?
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
