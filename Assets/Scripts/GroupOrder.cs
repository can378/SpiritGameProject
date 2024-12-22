using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroupOrder : MonoBehaviour
{
    // 발 위치를 기준으로 정렬
    [SerializeField] Transform PivotPos;

    void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y,PivotPos.position.y/1000);
    }
}
