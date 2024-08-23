using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]

public class TrailCollider : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public EdgeCollider2D edgeCollider;
    private List<Vector2> trailPoints = new List<Vector2>();

    void Start()
    {
        edgeCollider.isTrigger = true;
    }

    void Update()
    {
        UpdateColliderWithTrail();
    }

    void UpdateColliderWithTrail()
    {
        trailPoints.Clear();

        // TrailRenderer의 모든 점을 가져옴
        for (int i = 0; i < trailRenderer.positionCount; i++)
        {
            Vector3 worldPos = trailRenderer.GetPosition(i);
            // TrailRenderer가 있는 GameObject의 로컬 좌표로 변환
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            trailPoints.Add(localPos);
        }

        // EdgeCollider2D를 업데이트
        if (trailPoints.Count > 1)
        {
            edgeCollider.SetPoints(trailPoints);
        }
        else { edgeCollider.SetPoints(new List<Vector2>()); }
    }

}
