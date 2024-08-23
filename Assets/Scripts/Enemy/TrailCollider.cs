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

        // TrailRenderer�� ��� ���� ������
        for (int i = 0; i < trailRenderer.positionCount; i++)
        {
            Vector3 worldPos = trailRenderer.GetPosition(i);
            // TrailRenderer�� �ִ� GameObject�� ���� ��ǥ�� ��ȯ
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            trailPoints.Add(localPos);
        }

        // EdgeCollider2D�� ������Ʈ
        if (trailPoints.Count > 1)
        {
            edgeCollider.SetPoints(trailPoints);
        }
        else { edgeCollider.SetPoints(new List<Vector2>()); }
    }

}
