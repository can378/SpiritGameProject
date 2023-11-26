using System.Collections.Generic;
using UnityEngine;

public class MinimpController : MonoBehaviour
{
    public Transform playerTransform; // 실제 플레이어 Transform
    public float iconScale = 1.5f; // 아이콘의 크기 배율
    void Start()
    {
        // 아이콘의 크기를 설정된 배율로 조정
        transform.localScale = new Vector3(iconScale, iconScale, iconScale);
    }
    void Update()
    {
        // 아이콘의 위치를 플레이어의 현재 위치로 업데이트
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}