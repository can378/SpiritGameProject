using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FaceGreed : BossFace
{

    public float lineWidth = 2f; // 선의 두께


    public LineRenderer lineRenderer;
    private List<Vector3> positions; // 선을 그릴 위치들의 리스트


    //탐욕=입을 크게 벌린 상태로 쫓아온다. 이와중에 나온 혓바닥에 닿으면 안된다.
    //혓바닥은 지워지지않고 이 페이즈 끝날때까지 남아있어서 무빙을 잘 생각하고 쳐야한다.
    protected override void Start()
    {
        base.Start();


        //lineRenderer = gameObject.GetComponent<LineRenderer>();

        // 선 두께,material
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // 선 init
        lineRenderer.positionCount = 0;
    }



    protected override void init()
    {
        base.init();
        lineRenderer.positionCount = 0;
        positions = new List<Vector3>();
    }





    protected override void MovePattern()
    {
        Chase();
    }

    protected override void AttackPattern()
    {
        base.AttackPattern();
        if (nowAttack && isFaceAttack)
        {
            if (countTime <=1f && isReady)
            {
                StartCoroutine(ClearLineGradually());
            }
        }

    }
    protected override void faceAttack()
    {
        // 매 프레임마다 선의 끝 점을 목표로 업데이트
        //endPoint=transform;
        //lineRenderer.SetPosition(0, startPoint.position);
        //lineRenderer.SetPosition(1, endPoint.position);

        /*
        // 현재 위치를 positions에 추가
        positions.Add(transform.position);

        // Line
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
        */

        // 이전 위치와의 거리 계산
        if (positions.Count == 0)
        {
            // 일정 거리 이상 이동했을 때만 positions에 추가
            positions.Add(transform.position);


        }
        else 
        { 
        if (Vector3.Distance(positions[positions.Count - 1], transform.position) >= 0.5f)
        {
            // 일정 거리 이상 이동했을 때만 positions에 추가
            positions.Add(transform.position);

            // LineRenderer를 갱신
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
        }

    }
    private bool isReady = true;
    public IEnumerator ClearLineGradually()
    {
        isReady = false;

        float duration = 0.3f; // 전체 삭제 시간
        float interval = duration / positions.Count; // 각 점이 사라지는 간격

        while (positions.Count > 0)
        {
            // 가장 오래된 점을 제거
            positions.RemoveAt(0);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            yield return new WaitForSeconds(interval);

        }
        lineRenderer.positionCount = 0;
        isReady = true;
    }




}
