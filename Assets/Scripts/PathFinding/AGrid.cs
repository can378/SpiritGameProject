using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public bool displayGridGizmos;
    // �÷��̾��� ��ġ
    public Transform player;
    // ��ֹ� ���̾�
    public LayerMask OBSTACLE;
    // ȭ���� ũ��
    public Vector2 gridWorldSize;
    // ������
    public float nodeRadius;
    ANode[,] grid;

    // ������ ����
    float nodeDiameter;
    // x,y�� ������
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // ���� ����
        CreateGrid();
    }

    // A*���� ����� PATH.
    [SerializeField]
    public List<ANode> path;

    // Scene view ��¿� �����.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            ANode playerNode = NodeFromWorldPoint(player.position);
            foreach (ANode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white * new Color(1f, 1f, 1f, 0.5f) : Color.red * new Color(1f, 1f, 1f, 0.5f);
                if (n.walkable == false)

                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black * new Color(1f, 1f, 1f, 0.5f);
                            Debug.Log("?");

                        }
                    }
                if (playerNode == n) Gizmos.color = Color.cyan * new Color(1f, 1f, 1f, 0.5f);
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }

    // ���� ���� �Լ�
    void CreateGrid()
    {
        grid = new ANode[gridSizeX, gridSizeY];
        // ���� ������ ���� ���ϴܺ��� ����. transform�� ���� �߾ӿ� ��ġ�Ѵ�. 
        // �̿� x�� y��ǥ�� �ݹ� �� ����, �Ʒ������� �Ű��ش�.
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                // �ش� ���ڰ� Walkable���� �ƴ��� �Ǵ�.
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, OBSTACLE));
                // ��� �Ҵ�.
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
    }

    // node ���� �¿� ��带 ��ȯ�ϴ� �Լ�.
    public List<ANode> GetNeighbours(ANode node)
    {
        List<ANode> neighbours = new List<ANode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // �Է����� ���� ������ǥ�� node��ǥ��� ��ȯ.
    public ANode NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
}
