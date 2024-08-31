using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FaceAngry : BossFace
{
    //분노=머리랑 귀, 코 에서 불 나오고 랜덤으로 돌아다님

    //public List<GameObject> faces;
    public List<GameObject> fires;
    private bool isReady=false;

    protected override void MovePattern()
    {
        if (nowAttack) { RandomMove(); }
    }

    protected override void faceAttack()
    {
        base.faceAttack();
        if(isReady) { StartCoroutine(angry()); }
        
    }

    IEnumerator angry() 
    {
        isReady = false;
        foreach (GameObject fire in fires)
        { 
            fire.SetActive(true);
        }

        yield return new WaitForSeconds(3f); 
        isReady = true;
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
