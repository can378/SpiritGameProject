using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGridManager : MonoBehaviour
{
    public static AGridManager instance; // �̱��� �ν��Ͻ�

    public Vector2 gridWorldSize; // �׸����� ���� ���� ũ��
    public float nodeRadius; // ��� ������
    public LayerMask obstacleLayer; // ��ֹ� ���̾�

    public Node[,] grid; // �׸��� �迭
    public int gridSizeX, gridSizeY; // �׸����� ���� �� ���� ũ��

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateGrid();
    }

    // �׸��� ����
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

    // ���� ����Ʈ�� �ش��ϴ� ��� ã��
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

    // ��� �ð�ȭ
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
