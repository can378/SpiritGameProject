using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * OPEN SET : 평가되어야 할 노드
 * CLOSED SET : 평가 완료 노드
 * 
 * 1. OPEN SET에서 가장 낮은 F cost를 가진 노드를 CLOSED SET 삽입
 * 2. 이 노드가 목적지라면, 반복문 break
 * 3. 이 노드의 neighbour 노드들을 CLOSED SET에 넣고, 주변노드의 F값 계산. 주변노드의 G값보다 작다면 F값으로 G값 최신화
 * 4. 1번 반복.
 */
public class PathFinding : MonoBehaviour
{

    public static PathFinding instance=null;

    [HideInInspector]
    public Transform seeker;//추격자
    [HideInInspector]
    public Transform target;
    
    public Queue<Vector2> wayQueue = new Queue<Vector2>();//target까지 가는 방법

    public Vector2 fugitivePos;


    private int seekerSpeed;
    AGrid grid;//그리드




    private void Awake()
    {
        instance = this;
        seekerSpeed = 30;
        grid = GameObject.Find("AGridManager").GetComponent<AGrid>();
        target=FindObj.instance.Player.transform;
        seeker = this.transform;
        //grid = GetComponent<AGrid>();
    }



    private void FixedUpdate()
    {
        //print("seeker=" + seekerPos + "fugitive="+fugitivePos);
        
        fugitivePos = target.position;
        StartFinding(seeker.position, fugitivePos);
    }

    public void StartFinding(Vector2 startPos, Vector2 targetPos)
    {
        
        if (startPos != targetPos)
        {
            StopAllCoroutines();
            //print("seeker=" + startPos + "figitive=" + targetPos);
            StartCoroutine(FindPath(startPos,targetPos));
        }
    }



    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        
        yield return null;
        
        ANode startNode = grid.NodeFromWorldPoint(startPos);
        ANode targetNode = grid.NodeFromWorldPoint(targetPos);
        

        bool pathSuccess = false;// target 도착여부

        //도달 불가
        if (!startNode.walkable) {   Debug.Log("Unwalkable StartNode.");  }
        if (!targetNode.walkable) 
        { 
            print("Unwalkable TargetNode."); 
            targetNode=grid.NodeFromWorldPoint(FindObj.instance.playerMazePos.transform.position);
        }
        
        
        //길 찾기
        //if (targetNode.walkable) {
            List<ANode> openSet = new List<ANode>(); //계산한 노드
            HashSet<ANode> closedSet = new HashSet<ANode>();//계산할 노드

            openSet.Add(startNode);
            
            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                ANode currentNode = openSet[0];
                // openSet 내에서 current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || 
                        openSet[i].fCost == currentNode.fCost && 
                        openSet[i].hCost < currentNode.hCost)
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
        /*
        }
        else //목적지에 도달할 수 없을경우 way갱신 안함
        {
            Vector3 origin = seeker.transform.position;
            while (true)
            {
                seeker.transform.position = Vector2.MoveTowards(seeker.transform.position, origin, seekerSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.03f);
                if ((int)seeker.transform.position.x == (int)origin.x && (int)seeker.transform.position.y == (int)origin.y) break;
            }

            wayQueue.Clear();
        }
        */
        yield return null;


        // 길을 찾은 후 이동
        if (pathSuccess == true)
        {
            
            while (wayQueue.Count > 0)
            {
                seeker.transform.position = Vector2.MoveTowards(seeker.transform.position, wayQueue.First(), seekerSpeed * Time.deltaTime);
                if ((Vector2)seeker.transform.position == wayQueue.First())
                {
                    wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }

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

    // queue를 역순 저장 
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

    // custom g cost or heuristic cost 계산
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