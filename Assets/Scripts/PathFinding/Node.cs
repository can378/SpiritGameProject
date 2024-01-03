using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isObstacle; // 장애물 여부
    public Vector3 worldPosition; // 월드 좌표
    public int gridX, gridY; // 그리드 상의 좌표

    public float gCost; // 시작 지점으로부터의 비용
    public float hCost; // 목표 지점까지의 휴리스틱 비용
    public Node parent; // 경로 추적을 위한 부모 노드

    public float fCost
    {
        get { return gCost + hCost; } // fCost는 gCost와 hCost의 합
    }

    public Node(bool isObstacle, Vector2 worldPosition, int gridX, int gridY)
    {
        this.isObstacle = isObstacle;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
