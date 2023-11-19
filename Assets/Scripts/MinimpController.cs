using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimpController : MonoBehaviour
{
    public Material miniMapMaterial;
    public Transform playerTransform;
    public float radius = 5.0f;
    void Update()
    {
        // 플레이어의 현재 위치를 Shader에 전달
        if (miniMapMaterial != null && playerTransform != null)
        {
            Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
            miniMapMaterial.SetVector("_PlayerPos", new Vector4(playerPos.x, playerPos.y, 0, 0));
            miniMapMaterial.SetFloat("_Radius", radius);
        }
    }
}
