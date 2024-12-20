using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public static AGrid instance = null;

    public bool displayGridGizmos;

    //public Transform seeker;
   
    public LayerMask OBSTACLE;
    // 화면의 크기
    public Vector2 gridWorldSize;

    //node radius
    public float nodeRadius;
    ANode[,] grid;

    // node diameter
    float nodeDiameter;
    // x,y축 사이즈
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    private void Start()
    {
        // 격자 생성
        //CreateGrid();
    }

    // A*에서 사용할 PATH.
    [SerializeField]
    public List<ANode> path;

    // Scene view 출력용 Gizmo.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            //ANode seekerNode = NodeFromWorldPoint(seeker.position);
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
                //seekerNode(blue)
                //if (seekerNode == n) Gizmos.color = Color.cyan * new Color(1f, 1f, 1f, 0.5f);
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }

    // 격자 생성 함수
    public void CreateGrid()
    {
        grid = new ANode[gridSizeX, gridSizeY];
        // creating grid-> left bottom start / transform->world center 
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                // 해당 격자가 Walkable한지 아닌지 판단.
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, OBSTACLE));
                // 노드 할당.
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
    }

    // node 상하 좌우 노드 반환
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

    // 입력으로 들어온 월드좌표를 node좌표계로 변환.
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
