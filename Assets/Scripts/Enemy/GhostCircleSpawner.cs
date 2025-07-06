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

    public int desiredDensity = 80; // 1 유닛 반지름당 몇 마리의 유령 유지

    private List<Ghost> ghosts = new List<Ghost>();

    class Ghost
    {
        public GameObject obj;
        public float angle;
        public float radius; // 현재 위치
    }

    void Start()
    {
        FillInitialGhosts();
    }

    void Update()
    {
        float shrinkAmount = shrinkSpeed * Time.deltaTime;

        List<Ghost> ghostsToRemove = new List<Ghost>();

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

            // 투명도 조절 (radius가 minRadius ~ minRadius+1 구간에서 서서히 투명해짐)
            float fadeStart = minRadius;
            float fadeEnd = minRadius + 1f; // 1 유닛 거리 내에서 투명해짐
            float alpha = 1f;

            if (ghost.radius <= fadeEnd)
            {
                alpha = Mathf.InverseLerp(fadeStart, fadeEnd, ghost.radius);
                // Mathf.InverseLerp(a,b,value) : value가 a일때 0, b일때 1 반환, 중간값 보간
                // 여기서는 radius가 minRadius이면 alpha=0(투명), minRadius+1이면 alpha=1(불투명)
            }

            // SpriteRenderer가 있다고 가정 (ghostPrefab에 SpriteRenderer 있어야 함)
            var sr = ghost.obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            // 완전히 투명해지면 삭제 대기
            if (alpha <= 0.01f)
            {
                ghostsToRemove.Add(ghost);
            }
        }

        // 완전히 투명한 유령 삭제
        foreach (var ghost in ghostsToRemove)
        {
            ghosts.Remove(ghost);
            Destroy(ghost.obj);
        }

        // 밀도 체크 후 추가 생성은 기존과 동일
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
