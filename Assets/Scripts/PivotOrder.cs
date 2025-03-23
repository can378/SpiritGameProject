using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PivotOrder : MonoBehaviour
{
    // Pivot�� �������� z���� ������ ����
    [SerializeField] Transform PivotPos;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, PivotPos.position.y/1000);
        
    }
}
