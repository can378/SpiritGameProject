using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * ��ֹ��� ���� target��ġ�� seeker�� ���� ������ ��Ʈ�� �̵���Ű�� ��ũ��Ʈ
 * 
 * OPEN SET : �򰡵Ǿ�� �� ��� ����
 * CLOSED SET : �̹� �򰡵� ��� ����
 * 
 * 1. OPEN SET���� ���� ���� F�ڽ�Ʈ�� ���� ��� ȹ�� �� CLOSED SET ����
 * 2. �� ��尡 ���������, �ݺ��� Ż��
 * 3. �� ����� �ֺ� ������ CLOSED SET�� �ְ�, �ֺ������ F�� ���. �ֺ������ G������ �۴ٸ� F������ G�� �ֽ�ȭ
 * 4. 1�� �ݺ�.
 */
public class PathFinding : MonoBehaviour
{

    public Transform seeker;//�߰���

    AGrid grid;//�׸���
    public Queue<Vector2> wayQueue = new Queue<Vector2>();//target���� ���� ���

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
        
        
        bool pathSuccess = false;// target�� �����ߴ���

        if (!startNode.walkable)
            Debug.Log("Unwalkable StartNode.");

        
        if(targetNode.walkable)
        {

            List<ANode> openSet = new List<ANode>(); //����� ���
            HashSet<ANode> closedSet = new HashSet<ANode>();//����� ���

            openSet.Add(startNode);

            // closedSet���� ���� ������ F�� ������ ��带 ������. 
            while (openSet.Count > 0)
            {
                // currentNode�� ��� �� openSet���� ���� �Ѵ�.
                ANode currentNode = openSet[0];
                // ��� openSet�� ����, current���� f���� �۰ų�, h(�޸���ƽ)���� ������ �װ��� current�� ����.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                // openSet���� current�� �� ��, closedSet�� �߰�.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // ��� ���� ��尡 ������ �� ���
                if (currentNode == targetNode)
                {
                    // seeker�� ��ġ�� ������ target�� �ƴ� ���
                    if(pathSuccess == false)
                    {
                       // wayQueue�� PATH�� �־��ش�.
                       PushWay( RetracePath(startNode, targetNode) ) ;
                    }
                    pathSuccess = true;
                    break;
                }

                // current�� �����¿� ���鿡 ���Ͽ� g,h cost�� ����Ѵ�.
                foreach (ANode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    // F cost ����.
                    int fCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // �̿����� ���� F cost�� �̿��� G���� ª�ų�, �湮�غ� Openset�� �� ���� ���ٸ�,
                    if (fCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = fCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet�� �߰�.
                        if (!openSet.Contains(neighbour)){   openSet.Add(neighbour);  }
                    }
                }
            }
        }
        else //�������� ������ �� �������
        {
            // way���� ����
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

        // ���� ã���� �̵�
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















    // WayQueue�� ���ο� PATH�� �־��ش�.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array)
        {
            wayQueue.Enqueue(item);
        }
    }

    // ���� ť�� �Ųٷ� ����Ǿ������Ƿ�, �������� wayQueue�� �������ش�. 
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
        // Grid�� path�� ã�� ���� ����Ѵ�.
        grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node���� Vector ������ ������.
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

    // custom g cost �Ǵ� �޸���ƽ ����ġ�� ����ϴ� �Լ�.
    // �Ű������� ������ ���� ���� ����� �ٲ�ϴ�.
    int GetDistance(ANode nodeA, ANode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // �밢�� - 14, �����¿� - 10.
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}