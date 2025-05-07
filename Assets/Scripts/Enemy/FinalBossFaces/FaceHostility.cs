using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHostility : BossFace
{
    int bulletCount = 5; // �߻��� �Ѿ��� ��
    float spreadAngle = 45f; // ��ä���� �� ����
    float bulletSpeed = 3f; // �Ѿ��� �ӵ�
    float radius=6;


    Vector2 circleCenter;
    Vector2 playerCenter;
    Vector2 startPoint;
    Vector2 startDir;
    //���밨=�� ��ä�� ������� �߻�

    private bool isReady=true;


    protected override void faceAttack()
    {
        if(isReady) { StartCoroutine(hostility()); }
    }

    IEnumerator hostility() 
    {
        isReady = false;
        print("hostility");

        // ���� ����� ���� ���
        circleCenter = transform.position;
        startPoint = circleCenter + enemyStatus.targetDirVec * radius;

        // ���� ���� ����
        playerCenter = enemyStatus.EnemyTarget.transform.position;
        startDir = (playerCenter - startPoint).normalized;
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;


        for (int i = 0; i < bulletCount; i++)
        {
            //create bullet
            GameObject bullet = ObjectPoolManager.instance.Get("Bullet");
            bullet.transform.position = startPoint;
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();

            //calculate angle
            float currentAngle = startAngle + angleStep * i;
            Vector2 dirVec = Quaternion.Euler(0, 0, currentAngle) * startDir;

            //launch angle
            bulletRigid.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(3f);
        isReady = true;
    }

    
}
