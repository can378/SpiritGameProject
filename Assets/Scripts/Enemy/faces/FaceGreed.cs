using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGreed : EnemyBasic
{
    private Transform startPoint; // 선이 시작될 위치
    public float lineWidth = 2f; // 선의 두께

    private LineRenderer lineRenderer;


    //탐욕=입을 크게 벌린다. 이와중에 혓바닥이 길어지는데 혓바닥에 닿으면 안된다.
    //혓바닥은 지워지지않고 이 페이즈 끝날때까지 남아있어서 무빙을 잘 생각하고 쳐야한다.
    protected override void Start()
    {

        startPoint = transform;

        lineRenderer = gameObject.GetComponent<LineRenderer>();

        // 선 두께
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // 머티리얼
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // 시작 시점에 선의 위치 초기화
        lineRenderer.positionCount = 2;
    }
    protected override void AttackPattern()
    {
        print("greed1");
        StartCoroutine(greed());
    }

    IEnumerator greed()
    {
        print("greed2");
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //START
        startPoint = transform;

        // 매 프레임마다 선의 끝 점을 목표로 업데이트
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, enemyStatus.enemyTarget.position);


        //END
        enemyStatus.isAttack = false;
        yield return new WaitForSeconds(1f);
        enemyStatus.isAttackReady = true;

    }
}
