using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isObstacle; // ��ֹ� ����
    public Vector3 worldPosition; // ���� ��ǥ
    public int gridX, gridY; // �׸��� ���� ��ǥ

    public float gCost; // ���� �������κ����� ���
    public float hCost; // ��ǥ ���������� �޸���ƽ ���
    public Node parent; // ��� ������ ���� �θ� ���

    public float fCost
    {
        get { return gCost + hCost; } // fCost�� gCost�� hCost�� ��
    }

    public Node(bool isObstacle, Vector2 worldPosition, int gridX, int gridY)
    {
        this.isObstacle = isObstacle;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
