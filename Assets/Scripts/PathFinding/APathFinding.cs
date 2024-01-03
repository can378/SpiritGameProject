using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathFinding : MonoBehaviour
{
    public Transform Atarget; // 목표
    public LayerMask obstacleLayer; // 장애물 레이어
    public float nodeRadius = 0.3f; // 노드 반지름
    public float moveSpeed = 5f; // 이동 속도

    private List<Node> path; // 최종 경로
    private int currentPathIndex = 0; // 현재 경로 인덱스

    private void Start()
    {
        FindPath((Vector2)this.transform.position, (Vector2)Atarget.position);
    }

    private void Update()
    {
        if (path == null || path.Count == 0)
            return;

        // 현재 목표 지점까지 이동
        Vector2 direction = ((Vector2)path[currentPathIndex].worldPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // 목표 지점에 도달했으면 다음 경로로 이동
        if (Vector2.Distance((Vector2)transform.position, path[currentPathIndex].worldPosition) < 0.1f)
        {
            currentPathIndex++;

            if (currentPathIndex >= path.Count)
                path = null; // 경로를 모두 도달하면 초기화
        }
    }

    // A* 알고리즘을 사용하여 경로 찾기
    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || 
                    (openSet[i].fCost == currentNode.fCost && 
                    openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
            
        }

    }

    // 최종 경로 추적
    void RetracePath(Node startNode, Node endNode)
    {
        path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse(); // 시작부터 끝까지의 순서로 변환
        currentPathIndex = 0;
    }

    // 노드의 이웃 찾기
    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < AGridManager.instance.gridSizeX && checkY >= 0 && checkY < AGridManager.instance.gridSizeY)
                {
                    Node neighbor = AGridManager.instance.grid[checkX, checkY];
                    if (!Physics2D.OverlapCircle(neighbor.worldPosition, nodeRadius, obstacleLayer))
                        neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    // 두 노드 간의 거리 계산
    float GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return Mathf.Sqrt(distX * distX + distY * distY);
    }

    // 월드 포인트에 해당하는 노드 찾기
    Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + AGridManager.instance.gridWorldSize.x / 2) / AGridManager.instance.gridWorldSize.x;
        float percentY = (worldPosition.y + AGridManager.instance.gridWorldSize.y / 2) / AGridManager.instance.gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((AGridManager.instance.gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((AGridManager.instance.gridSizeY - 1) * percentY);

        return AGridManager.instance.grid[x, y];
    }
}
