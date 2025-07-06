using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject ghostPrefab;

    [Header("Spawn Settings")]
    public float minRadius = 2f;
    public float startRadius = 6f;
    public float maxRadius = 10f;
    public float shrinkSpeed = 1f;

    public int desiredDensity = 80; // 1 ���� �������� �� ������ ���� ����

    private List<Ghost> ghosts = new List<Ghost>();

    class Ghost
    {
        public GameObject obj;
        public float angle;
        public float radius; // ���� ��ġ
    }

    void Start()
    {
        FillInitialGhosts();
    }

    void Update()
    {
        float shrinkAmount = shrinkSpeed * Time.deltaTime;

        // ������ �������� �̵�
        foreach (var ghost in ghosts)
        {
            ghost.radius -= shrinkAmount;
            ghost.radius = Mathf.Max(ghost.radius, minRadius);

            Vector3 newPos = new Vector3(
                Mathf.Cos(ghost.angle) * ghost.radius,
                Mathf.Sin(ghost.angle) * ghost.radius,
                0
            );
            ghost.obj.transform.localPosition = newPos;
        }

        // ���� ���� �� ���� �е� Ȯ�� �� �����ϸ� ���� ����
        float currentSpan = maxRadius - minRadius;
        int currentGhostCount = ghosts.Count;
        int desiredGhostCount = Mathf.RoundToInt(currentSpan * desiredDensity);

        if (currentGhostCount < desiredGhostCount)
        {
            int toSpawn = desiredGhostCount - currentGhostCount;
            AddGhosts(toSpawn);
        }
    }

    void FillInitialGhosts()
    {
        float areaSpan = maxRadius - startRadius;
        int initCount = Mathf.RoundToInt(areaSpan * desiredDensity);
        AddGhosts(initCount);
    }

    void AddGhosts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Random.Range(startRadius, maxRadius);

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );

            GameObject g = Instantiate(ghostPrefab, pos, Quaternion.identity, transform);
            g.transform.localScale = Vector3.one * 0.6f;

            ghosts.Add(new Ghost
            {
                obj = g,
                angle = angle,
                radius = radius
            });
        }
    }
}
