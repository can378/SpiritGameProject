using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * 장애물을 피해 target위치로 seeker를 가장 최적의 루트로 이동시키는 스크립트
 * 
 * OPEN SET : 평가되어야 할 노드 집합
 * CLOSED SET : 이미 평가된 노드 집합
 * 
 * 1. OPEN SET에서 가장 낮은 F코스트를 가진 노드 획득 후 CLOSED SET 삽입
 * 2. 이 노드가 목적지라면, 반복문 탈출
 * 3. 이 노드의 주변 노드들을 CLOSED SET에 넣고, 주변노드의 F값 계산. 주변노드의 G값보다 작다면 F값으로 G값 최신화
 * 4. 1번 반복.
 */
public class PathFinding : MonoBehaviour
{

    public Transform seeker;//추격자

    AGrid grid;//그리드
    public Queue<Vector2> wayQueue = new Queue<Vector2>();//target까지 가는 방법

    public static bool walkable = true;

    public Vector2 fugitivePos;
    public Vector2 seekerPos;

    public bool isWalking;
    private int moveSpeed;

    private void Awake()
    {
        grid = GetComponent<AGrid>();
        walkable = true;
    }

    private void Start()
    {
        isWalking = false;
        seekerPos = seeker.position;
        moveSpeed = 10;
    }

    private void FixedUpdate()
    {

        if (walkable == true)
        {
            if (seekerPos != fugitivePos)
            {
                //seekerPos = fugitivePos;
                StopAllCoroutines();
                StartCoroutine(FindPath(seeker.position, fugitivePos));
            }
        }
        else
        {
            seekerPos = (Vector2)seeker.position;
        }

    }





    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {   

        ANode startNode = grid.NodeFromWorldPoint(startPos);
        ANode targetNode = grid.NodeFromWorldPoint(targetPos);
        
        
        bool pathSuccess = false;// target에 도착했는지

        if (!startNode.walkable)
            Debug.Log("Unwalkable StartNode.");

        
        if(targetNode.walkable)
        {

            List<ANode> openSet = new List<ANode>(); //계산한 노드
            HashSet<ANode> closedSet = new HashSet<ANode>();//계산할 노드

            openSet.Add(startNode);

            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                ANode currentNode = openSet[0];
                // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                // openSet에서 current를 뺀 후, closedSet에 추가.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // 방금 들어온 노드가 목적지 인 경우
                if (currentNode == targetNode)
                {
                    // seeker가 위치한 지점이 target이 아닌 경우
                    if(pathSuccess == false)
                    {
                       // wayQueue에 PATH를 넣어준다.
                       PushWay( RetracePath(startNode, targetNode) ) ;
                    }
                    pathSuccess = true;
                    break;
                }

                // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
                foreach (ANode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    // F cost 생성.
                    int fCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                    if (fCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = fCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet에 추가.
                        if (!openSet.Contains(neighbour)){   openSet.Add(neighbour);  }
                    }
                }
            }
        }
        else //목적지에 도달할 수 없을경우
        {
            // way갱신 안함
            Vector3 origin = seeker.position;
            while (true)
            {
                seeker.position = Vector2.MoveTowards(seeker.position, origin, moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.03f);
                if ((int)seeker.position.x == (int)origin.x && (int)seeker.position.y == (int)origin.y) break;
            }

            wayQueue.Clear();
        }
        
        yield return null;

        // 길을 찾으면 이동
        if(pathSuccess == true)
        {

            isWalking = true;

            while (wayQueue.Count > 0)
            {
                seeker.position = Vector2.MoveTowards(seeker.position, wayQueue.First(), moveSpeed * Time.deltaTime);
                if ((Vector2)seeker.position == wayQueue.First())
                {
                    wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }

            isWalking = false;
        }
    }















    // WayQueue에 새로운 PATH를 넣어준다.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array)
        {
            wayQueue.Enqueue(item);
        }
    }

    // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
    Vector2[] RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode  = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Grid의 path에 찾은 길을 등록한다.
        grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node에서 Vector 정보만 빼낸다.
    Vector2[] SimplifyPath(List<ANode> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 0; i < path.Count; i++)
        {
            wayPoints.Add(path[i].worldPosition);
        }
        return wayPoints.ToArray();
    }

    // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
    // 매개변수로 들어오는 값에 따라 기능이 바뀝니다.
    int GetDistance(ANode nodeA, ANode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // 대각선 - 14, 상하좌우 - 10.
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}