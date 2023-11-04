using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : EnemyBasic
{
    bool isJumping=false;
    Vector2 jumpStartPosition;

    int jumpSpeed=10;

    void Start()
    {
        
    }


    void Update()
    {

        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        //target과 가까워지면 이동
        if (targetDistance <= 7 && targetDistance >= 0f)
        {
            if (isJumping == false) { JumpToTarget(); }
        }
        else //그 외에는 혼자 배회
        {
            //JumpWander();
        }
    }



    //target으로 이동
    void JumpToTarget()
    {
        // 이동 방향 계산
        Vector3 direction = enemyTarget.position - transform.position;
        direction.Normalize();

        // 이동 속도와 시간 설정
        float jumpDuration = Vector3.Distance(enemyTarget.position, transform.position) / jumpSpeed;

        
        // 점프 시작
        StartCoroutine(JumpCoroutine(direction, jumpDuration));
    }

    IEnumerator JumpCoroutine(Vector3 direction, float duration)
    {
        isJumping = true;
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.Translate(direction * (Time.deltaTime / duration) * jumpSpeed);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // 점프 완료 후 상태 초기화
        isJumping = false;
    }

}
