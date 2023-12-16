using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : EnemyBasic
{
    bool isJumping=false;
    int jumpSpeed=10;

    void Start()
    {
        
    }


    void Update()
    {

        float targetDistance = Vector2.Distance(transform.position, enemyTarget.position);

        if (targetDistance <= detectionDistance && targetDistance >= 0.2f)
        {
            if (isJumping == false) { JumpToTarget(); }
        }
        else
        {
            //wander();
        }
    }




    void JumpToTarget()
    {
        // 이동 방향
        Vector3 direction = enemyTarget.position - transform.position;
        direction.Normalize();

        // 이동 속도, 시간
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
