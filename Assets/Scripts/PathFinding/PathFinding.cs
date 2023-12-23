using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * �Է¹��� ��ֹ��� ���� target��ġ�� �÷��̾�(seeker)�� ���� ������ ��Ʈ�� �̵���Ű�� ��ũ��Ʈ �Դϴ�.
 * 
 * �� Ŭ������ ȭ���� grids�� ������� �մϴ�. �̸� ���� Ŀ����Ŭ���� Grid�� Node class�� ����մϴ�.
 * 
 * ����, Astar prefab�� Hierarchy view�� �ű� �� position�� 0,0,0���� ����, �׸���
 * seeker�� Player�� ����ϸ� �˴ϴ�.
 * 
 * �˰��� ����
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
  [Header("Path Finding")]
    public Transform seeker;
    // ���� ���ڷ� �����Ѵ�.
    AGrid grid;
    // �����Ÿ��� ���� ť ����.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    [Header("Player Ctrl")]

    // ������ ��ȣ�ۿ� �ϰ� �������� walkable�� false �� ��.
    public static bool walkable = true;
    // ��ġ ��ǥ�� ������ ����
    public Vector2 touchOrigin;
    public Vector2 touchSave;
    private Quaternion playerRot;
    // �÷��̾� �̵�/ȸ�� �ӵ� �� ������ ����
    public float moveSpeed;
    public float rotSpeed;
    // ��ֹ�/NPC �Ǵܽ� ���߰� �� ����
    public float range;

    public bool isWalk;
    public bool isWalking;

    private void Awake()
    {
        // ���� ����
        grid = GetComponent<AGrid>();
        walkable = true;
    }
    private void Start()
    {
        // �ʱ갪 �ʱ�ȭ.
        isWalking = false;
        moveSpeed = 20f;
        rotSpeed = 5f;
        touchSave = seeker.position;
        touchOrigin = seeker.position;
        range = 4f;
        playerRot = transform.rotation;
        // story 0-1���� ĳ���� �ڵ��̵��̸�, �̶� �ӵ��� 10���� ����.
        if (SceneManager.GetActiveScene().name == "Story0-1")
            moveSpeed = 10f;
    }

    private void FixedUpdate()
    {
        isWalk = walkable;
        // walkable = true �� ��쿡�� ��ġ ���� 
        if (walkable == true)
        {
            CheckTouch();
        }
        // walkable�� false �� ���ڸ� ����
        else
        {
            touchSave = (Vector2)seeker.position;
        }
    }

    // ȭ���� ��ġ �Ǿ����� �Ǵ��ϴ� �Լ�.
    private void CheckTouch()
    {
        // ȭ�鿡 ��ġ�� �����Ǿ��� ���
        if (Input.touchCount > 0)
        {
            // �Ʒ� �ڵ�� ȭ�� �󿡼� UI�� ��ġ ���� �ʾ��� ���(��, ��ư ���� Ŭ������ �ʾ��� ��쿡�� �����̰� ��.)
            // ������ �ش� ���� EventSystem�� �־�� ��. ����, eventsystem�� ���ٸ� �߰��ؾ� ��.
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Touch myTouch = Input.touches[0];
                // ó�� ��ġ ���� | ������ ��ġ ������ ����
                if ((myTouch.phase == TouchPhase.Began))
                {
                    touchOrigin = myTouch.position;
                }

                // �Է¹��� ��ġ�� ȭ����ǥ�� -> ������ǥ��� ��ȯ
                touchOrigin = Camera.main.ScreenToWorldPoint(myTouch.position);
            }
        }

        // �� ��ü�� ��ġ�� �������� �̵���ŵ�ϴ�.
        if(touchSave != touchOrigin)
        {
            touchSave = touchOrigin;
            //��ġ �ʱ� ��ġ�� �ٸ� ��ǥ�� �ԷµǾ��� ���, ��ã�⸦ ����.
            StartFindPath(seeker.position, touchSave);
        }
    }

    // start to target �̵�.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // ��ã�� ����.
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {   
        // start, target�� ��ǥ�� grid�� ������ ��ǥ�� ����.
        ANode startNode = grid.NodeFromWorldPoint(startPos);
        ANode targetNode = grid.NodeFromWorldPoint(targetPos);
        
        // target�� �����ߴ��� Ȯ���ϴ� ����.
        bool pathSuccess = false;

        if (!startNode.walkable)
            Debug.Log("Unwalkable StartNode �Դϴ�.");

        // walkable�� targetNode�� ��� ��ã�� ����.
        if(targetNode.walkable)
        {
            // openSet, closedSet ����.
            // closedSet�� �̹� ��� ����� ����.
            // openSet�� ����� ��ġ�� �ִ� ����.
            List<ANode> openSet = new List<ANode>();
            HashSet<ANode> closedSet = new HashSet<ANode>();

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
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // �̿����� ���� F cost�� �̿��� G���� ª�ų�, �湮�غ� Openset�� �� ���� ���ٸ�,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet�� �߰�.
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        else
        {
            // �� ���ٰ� Unwalkable ������Ʈ�� Ŭ���� ��� ���� PATH�� ���󰣴�.
            // �׷��� way �ֽ�ȭ�� ���� �ʰ� clear�Ѵ�.
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

        // ���� ã���� ���(��� �� �������) �̵���Ŵ.
        if(pathSuccess == true)
        {
            // �̵����̶�� ���� ON
            isWalking = true;
            // wayQueue�� ���� �̵���Ų��.
            while (wayQueue.Count > 0)
            {
                seeker.position = Vector2.MoveTowards(seeker.position, wayQueue.First(), moveSpeed * Time.deltaTime);
                if ((Vector2)seeker.position == wayQueue.First())
                {
                    wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }
            // �̵����̶�� ���� OFF
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