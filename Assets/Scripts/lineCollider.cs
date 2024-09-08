using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]

public class lineCollider : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    private List<Vector2> linePoints = new List<Vector2>();

    void Start()
    {
        edgeCollider.isTrigger = true;
    }

    void Update()
    {
        UpdateColliderWithLine();
    }

    void UpdateColliderWithLine()
    {
        linePoints.Clear();

        // LineRenderer의 모든 점을 가져옴
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 worldPos = lineRenderer.GetPosition(i);
            // LineRenderer가 있는 GameObject의 로컬 좌표로 변환
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            linePoints.Add(localPos);
        }

        // EdgeCollider2D를 업데이트
        if (linePoints.Count > 1)
        {
            edgeCollider.SetPoints(linePoints);
        }
        else
        {
            edgeCollider.SetPoints(new List<Vector2>());
        }
    }
}