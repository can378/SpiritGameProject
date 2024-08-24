using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHostility : BossFace
{
    int bulletCount = 5; // �߻��� �Ѿ��� ��
    float spreadAngle = 45f; // ��ä���� �� ����
    float bulletSpeed = 3f; // �Ѿ��� �ӵ�
    float radius;
    float angleStep;
    float startAngle;

    Vector2 circleCenter;
    Vector2 playerCenter;
    Vector2 startPoint;
    Vector2 startDir;
    //���밨=�� ��ä�� ������� �߻�

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

        // ���� ����� ���� ���
        circleCenter = transform.position;
        startPoint = circleCenter + enemyStatus.targetDirVec * radius;

        // ���� ���� ����
        playerCenter=enemyStatus.enemyTarget.transform.position;
        startDir = (playerCenter - startPoint).normalized;
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;


        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = ObjectPoolManager.instance.Get2("Bullet");
            bullet.transform.position = startPoint;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

            // ������ ����Ͽ� ���͸� ����ϴ�.
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
