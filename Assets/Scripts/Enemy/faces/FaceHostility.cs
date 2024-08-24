using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHostility : BossFace
{
    int bulletCount = 5; // 발사할 총알의 수
    float spreadAngle = 45f; // 부채꼴의 총 각도
    float bulletSpeed = 3f; // 총알의 속도
    float radius;
    float angleStep;
    float startAngle;

    Vector2 circleCenter;
    Vector2 playerCenter;
    Vector2 startPoint;
    Vector2 startDir;
    //적대감=총 부채꼴 모양으로 발사

    protected override void Start()
    {
        radius = 6;
        angleStep = spreadAngle / (bulletCount - 1);
        startAngle = -spreadAngle / 2;
    }
    protected override void AttackPattern()
    {
        print("hostility");
        StartCoroutine(hostility());
    }


    protected override void MovePattern()
    {
        //Chase();
    }

    IEnumerator hostility()
    {
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //START//////////////////////////////////

        // 가장 가까운 지점 계산
        circleCenter = transform.position;
        startPoint = circleCenter + enemyStatus.targetDirVec * radius;

        // 시작 방향 설정
        playerCenter=enemyStatus.enemyTarget.transform.position;
        startDir = (playerCenter - startPoint).normalized;
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;


        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = startPoint;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

            // 각도를 계산하여 벡터를 만듭니다.
            float currentAngle = startAngle + angleStep * i;
            Vector2 dirVec = Quaternion.Euler(0, 0, currentAngle) * startDir;

            bulletRigid.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);
        }






        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(3f);
        enemyStatus.isAttackReady = true;

    }
}
