using System.Collections.Generic;
using UnityEngine;

public class GhostCircleSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public int ghostCount = 100;
    public float initialRadius = 5f;
    public float shrinkSpeed = 0.5f;

    private List<GhostInfo> ghosts = new List<GhostInfo>();
    private float currentRadius;

    class GhostInfo
    {
        public GameObject ghost;
        public float angleOffset;
    }

    void Start()
    {
        currentRadius = initialRadius;

        for (int i = 0; i < ghostCount; i++)
        {
            float angle = i * Mathf.PI * 2 / ghostCount;
            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * currentRadius,
                Mathf.Sin(angle) * currentRadius,
                0
            );
            GameObject g = Instantiate(ghostPrefab, pos, Quaternion.identity, transform);
            ghosts.Add(new GhostInfo { ghost = g, angleOffset = angle });
        }
    }

    void Update()
    {
        currentRadius -= shrinkSpeed * Time.deltaTime;
        currentRadius = Mathf.Max(currentRadius, 0.5f); // 너무 작아지지 않게 제한

        foreach (var ghost in ghosts)
        {
            float x = Mathf.Cos(ghost.angleOffset) * currentRadius;
            float y = Mathf.Sin(ghost.angleOffset) * currentRadius;
            ghost.ghost.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}
