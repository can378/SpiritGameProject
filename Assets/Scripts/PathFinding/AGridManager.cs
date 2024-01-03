using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGridManager : MonoBehaviour
{
    public static AGridManager instance; // 싱글톤 인스턴스

    public Vector2 gridWorldSize; // 그리드의 월드 공간 크기
    public float nodeRadius; // 노드 반지름
    public LayerMask obstacleLayer; // 장애물 레이어

    public Node[,] grid; // 그리드 배열
    public int gridSizeX, gridSizeY; // 그리드의 가로 및 세로 크기

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateGrid();
    }

    // 그리드 생성
    void CreateGrid()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeRadius * 2));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (nodeRadius * 2));

        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = 
            (Vector2) this.transform.position - 
            Vector2.right * gridWorldSize.x / 2 - 
            Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = 
                    worldBottomLeft + 
                    Vector2.right * (x * nodeRadius * 2 + nodeRadius) + 
                    Vector2.up * (y * nodeRadius * 2 + nodeRadius);
                bool isObstacle = Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleLayer);
                grid[x, y] = new Node(isObstacle, worldPoint, x, y);
                
                
            }
        }
    }

    // 월드 포인트에 해당하는 노드 찾기
    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    // 노드 시각화
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.isObstacle) ? Color.red : Color.white;
                Gizmos.DrawCube(node.worldPosition, Vector2.one * (nodeRadius * 2 - 0.1f));
            }
        }
    }
}
