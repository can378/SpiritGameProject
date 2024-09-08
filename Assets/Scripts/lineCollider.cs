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

        // LineRenderer�� ��� ���� ������
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 worldPos = lineRenderer.GetPosition(i);
            // LineRenderer�� �ִ� GameObject�� ���� ��ǥ�� ��ȯ
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            linePoints.Add(localPos);
        }

        // EdgeCollider2D�� ������Ʈ
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